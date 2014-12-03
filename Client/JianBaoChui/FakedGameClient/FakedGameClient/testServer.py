#!/usr/local/bin/python
# coding: gbk

import sys,uuid,random,time


from twisted.application.service import Application
from twisted.python.log import ILogObserver, FileLogObserver
from twisted.python.logfile import DailyLogFile
from twisted.internet.defer import inlineCallbacks, Deferred


from BroadcastCommand import PlayerInfo,KOGameCommand,BetTopNCommand,ChallengeCommand
from TeamNode import TeamNode, BinaryTree
from twisted.internet import reactor, protocol
from twisted.protocols import basic
from twisted.python import log
from datetime import datetime
from twisted.application.reactors import Reactor

############### �������� ###############

global  ALL_ROOM_INFO #ȫ��������Ϣ
global  ALL_ROOM_APPLY #ȫ�������û�
global  SEPARATOR   #��Ϣ�ָ���
global  CONNECT_PORT #���Ӷ˿ں� 
global  AUTHCODE #��Ȩ��
global  ALL_TEAM  #�������
global  DEBUGERR   #�Ƿ������ʽ��Ϣ
ALL_TEAM = {} #TeamNode ���� ��������� ʾ����ALL_TEAM['B'][0].pLevel�� PlayerInfo���ͣ�Player = ALL_TEAM['B'][0].data

LEVEL={} #��Ϸ�������� ,name:��ʶ����,count,�����ͻ���
LEVEL[0]={'name':'A','count':1}
LEVEL[1]={'name':'B','count':2}
LEVEL[2]={'name':'C','count':4}
LEVEL[3]={'name':'D','count':8}
LEVEL[4]={'name':'E','count':16}
LEVEL[5]={'name':'F','count':32}
LEVEL[6]={'name':'G','count':64}
LEVEL[7]={'name':'H','count':128}
LEVEL[8]={'name':'I','count':256}
LEVEL[9]={'name':'J','count':512}

global  number_of_connections  #��ǰ��������
global  max_connections           #�����������
############### ��ʼ�� ###############

ALL_ROOM_INFO = {}
ALL_ROOM_APPLY = {}
ALL_TEAM = {} 
SEPARATOR  =  "^"
CONNECT_PORT = 8888
AUTHCODE  = 'yang'
DEBUGERR = True

number_of_connections = 0
max_connections = 5 


application = Application("myapp")
logFile = DailyLogFile("my.log", "log")
application.setComponent(ILogObserver, FileLogObserver(logFile).emit)
logFile.write('begin...\r\n')


log.startLogging(sys.stdout)
log.msg('begin...')


############### �������� ###############
def LogWrite(self,str,line=''):
        ''' ��¼��־  '''
        
        logstr = FormatlogMsg(self,str,line)
        logFile.write( logstr )
        if (DEBUGERR==True):
                log.msg( str )

def FormatlogMsg(self,replay='',line=''):
        ''' ��ʽ����־��ʽ '''
        target = self.transport.getHost();
        if (line==""):
                format = 'ip:' + target.host + ' time:' +datetime.now().strftime('%Y-%m-%d %H:%M:%S')
        else:
                format = 'ip:' + target.host + ' time:' +datetime.now().strftime('%Y-%m-%d %H:%M:%S') +' rec: '+ line   
                
        if (replay==""):
                return  format +' \r\n'
        else:
                return  format + ' rep:' + replay +' \r\n'
        

        
def RoomLogin_InAuth(logininfo):
        '''�����½��֤��Ϣ��֤���� '''
        info = logininfo.split("^")
        if(info[0]==AUTHCODE):
                return 1
        else:
                return 0

def GetRoomId_InAuth(logininfo):
        '''��ȡ������'''
        info = logininfo.split("^")
        roomid  = info[1]
        if(roomid!=""):         
                return roomid;
        else:
                return 0

def GetRoomType_InAuth(logininfo):
        ''' ��ȡָ��'''
        info = logininfo.split("^")
        return info[3]
        
def GetRoomData_InAuth(logininfo):
        ''' ��ȡָ��ֵ '''
        info = logininfo.split("^")
        return info[4]

def Checkinfo(self,line):
        ''' ��֤ͨ�����ݿ��Ŷ�  '''
        #LogWrite(self,'auth checking ...',line)
        
        auth = RoomLogin_InAuth(line)
        if(auth==0):
       #         LogWrite(self,'auth is error .',line)
                self.connectionLost('') 
        
       # LogWrite(self,'room checking ...',line) 
        roomid = GetRoomId_InAuth(line)
        if(roomid==0):                          
        #        LogWrite(self,'room is error .',line)                           
                self.connectionLost('') 


def switch(self,type,data,logininfo):
        #print '--'+type+'--'
        if(type=='0001'):
                Login(self,data,logininfo)#��½
        elif(type=='0002'):
                Apply(self,data,logininfo)#����
        elif(type=='0003'):
                Punches(self,data,logininfo)#��ȭ
        elif(type=='0004'):
                Bet(self,data,logininfo)#��ע
        elif(type=='0005'):
                Team(self,data,logininfo)#����
        elif(type=='0000'):
                Exit(self,data,logininfo)#�˳�
        else:
                Login(self,data,logininfo)#Ĭ��Ϊ��½
                
def ClientloseConnection(self,rid=''):
        ''' �Ͽ��ڿͻ������� '''
        global number_of_connections,ALL_ROOM_INFO,ALL_ROOM_APPLY
        try:
                if(rid != ''):
                        number_of_connections -= 1     
                        del ALL_ROOM_INFO[rid]
                        del ALL_ROOM_APPLY[rid]
                        self.factory.clients.remove(self)
                        LogWrite(self,'Room %s Bye bye ...' % rid)
                        LogWrite(self,'Total room is %d' % len(ALL_ROOM_INFO) )
                        LogWrite(self,'Total apply is %d' % len(ALL_ROOM_APPLY) )
                        self.sendLine('Room %s bye bye ...' % rid)
                else:
                        self.sendLine('Exit bye bye ...')
                self.transport.loseConnection()                 
        except:
         #      self.transport.loseConnection()
         #      print 'exit error ...'
                pass
def searchCommonFather(level,no,note):
    ''' ������ͬ���׵���һ������  '''
    plevel = note.pLevel
    pno = note.pNo
    Rnote = None

    for j in range(0,LEVEL[level]['count']):
        if(ALL_TEAM[LEVEL[level]['name']][j].pNo==pno and ALL_TEAM[LEVEL[level]['name']][j].pLevel==plevel ):
            if(j!=no):
                Rnote =  ALL_TEAM[LEVEL[level]['name']][j]
                break
    return Rnote

def Guess(p1,p2):
    ''' ����p1 p2 ˭ʤ����1������2����3��������1:g1ʤ��2:g2ʤ,0:ƽ  '''
    result = g1 - g2;
    if(result==-1 or result==2):
        return 1
    elif(result==0):
        return 0
    else:
        return 2
    
    
############### ���ܺ��� ###############

def Login(self,data,line):
        ''' �ͻ��˵�½ '''
        global ALL_ROOM_INFO
        Checkinfo(self,line)    
        AddRoom(self,line)

def AddRoom(self,logininfo):
        '''  �����µķ��䵽�����б� ''' 
        global number_of_connections,log
        flag = 0
        info = logininfo.split("^")
        roomid  = info[1]
        roomnick = info[2]      
        for room in ALL_ROOM_INFO.keys():
                if(room==roomid):
                        flag = 1
                        LogWrite(self,'%s have...' % room,logininfo) 
        if flag == 0:
                ALL_ROOM_INFO[roomid] = PlayerInfo(roomid,roomnick,state=2)
                number_of_connections += 1
                LogWrite(self,"%s Room +1" % roomid,logininfo)
                LogWrite(self,'Total room is %d' % len(ALL_ROOM_INFO),logininfo)
        SendRoomInfoMessage(self)
        
def Apply(self,data,line):
        ''' �ͻ��˱������� ''' 
        global ALL_ROOM_APPLY   
        Checkinfo(self,line)    
        flag = 0
        info = line.split("^")
        roomid  = info[1]
        roomnick = info[2]      
        for room in ALL_ROOM_APPLY.keys():
                if(room==roomid):
                        flag = 1
                        LogWrite(self,'%s apply have ...' % roomid,line)
        if flag == 0:
                                player = ALL_ROOM_INFO[roomid]
                                player.state = 3 
                                ALL_ROOM_INFO[roomid] = player
                                ALL_ROOM_APPLY[roomid]= ALL_ROOM_INFO[roomid]
                                LogWrite(self,'%s apply +1 ' % roomid,line)
                                LogWrite(self,'Total apply is %d' % len(ALL_ROOM_APPLY),line)
        SendRoomInfoMessage(self)       
        
def Bet(self,data,logininfo):
        LogWrite(self,'Bet...',logininfo)
        SendRoomInfoMessage(self)
        
def Team(self,data,logininfo):
        LogWrite(self,'Team...',logininfo)
        
        #E={} #ģ��ͻ���
        #for e in range(1,17):
        #    E[e]=e
        
        #LogWrite(self,'ALL_ROOM_APPLY :',ALL_ROOM_APPLY)
        
        E = ALL_ROOM_APPLY
        
        length = len(E)  #�ͻ�������
        LogWrite(self,'Client Count is %d' % length)
        gameround = 1 #��ʼ��Ϸ����
        for i in LEVEL:
            if length > LEVEL[i]['count']: 
                gameround=i+1 #ȷ������      
          
        LogWrite(self,'need %d round ' % gameround) #������� 
        
        #��Ϸ�������ݶ�������ʼ��
        for t in range(gameround,-1,-1):
            tmp = {}
            for j in range(0,LEVEL[t]['count']):
                key = '%s%d' % (LEVEL[t]['name'],j)
                name = '%s' % (key)
                tmp[j]=TeamNode(name)  
              
            ALL_TEAM[LEVEL[t]['name']]= tmp
        
        #��Ϸ�������ݶ�������ʼ��������ϵ
        for t in range(gameround,-1,-1):
            tmp = {}
            i = 0 
            for j in range(0,LEVEL[t]['count']): 
               
                childLevel = LEVEL[t+1]['name'] #�ӵļ�������
                
                #Ĭ��û���ӽڵ�
                leftchild  = None
                rightchild = None
        
                if(t != gameround):#���������ײ� 
                    leftchild  = ALL_TEAM[childLevel][i] #0 2 4 6
                    i=i+1
                    rightchild = ALL_TEAM[childLevel][i] #1 3 5 7
                i=i+1               
        
                #Ĭ�ϳ�ʼ��Ϊ����
                parentLevel = '-1'
                parentNo   = '-1'
                
                if(t-1>=0):#������������
                    parentLevel = LEVEL[t-1]['name'] #���ļ�������
                    parentNo   = j//2 
                
                Level  = LEVEL[t]['name'] #��ǰ��������
                
                ALL_TEAM[Level][j].lchild = leftchild
                ALL_TEAM[Level][j].rchild = rightchild
                ALL_TEAM[Level][j].pLevel = parentLevel
                ALL_TEAM[Level][j].pNo  =  parentNo
       
        #print sorted(ALL_TEAM) #������ʾ�����ʼ�����
        #print ALL_TEAM #������ʾ�����ʼ������
        i=0
        for e in E.keys():
            Level = LEVEL[gameround]['name']
            ALL_TEAM[Level][i].data = E[e] #��ʼ��PLAYER
            i=i+1

        #print ALL_TEAM 
        tree=BinaryTree(ALL_TEAM['A'][0])

        #�������ȫ�����
        #print   '    preorder:',   
        #tree.preorder(tree.root)        
        #print ALL_TEAM['B'][0].pLevel
        
        for g in range(gameround,-1,-1):
            for t in ALL_TEAM[LEVEL[g]['name']]:
                note = ALL_TEAM[LEVEL[g]['name']][t]
                if(note.pLevel!='-1'):
                        if(note.data!=None):
                                rivalPlayer = searchCommonFather(g,t,note)
                                ALL_ROOM_INFO[note.data.id].level= g
                                ALL_ROOM_INFO[note.data.id].levelno = t
                                #print type(Player)
                                if(Player.data!=None):
                                        ALL_ROOM_INFO[note.data.id].rivalid = rivalPlayer.data.id
                                        ALL_ROOM_INFO[note.data.id].rivallevelno = rivalPlayer.data.levelno
                                        
                #if(g!=gameround):
                    #pass
                #    print note.name,'��һ��:',note.pLevel,note.pNo,'����:',note.lchild.name,'�Һ���:',note.rchild.name
                #else:
                    #pass
                #    print note.name,'��һ��:',note.pLevel,note.pNo
        #i=0
        #for e in E.keys():
        #    Level = LEVEL[gameround]['name']
        #    print ALL_TEAM[Level][i].name,ALL_TEAM[Level][i].data.id
        #    i=i+1      
            
        SendRoomInfoMessage(self)           
        
def Punches(self,data,logininfo):
        ''' �ͻ��˳�ȭ����,�������Ӯ��� '''
        global ALL_ROOM_APPLY   
        Checkinfo(self,logininfo)       
        flag = 0
        info = logininfo.split("^")
        roomid  = info[1]
        roomnick = info[2]      
        for room in ALL_ROOM_APPLY.keys(): #����ǲ����ͻ���
                if(room==roomid):
                        flag = 1
        if flag == 1:
                #��ǰ�ͻ���
                player = ALL_ROOM_INFO[roomid]
                player.guess = data
                #���ֿͻ���
                rival  = ALL_TEAM[LEVEL[player.level]['name']][player.rivallevelno]
                rivalplayer = rival.data;
                
                if(rivalplayer.guess!=''):
                   result =  Guess(player.guess,rivalplayer.guess)
                    
                ALL_ROOM_INFO[roomid] = player
                ALL_ROOM_APPLY[roomid]=ALL_ROOM_INFO[roomid]
        SendRoomInfoMessage(self)        

def Exit(self,data,logininfo):
        ''' �ͻ����˳� '''
        global ALL_ROOM_APPLY   
        Checkinfo(self,logininfo)       
        flag = 0
        info = logininfo.split("^")
        roomid  = info[1]
        ClientloseConnection(self,roomid)
        SendRoomInfoMessage(self)

def SendArrayToClent(self,data):
        for c in self.factory.clients:
                for a in data:
                        c.sendLine(a)

def loopMessage (self,line):
        ''' ������Ϣѭ��  '''         
        global ALL_ROOM_INFO    
        type = GetRoomType_InAuth(line)
        data =  GetRoomData_InAuth(line)
        switch(self,type,data,line)
                                
def SendRoomInfoMessage(self):
        ''' ����ȫ���ͻ��˶���Ϣ��ָ���ͻ��� '''        
        global ALL_ROOM_INFO
        data = ''
        for r in ALL_ROOM_INFO.keys():          
                 player = ALL_ROOM_INFO[r]
                 data+=player.getCommand()

        for c in self.factory.clients:
            c.sendLine(data)
            
        #print self.transport.getHost() 
############### �̺߳��� ###############


            
    
        
    


  
############### �������� ###############

class PubProtocol(basic.LineReceiver):
    def __init__(self, factory):         
        self.factory = factory

    def connectionMade(self):  
        self.factory.clients.add(self)   

    def connectionLost(self,reason):
        ClientloseConnection(self,reason)               
        
    def lineReceived(self, line):
        if (DEBUGERR==True):
                print line
           
        
        #for c in self.factory.clients:
        #    c.sendLine(line)
            
        loopMessage(self,line)


class PubFactory(protocol.Factory):
    def __init__(self):
        self.clients = set()

    def buildProtocol(self, addr):
        return PubProtocol(self)


reactor.listenTCP(CONNECT_PORT, PubFactory())
reactor.run()

