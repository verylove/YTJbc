#!/usr/local/bin/python
# coding: gbk

import sys,uuid,random,time,errno,optparse,select,ConfigParser,string,threading,socket

from twisted.application.service import Application
from twisted.python.log import ILogObserver, FileLogObserver
from twisted.python.logfile import DailyLogFile
from twisted.internet.defer import inlineCallbacks, Deferred
from operator import itemgetter,attrgetter 

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
global  ALL_ROOM_BETS  #ȫ����ע�ķ���
global  ALL_TEAM_VS #PK����
global  tree   #������
global  controlThread #������
global  clostControl #�رտ�����



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

global is_port,is_maxconnect,is_maxapplys,is_debug
global autogame,intervaltime,bittime,takttime

global  number_of_connections  #��ǰ��������
global  max_connections        #�����������
global  max_applys        #�����Ϸ����
global  gamestart  #��Ϸ��ʼʱ��
global  gameend    #��Ϸ����ʱ��
global  autogame   #�Ƿ��Զ���Ϸ
############### ��ʼ�� ###############

ALL_ROOM_INFO = {}
ALL_ROOM_APPLY = {}
ALL_TEAM = {} #TeamNode ���� ��������� ʾ����ALL_TEAM['B'][0].pLevel�� PlayerInfo���ͣ�PlayerID = ALL_TEAM['B'][0].data
ALL_ROOM_BETS = []#��ע��� ��ʾ��ALL_ROOM_BETS=[(1,A,10),(2,B,20),(3,C,30)]
ALL_TEAM_VS = {} #��սPK��˫��
tree = None
autogame = None
controlThread = None
clostControl = False
ALL_TEAM_VS[0]=None
ALL_TEAM_VS[1]=None



autogame=0

is_port = ''
is_maxconnect = ''
is_maxapplys = ''
is_debug = ''

def ReadConig(self,data='',logininfo=''):
    '''  ��ȡ������Ϣ  '''
    print 'read config  ...'
    global is_port,is_maxconnect,is_maxapplys,is_debug
    global SEPARATOR,AUTHCODE,CONNECT_PORT,DEBUGERR,maxconnect,maxapplys,autogame,gamestart,gameend,intervaltime,bittime,takttime
    
    cf=ConfigParser.ConfigParser()
    cf.read("config.ini")
    
    #SEPARATOR  =  cf.get("baseconf", "separator")#"^"
    #is_port = cf.get("baseconf", "port")#8888
    #AUTHCODE  = cf.get("baseconf", "authcode")#'yang'
    is_maxconnect = cf.get("baseconf","maxconnect")
    is_maxapplys = cf.get("baseconf","maxapplys")
    is_autogame = cf.get("baseconf","isauto")#�Ƿ��Զ���Ϸ
    
    gamestart = cf.get("gameconf","gamestart") #��Ϸ��ʼʱ��
    gameend = cf.get("gameconf","gameend") #��Ϸ����ʱ��
    
    intervaltime=cf.get("gameconf","intervaltime") #KO ��  PK���г���Ϣʱ��
    bittime=cf.get("gameconf","bittime") #��עʱ��
    takttime=cf.get("gameconf","takttime") #�´���ע�ȴ�ʱ��
    
    
    #is_debug = cf.get("debug", "isdebug")#True

    #if(is_port==''):
    #    print 'Port is Empty !!'
    #    sys.exit(0)
    #else:
    #    CONNECT_PORT = string.atoi(is_port)
    
    SEPARATOR = "^"
    CONNECT_PORT = 8888
    AUTHCODE = "yang"
    
    if(is_maxconnect==''):
        print 'max connect is Empty !!'
        sys.exit(0)
    else:
         maxconnect = string.atoi(is_maxconnect)
    
    if(is_autogame==''):
        print 'isauto config is Empty !!'
        sys.exie(0)
    else:
        autogame = string.atoi(is_autogame)
    
    if(is_maxapplys==''):
        print 'max online Palyer is Empty !!'
        sys.exit(0)
    else:
         maxapplys = string.atoi(is_maxapplys)
 
    pointf = open("point.txt")             # ����һ���ļ�����
    roomline = pointf.readline()             # �����ļ��� readline()����
    while roomline:
        # print(line, end = '')������# �� Python 3��ʹ��
        roomline = pointf.readline()
        roompoints = roomline.split('=')
        if(roompoints[0]):
            for r in ALL_ROOM_INFO.keys():
                if(r==roompoints[0]):
                    ALL_ROOM_INFO[r].points = string.atoi(roompoints[1])
    pointf.close()

    #if(is_debug=='1'):
    #    DEBUGERR=True
    #else:
    #    DEBUGERR=False
    
    #DEBUGERR = False
    #echo_debug = 0
    DEBUGERR=True
    if(SEPARATOR=='' or CONNECT_PORT=='' or AUTHCODE=='' or DEBUGERR=='' or gamestart=='' or gameend=='' or intervaltime=='' or takttime==''):
        print 'config is error !!'
        sys.exit(0)

    
    intervaltime=string.atof(intervaltime)
    bittime=string.atof(bittime)
    takttime=string.atof(takttime)
    if(self!=''):
        SuccessMessage(self,'01','',True)
    return 

ReadConig('')
number_of_connections = 0
echo_debug = 3 #1����ʾ  2������Ϣ  3 ������Ϣ


application = Application("myapp")
logFile = DailyLogFile("my.log", "log")
application.setComponent(ILogObserver, FileLogObserver(logFile).emit)
logFile.write('begin...\r\n')


log.startLogging(sys.stdout)
log.msg('begin...')


############### �������� ###############
def LogWrite(self,str,line='',send=''):
        ''' ��¼��־  '''
        
        logstr = FormatlogMsg(self,str,line,send)
        logFile.write( logstr )
        if (DEBUGERR==True):
                if(str!=''):
                    log.msg( str )

def FormatlogMsg(self,display='',rec='',send=''):
        ''' ��ʽ����־��ʽ '''
        target = self.transport.getHost()        
        
        if (display==""):
                format = 'ip:' + target.host + ' time:' +datetime.now().strftime('%Y-%m-%d %H:%M:%S')
        else:
                format = 'ip:' + target.host + ' time:' +datetime.now().strftime('%Y-%m-%d %H:%M:%S') +' dis: '+ display 
            
        if (rec!=""):
                format = format +' rec: '+ rec   
        
        if (send!=""):
                format = format + ' send:' + send 
                    
        return  format +' \r\n'
        

        
def RoomLogin_InAuth(logininfo):
        '''�����½��֤��Ϣ��֤���� '''
        info = logininfo.split("^")
        print "%s,%s" % (info[0],AUTHCODE)
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
        have = 1
        auth = RoomLogin_InAuth(line)
        if(auth==0):
                LogWrite(self,'auth is error .',line)
                self.connectionLost('') 
                have = 0 
       # LogWrite(self,'room checking ...',line) 
        roomid = GetRoomId_InAuth(line)
        if(roomid==0):                          
                LogWrite(self,'room is error .',line)                           
                self.connectionLost('') 
                have = 0
        return have

def CheckHave(self,line):
        ''' �˷��������߷���  '''
        roomid = GetRoomId_InAuth(line)
        have = 0    
        for r in ALL_ROOM_INFO.keys():
            if(r==roomid):
                have=1
        return have
                 
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
                Bits(self,'0',logininfo)#������ע��ʼ  
        elif(type=='0006'):
                ChampionExit(self,data,logininfo)#�����˳���Ϸ 
        elif(type=='0010'):
                Team(self,data,logininfo)#������ʼ
        elif(type=='0011'):
                Reset(self,data,logininfo)#��������
        elif(type=='0012'):
                Reset(self,data,logininfo)#���ƶ���������
        elif(type=='0013'):
                Bits(self,data,logininfo)#���ƶ���ע����
        elif(type=='0014'):
                Notice(self,data,logininfo)#����˷���������Ϣ   
        elif(type=='0015'):
                Message(self,data,logininfo)#����˷�����Ϣ    
        elif(type=='0016'):
                ReadConig(self,data,logininfo)#����˷��Ͷ�ȡ������Ϣ            
        elif(type=='1000'):
                Test(self,data,logininfo)#���ƶ˲�����Ϣ
        elif(type=='1001'):
                QuitControl(self,data,logininfo)#�������˳�
        elif(type=='1002'):
                StartControl(self,data,logininfo)#����������
        elif(type=='1003'):
                EndGame(self,data,logininfo)#������Ϸ                
        elif(type=='0000'):
                Exit(self,data,logininfo)#�˳�
        else:
                Login(self,data,logininfo)#Ĭ��Ϊ��½
                
def ClientloseConnection(self,rid=''):
        ''' �Ͽ��ڿͻ������� '''
        global number_of_connections,ALL_ROOM_INFO,ALL_ROOM_APPLY
        try:
                if(rid != ''):
                        if(ALL_ROOM_INFO[rid]!=''):
                            number_of_connections -= 1     
                        del ALL_ROOM_INFO[rid]
                        del ALL_ROOM_APPLY[rid]
                        if(ALL_ROOM_APPLY[rid].rivalid!=''):
                            rivalidPlay = ALL_ROOM_APPLY[rid].rivalid
                            ALL_ROOM_APPLY[rivalidPlay].win=1
                            ALL_ROOM_APPLY[rivalidPlay].guess=''
                            ALL_ROOM_APPLY[rivalidPlay].state=5
                        #self.transport.loseConnection()
                        self.factory.clients.delClient(self)
                        LogWrite(self,'Room %s Bye bye ...' % rid)
                        LogWrite(self,'Total room is %d' % len(ALL_ROOM_INFO) )
                        LogWrite(self,'Total apply is %d' % len(ALL_ROOM_APPLY) )
                        #self.sendLine('Room %s bye bye ...' % rid)
                else:
                        #self.sendLine('Exit bye bye ...')
                #self.transport.loseConnection() 
                #self.connectionLost('')
                        #self.transport.loseConnection()
                        self.factory.clients.delClient(self)               
        except:
         #      self.transport.loseConnection()
         #      print 'exit error ...'
                pass
def searchCommonFather(level,no,note):
    ''' ������ͬ���׵���һ������  ,������һ�����ӵĽڵ� '''
    parentLevel = note.pLevel
    parentNo = note.pNo
    Rnote = None
    if(echo_debug>1):  
        print '���ҵ�:',parentLevel,parentNo,'��ǰ�ģ�',level,no
    for j in range(0,LEVEL[level]['count']):
        if(echo_debug>1):
            print "������:",ALL_TEAM[LEVEL[level]['name']][j].name,'����:',ALL_TEAM[LEVEL[level]['name']][j].data,'���ȼ�:',ALL_TEAM[LEVEL[level]['name']][j].pLevel,ALL_TEAM[LEVEL[level]['name']][j].pNo
        if(ALL_TEAM[LEVEL[level]['name']][j].pNo==parentNo and ALL_TEAM[LEVEL[level]['name']][j].pLevel==parentLevel ):
            if(j!=no):
                if(echo_debug>1):
                    print '����ͬ����'
                if(ALL_TEAM[LEVEL[level]['name']][j].data!=None):
                    if(echo_debug>1):
                        print "�ҵ�����:",ALL_TEAM[LEVEL[level]['name']][j].name
                    Rnote =  ALL_TEAM[LEVEL[level]['name']][j]
                    break
    if(echo_debug>1):
        print '----------'
    return Rnote

def Guess(p1,p2):
    ''' ����p1 p2 ˭ʤ����1������2����3��������1:g1ʤ��2:g2ʤ,0:ƽ  '''
    a1 = string.atoi(p1)
    a2 = string.atoi(p2)
    result = a1 - a2;
    if(result==-1 or result==2):
        return 1
    elif(result==0):
        return 0
    else:
        return 2

def ReTeam(winPlayer,rivalplayer):
    '''   ���·���,�޸ı���״̬����Ϸ״̬  ''' 
    global ALL_ROOM_INFO,ALL_TEAM,ALL_TEAM_VS,tree   
    
    playLeveTeam = LEVEL[winPlayer.level]['name']
    playLeveNo = winPlayer.levelno
    
    rivalplayerTeam = LEVEL[rivalplayer.level]['name']
    rivalplayerNo = rivalplayer.levelno
    
    if(echo_debug>1):
        print 'Ӯ�ҽڵ㣺',ALL_TEAM[playLeveTeam][playLeveNo].name
    pNo  = ALL_TEAM[playLeveTeam][playLeveNo].pNo
    pLevel = ALL_TEAM[playLeveTeam][playLeveNo].pLevel
    
    if(pLevel<=3):#�޸� �����׶α�ʶ
        if(echo_debug>1):            
            print 'pLevel:',pLevel
        if(pLevel==3):
            ALL_ROOM_INFO[winPlayer.id].stage =2
        elif(pLevel==2):
            ALL_ROOM_INFO[winPlayer.id].stage =3
        elif(pLevel==1):
            ALL_ROOM_INFO[winPlayer.id].stage =4
        elif(pLevel==0):#����Ӯ��
            if(echo_debug>1):
                print '������:',winPlayer.id
            ALL_ROOM_INFO[winPlayer.id].ischampion = 1
            #if(echo_debug>1):
            #    print "ALL_TEAM_VS:",ALL_TEAM_VS
            #if(ALL_TEAM_VS[1]!=None):#���ʤ������ֱ�ӽ����� ����PK
            #    ALL_TEAM_VS[0] = winPlayer.id
            #    ALL_ROOM_INFO[winPlayer.id].rivalid=ALL_TEAM_VS[1]
            #    ALL_ROOM_INFO[ALL_TEAM_VS[1]].rivalid=ALL_TEAM_VS[0]
            
            #    for p in ALL_ROOM_INFO.keys():#֪ͨ��Ϸ״̬ΪKO����
            #        ALL_ROOM_INFO[p].gstate=2
            #else:
            for p in ALL_ROOM_INFO.keys():#֪ͨ��Ϸ״̬ΪKO����
                ALL_ROOM_INFO[p].gstate=3
    
        ALL_TEAM[LEVEL[pLevel]['name']][pNo].data = winPlayer.id     
    
    else:
        ALL_TEAM[LEVEL[pLevel]['name']][pNo].data = winPlayer.id
    
    ALL_ROOM_INFO[winPlayer.id].state = 5
    ALL_ROOM_INFO[rivalplayer.id].state = 1
    
    ALL_ROOM_INFO[winPlayer.id].win = ''
    ALL_ROOM_INFO[rivalplayer.id].win = ''
    
    
    ALL_ROOM_INFO[winPlayer.id].rivalid=''
    ALL_ROOM_INFO[winPlayer.id].rivallevelno=''
    
    ALL_ROOM_INFO[winPlayer.id].level   = pLevel
    ALL_ROOM_INFO[winPlayer.id].levelno = pNo 
    
    #ALL_ROOM_INFO[player.id].guess = ''
    #ALL_ROOM_INFO[rivalplayer.id].guess = ''
    if(echo_debug>1):
        print 'pLevel:',pLevel,'pNo:',pNo,'ALL:',pLevel,pNo
        
    try:
        #�Զ�����
        for R in ALL_ROOM_INFO.keys():
            update = 0
            mePlevel = ALL_ROOM_INFO[R].level
            mePlevelNo = ALL_ROOM_INFO[R].levelno     
            for n in  range(0,LEVEL[mePlevel]['count']):
                if(ALL_TEAM[LEVEL[mePlevel]['name']][n].data!=None and n!=mePlevelNo):
                    update=1
                    if(DEBUGERR==True):
                        print '*** �����ݣ�',LEVEL[mePlevel]['name'],n
                #else:
                    #print '*** ������',n
            if(update==0 and ALL_ROOM_INFO[R].rivalid=='' and ALL_ROOM_INFO[R].guess==''):
                pNote = ALL_TEAM[LEVEL[mePlevel]['name']][mePlevelNo]
                ALL_ROOM_INFO[R].win=1
                ALL_ROOM_INFO[R].level=pNote.pLevel
                ALL_ROOM_INFO[R].levelno=pNote.pNo
                ALL_TEAM[LEVEL[pNote.pLevel]['name']][pNote.pNo].data=R
                ALL_TEAM[LEVEL[mePlevel]['name']][mePlevelNo].data=None
                
                if(pNote.pLevel==3):
                     ALL_ROOM_INFO[R].stage=2
                elif(pNote.pLevel==2):
                     ALL_ROOM_INFO[R].stage=3
                elif(pNote.pLevel==1):   
                     ALL_ROOM_INFO[R].stage=4
                     #ALL_TEAM_VS[1]=R  #�������PK����ֱ��ս����
                elif(pNote.pLevel==0):
                     ALL_ROOM_INFO[R].stage=6
                     ALL_TEAM_VS[1]=R  #�������PK����ֱ��ս����    
    except:
        pass
    
    #���²��Ҷ���
    rivalNode = searchCommonFather(pLevel,pNo,ALL_TEAM[LEVEL[pLevel]['name']][pNo])
    if(rivalNode!=None):
        if(rivalNode.data!=''):
            if(echo_debug>1):
                print '����:',rivalNode.data
            rivalPlayer = ALL_ROOM_INFO[rivalNode.data]
            ALL_ROOM_INFO[winPlayer.id].rivalid = rivalNode.data
            ALL_ROOM_INFO[winPlayer.id].rivallevelno = rivalPlayer.levelno   
            
            ALL_ROOM_INFO[rivalNode.data].rivalid = winPlayer.id
            ALL_ROOM_INFO[rivalNode.data].rivallevelno = winPlayer.levelno
              

  
def PK(self,roomid,data):
    '''  PK�������  '''
    print 'PKTeam...'  
    

    player =ALL_TEAM_VS[0]
    rivalplayer = ALL_TEAM_VS[1]

        
    ALL_ROOM_INFO[roomid].guess=data
    tmp = ALL_ROOM_INFO[roomid].rivalid
    loseRoom = None
    gamein = None
    if (tmp!=''):
        if(ALL_ROOM_INFO[tmp].guess!='' and ALL_ROOM_INFO[tmp].state==4):
            gamein = 1
            result = Guess(ALL_ROOM_INFO[player].guess,ALL_ROOM_INFO[rivalplayer].guess)       
            if(result==0):#ƽ
                ALL_ROOM_INFO[player].win=3
                ALL_ROOM_INFO[rivalplayer].win=3
                ALL_ROOM_INFO[player].state=5
                ALL_ROOM_INFO[rivalplayer].state=5
            elif(result==1):#����ʤ
                ALL_ROOM_INFO[player].win=1
                ALL_ROOM_INFO[rivalplayer].win=2
                ALL_ROOM_INFO[player].state=5
                ALL_ROOM_INFO[rivalplayer].state=5
                loseRoom = rivalplayer
                ALL_ROOM_INFO[player].ischampion=1
                
                ALL_ROOM_INFO[player].points= ALL_ROOM_INFO[player].points + ALL_ROOM_INFO[rivalplayer].betpoints
                ALL_ROOM_INFO[rivalplayer].points= ALL_ROOM_INFO[rivalplayer].points - ALL_ROOM_INFO[rivalplayer].betpoints
                
                if(ALL_ROOM_INFO[player].gstate==2):#�����KO����״̬����ΪKO����
                    for p in ALL_ROOM_INFO.keys():#֪ͨ��Ϸ״̬ΪKO����
                        ALL_ROOM_INFO[p].gstate=3
            elif(result==2):#��ս��ʤ
                ALL_ROOM_INFO[player].win=2
                ALL_ROOM_INFO[rivalplayer].win=1
                ALL_ROOM_INFO[player].state=5
                loseRoom = player
                ALL_ROOM_INFO[rivalplayer].state=5
                ALL_ROOM_INFO[rivalplayer].ischampion=1
                
                ALL_ROOM_INFO[rivalplayer].points= ALL_ROOM_INFO[rivalplayer].points + ALL_ROOM_INFO[rivalplayer].betpoints
                ALL_ROOM_INFO[player].points= ALL_ROOM_INFO[player].points - ALL_ROOM_INFO[rivalplayer].betpoints
        
        
                if(ALL_ROOM_INFO[player].gstate==2):#�����KO����״̬����ΪKO����
                    for p in ALL_ROOM_INFO.keys():#֪ͨ��Ϸ״̬ΪKO����
                        ALL_ROOM_INFO[p].gstate=3
        else:
            ALL_ROOM_INFO[player].state=4
            ALL_ROOM_INFO[rivalplayer].state=4
    SendRoomInfoMessage(self)
    
    if(loseRoom!=None):
        ALL_ROOM_INFO[loseRoom].state=1
        print 'ʧ�ܷ��䣺',loseRoom
        SendRoomInfoMessage(self)
    else:
        if(gamein==1):
            ALL_ROOM_INFO[player].guess=''
            ALL_ROOM_INFO[rivalplayer].guess=''
            SendRoomInfoMessage(self)
def GameStop():
    print ''' ��Ϸ����  '''
    time.sleep(3)    
    reactor.stop()           
         
############### ���ܺ��� ###############

        
def Login(self,data,line):
        ''' �ͻ��˵�½ '''
        global ALL_ROOM_INFO
        hinfo = Checkinfo(self,line)
        if(hinfo==0):
            return 
        AddRoom(self,line)

def AddRoom(self,logininfo):
        '''  �����µķ��䵽�����б� ''' 
        global number_of_connections,log,maxconnect
        if(number_of_connections>=maxconnect):
           if(echo_debug>1):
               print 'max connect ...'
           ErrorMessage(self,05,logininfo)
           #self.connectionLost('max connect ...')
           ClientloseConnection(self)
        else:
            number_of_connections += 1  
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
                    LogWrite(self,"%s Room +1" % roomid,logininfo)
                    LogWrite(self,'Total room is %d' % len(ALL_ROOM_INFO),logininfo)
            SendRoomInfoMessage(self)
        
def Apply(self,data,line):
        ''' �ͻ��˱������� ''' 
        global ALL_ROOM_APPLY   
        hinfo = Checkinfo(self,line)
        if(hinfo==0):
            return  
        have = CheckHave(self,line)
        if(have==0):
           LogWrite(self,'no have room !',line) 
           self.connectionLost('')  
           return
          
        flag = 0
        info = line.split("^")
        roomid  = info[1]
        roomnick = info[2]      
        

        for room in ALL_ROOM_APPLY.keys():
            if(ALL_ROOM_APPLY[room].gstate==1 or ALL_ROOM_APPLY[room].gstate==2 or ALL_ROOM_APPLY[room].gstate==3 or ALL_ROOM_APPLY[room].gstate==4):
                ErrorMessage(self,04,line)
                return 
        
        for room in ALL_ROOM_APPLY.keys():
                if(room==roomid):
                        flag = 1
                        LogWrite(self,'%s apply have ...' % roomid,line)
        if flag == 0:
                                player = ALL_ROOM_INFO[roomid]
                                player.nickname = roomnick
                                player.state = 3 
                                ALL_ROOM_INFO[roomid] = player
                                ALL_ROOM_APPLY[roomid]= ALL_ROOM_INFO[roomid]
                                LogWrite(self,'%s apply +1 ' % roomid,line)
                                LogWrite(self,'Total apply is %d' % len(ALL_ROOM_APPLY),line)
        #SuccessMessage(self,"01",line,True)
        SendRoomInfoMessage(self)       
        
def Bet(self,data,logininfo):
        LogWrite(self,'Bet...',logininfo)
        
        global ALL_ROOM_INFO,ALL_ROOM_BETS
        hinfo = Checkinfo(self,logininfo)
        if(hinfo==0):
            return    
        have = CheckHave(self,logininfo)
        if(have==0):
           LogWrite(self,'no have room !',logininfo) 
           self.connectionLost('')
           return 
           
        info = logininfo.split("^")
        roomid  = info[1]
        roomnick = info[2]   
           
        tmp=()
        now = time.time()
        have = 0
        me = None
        for b  in ALL_ROOM_BETS:
            if(b[0]==roomid):
                have = 1
                me = b
                
#       print 'have...'
        if(ALL_ROOM_INFO[roomid].gstate!=4):
            ErrorMessage(self,03,logininfo)
            return 
        
        if(ALL_ROOM_INFO[roomid].ischampion==1):
            ErrorMessage(self,02,logininfo)
            return 
        
       
        if(have==1):   #�����ע�����ۼ�������ע���������޸���עʱ��             
            now = b[1]
            points = b[2]+string.atoi(data)
            tmp = (roomid,now,points)
            ALL_ROOM_BETS.remove(me)
            ALL_ROOM_BETS.append(tmp)
            if(ALL_ROOM_INFO[roomid].betpoints>3000):
                ALL_ROOM_INFO[roomid].betpoints = 0
            else:
                ALL_ROOM_INFO[roomid].betpoints= ALL_ROOM_INFO[roomid].betpoints + string.atoi(data)
#            print '++'
        else:
            points = string.atoi(data)
            tmp = (roomid,now,points)   
            ALL_ROOM_BETS.append(tmp)             
            ALL_ROOM_INFO[roomid].betpoints= string.atoi(data)
 #           print '--'
            
        ALL_ROOM_APPLY[roomid] = ALL_ROOM_INFO[roomid] #ͬ����������
        SendRoomInfoMessage(self)
        
def Team(self,data,logininfo):
        global ALL_ROOM_APPLY,ALL_ROOM_INFO,ALL_ROOM_BETS,tree
        LogWrite(self,'Team...',logininfo)
        
        ReadConig(self,data,logininfo)
        #E={} #ģ��ͻ���
        #for e in range(1,17):
        #    E[e]=e
        
        #LogWrite(self,'ALL_ROOM_APPLY :',ALL_ROOM_APPLY)
        
        E = ALL_ROOM_APPLY

        
        length = len(E)  #�ͻ�������
        if(length<2):
                if(DEBUGERR==True):
                    print 'Client less ...'
                return
        
      
        LogWrite(self,'Client Count is %d' % length)

            
        gameround = 1 #��ʼ��Ϸ����
        for i in LEVEL:
            if length > LEVEL[i]['count']:  #2 4 8 16 32 64 128 256 512
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
                    parentLevel = t-1#LEVEL[t-1]['name'] #���ļ�������
                    parentNo   = j//2 
                
                Level  = LEVEL[t]['name'] #��ǰ��������
                
                ALL_TEAM[Level][j].lchild = leftchild
                ALL_TEAM[Level][j].rchild = rightchild
                ALL_TEAM[Level][j].level  = t
                ALL_TEAM[Level][j].levelno= j
                ALL_TEAM[Level][j].pLevel = parentLevel
                ALL_TEAM[Level][j].pNo  =  parentNo
       
        #print sorted(ALL_TEAM) #������ʾ�����ʼ�����
        print ALL_TEAM #������ʾ�����ʼ������        
        p=[]
        for x in range(0,length):
                p.append(x)
        random.shuffle(p)
        
        #print ALL_TEAM[LEVEL[gameround]['name']]
        #print p,E
        #print "count:",len(E)
        i=0
        for e in E.keys():
            Level = LEVEL[gameround]['name']
            ALL_TEAM[Level][p[i]].data = E[e].id #��ʼ��PLAYER
            i=i+1

        #print ALL_TEAM 
        tree=BinaryTree(ALL_TEAM['A'][0])

        #�������ȫ�����
        if(echo_debug>1):
            print   'preorder:',   
            tree.preorder(tree.root)        
            #print ALL_TEAM['B'][0].pLevel
            print '---------------'
        
        
        for g in range(gameround,-1,-1):
            for t in ALL_TEAM[LEVEL[g]['name']]:
                note = ALL_TEAM[LEVEL[g]['name']][t]
                if(note.pLevel!='-1'):
                        if(note.data!=None):
                                print 'pLevel:',g,'pNo:',t,'ALL:',g,t
                                rivalPlayer = searchCommonFather(g,t,note)
                                ALL_ROOM_INFO[note.data].level= g
                                ALL_ROOM_INFO[note.data].levelno = t
                                ALL_ROOM_INFO[note.data].gstate = 1
                                #print type(Player)
                                if(rivalPlayer!=None):
                                    if(rivalPlayer.data!=None):
                                        ALL_ROOM_INFO[note.data].rivalid = rivalPlayer.data                                        
                                        ALL_ROOM_INFO[note.data].rivallevelno = t
                                else:#���û�ж���ֱ�ӽ���
                                    ALL_ROOM_INFO[note.data].win=1
                                    ALL_ROOM_INFO[note.data].state=5
                                    #ALL_ROOM_INFO[note.data].level=note.pLevel
                                    #ALL_ROOM_INFO[note.data].levelno=note.pNo
                                    #if(note.pLevel==3):
                                    #    ALL_ROOM_INFO[note.data].stage=2
                                    #elif(note.pLevel==2):
                                    #    ALL_ROOM_INFO[note.data].stage=3
                                    #elif(note.pLevel==1):   
                                    #    ALL_ROOM_INFO[note.data].stage=4
                                    #elif(note.pLevel==0):
                                    #    ALL_ROOM_INFO[note.data].stage=5
                                    #ALL_TEAM[LEVEL[note.pLevel]['name']][note.pNo].data=note.data
                                    #ALL_TEAM[LEVEL[g]['name']][t].data=None
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
        ''' �ͻ��˳�ȭ���� '''
        global ALL_ROOM_APPLY,ALL_TEAM_VS,ALL_ROOM_INFO,tree
        hinfo = Checkinfo(self,logininfo)
        if(hinfo==0):
            return 
        have = CheckHave(self,logininfo)
        if(have==0):
           LogWrite(self,'no have room !',logininfo) 
           self.connectionLost('')
           return   
                  
        flag = 0
        info = logininfo.split("^")
        roomid  = info[1]
        roomnick = info[2]     
        

        
        for room in ALL_ROOM_APPLY.keys(): #����ǲ����ͻ���
                if(room==roomid):
                        flag = 1
        if(flag==1):
            if(ALL_ROOM_INFO[roomid].win==1 and ALL_ROOM_INFO[roomid].guess=='' and ALL_ROOM_INFO[roomid].rivalid==''):
                ErrorMessage(self,01,logininfo)
                return
            
        if(flag==1 and ALL_ROOM_INFO[roomid].stage==5):#����PK��ȭ���
                PK(self,roomid,data);
                return               
        loseRoom = None
        if flag == 1 and tree!=None:
                #��ǰ�ͻ���
                player = ALL_ROOM_INFO[roomid]
                player.guess = data
                #if(echo_debug>1):
                #    print player.id,':',ALL_ROOM_INFO[player.id].state,player.rivalid,':',ALL_ROOM_INFO[player.rivalid].state           
                ALL_ROOM_INFO[player.id].state = 4
                
                #���ֿͻ���
                
                #rival  = ALL_TEAM[LEVEL[player.level]['name']][player.rivallevelno]
                if(player.rivalid!=''):
                    rivalplayer = ALL_ROOM_INFO[player.rivalid]                 

                    if(rivalplayer.guess!='' and rivalplayer.state==4):
                       result =  Guess(player.guess,rivalplayer.guess)
                      
                       if(echo_debug>1):
                            c=''
                            d=''
                            r=''
                            if(player.guess=="1"):
                                c = '����'
                            elif(player.guess=="2"):
                                c= "��"
                            elif(player.guess=="3"):
                                c= "��"
                                
                            if(rivalplayer.guess=="1"):
                                d = '����'
                            elif(rivalplayer.guess=="2"):
                                d= "��"
                            elif(rivalplayer.guess=="3"):
                                d= "��"
                            if(result==0):
                                r="ƽ��"
                            elif(result==1):
                                r=player.id
                            elif(result==2):
                                r=rivalplayer.id
                            print player.id,"��:",c,"����",rivalplayer.id,'��',d,r,"ʤ"
                            
                       if(result==0):#ƽ��
                           ALL_ROOM_INFO[player.id].win = 3
                           ALL_ROOM_INFO[rivalplayer.id].win = 3
                           
                           ALL_ROOM_INFO[player.id].state = 5
                           ALL_ROOM_INFO[rivalplayer.id].state = 5
                           
                       elif(result==1):#playerʤ
                           ALL_ROOM_INFO[player.id].win = 1
                           ALL_ROOM_INFO[rivalplayer.id].win = 2
                           loseRoom =  rivalplayer.id
                           
                           tTeam = LEVEL[player.level]['name']
                           tNo = player.levelno
                           
                           rtTeam = LEVEL[rivalplayer.level]['name']
                           rtNo = rivalplayer.levelno
                           
                           if(echo_debug>1):
                               print 'Team2:',tTeam,'No:',tNo 
                           
                           #ALL_ROOM_INFO[player.id].level   = ALL_TEAM[tTeam][tNo].pLevel
                           #ALL_ROOM_INFO[player.id].levelno = ALL_TEAM[tTeam][tNo].pNo 
                         
                           ALL_TEAM[tTeam][tNo].data=None
                           ALL_TEAM[rtTeam][rtNo].data=None
                           
                           ALL_ROOM_INFO[player.id].state = 5
                           ALL_ROOM_INFO[rivalplayer.id].state = 5
                           
                           SendRoomInfoMessage(self)  
 
                           ReTeam(player,rivalplayer)#���·��鲢�޸�ȫ����Ϸ״̬                      
                            
                       elif(result==2):#rivalpalyerʤ
                           ALL_ROOM_INFO[player.id].win = 2
                           ALL_ROOM_INFO[rivalplayer.id].win = 1                          
                           loseRoom =  player.id
                           
                           tTeam = LEVEL[rivalplayer.level]['name']
                           tNo = rivalplayer.levelno
                           
                           rtTeam = LEVEL[player.level]['name']
                           rtNo = player.levelno
                           
                           if(echo_debug>1):
                               print 'Team2:',tTeam,'No:',tNo 
                           
                           #ALL_ROOM_INFO[rivalplayer.id].level   = ALL_TEAM[tTeam][tNo].pLevel
                           #ALL_ROOM_INFO[rivalplayer.id].levelno = ALL_TEAM[tTeam][tNo].pNo                           
                           
                           ALL_TEAM[tTeam][tNo].data=None
                           ALL_TEAM[rtTeam][rtNo].data=None  
                           
                           ALL_ROOM_INFO[player.id].state = 5
                           ALL_ROOM_INFO[rivalplayer.id].state = 5
                           
                           SendRoomInfoMessage(self)

                           ReTeam(rivalplayer,player)#���·��鲢�޸�ȫ����Ϸ״̬                          

                    else:
                         #pass                             
                        ALL_ROOM_INFO[rivalplayer.id].state=4;   
                        ALL_ROOM_INFO[rivalplayer.id].guess=''                      
                        ALL_ROOM_INFO[rivalplayer.id].win = ''
                        ALL_ROOM_INFO[player.id].win = '' 
                        pass
                    
                    #ALL_ROOM_INFO[player.rivalid].state = 4
                     
                   #print player.id,':',ALL_ROOM_INFO[player.id].state,player.rivalid,':',ALL_ROOM_INFO[player.rivalid].state 
                    ALL_ROOM_APPLY[rivalplayer.id]=ALL_ROOM_INFO[rivalplayer.id]
             
                
                ALL_ROOM_APPLY[player.id]=ALL_ROOM_INFO[player.id]
        
        if((echo_debug>2 and tree!=None) or (echo_debug>1 and tree!=None)):
            print  'reTeam Preorder:'   
            tree.preorder(tree.root)        
            print '---------------'         
    
        SendRoomInfoMessage(self) 
        
        #if(loseRoom!=None):
        #    ALL_ROOM_INFO[loseRoom].state=1
        #    print 'ʧ�ܷ��䣺',loseRoom
         #   SendRoomInfoMessage(self)     
          
def Bits(self,data,logininfo):
    ''' ��ע��������  '''
    global ALL_ROOM_BETS,ALL_TEAM_VS,ALL_ROOM_INFO
    champion = ''
    if(data=='1'):
        #for room in ALL_ROOM_INFO.keys(): #����ǲ����ͻ���
        #        ALL_ROOM_INFO[room].gstate=5 #��ע������PK����
        #PK()
        sortBits = sorted(ALL_ROOM_BETS,key=lambda sbits:sbits[2])
        print 'sortBits:',sortBits
        lenSortBits = len(sortBits)
        defier = None
        tmpSameBitsRoom = []
        if(lenSortBits>1):
            for t in range(0,lenSortBits-1):
                if(sortBits[t][2]==sortBits[lenSortBits-1][2]):
                    tmpSameBitsRoom.append(sortBits[t])
                tmpSameBitsRoom.append(sortBits[lenSortBits-1])
        if(tmpSameBitsRoom!=''):
            tmpSortBits =  sorted(tmpSameBitsRoom, key=itemgetter(2,1))
            defier = tmpSortBits[0][0]
        else:
            defier = sortBits[lenSortBits-1][0]
        if(echo_debug>1):     
            print ALL_ROOM_BETS
        
        if(lenSortBits>=1):
            ALL_TEAM_VS[1] =defier
            room = ALL_TEAM_VS[0]
            rivalroom = ALL_TEAM_VS[1]
            print 'PK room:',ALL_TEAM_VS[0],' vs ',ALL_TEAM_VS[1]
            for r in ALL_ROOM_INFO.keys(): #����ǲ����ͻ���
                ALL_ROOM_INFO[r].gstate=5 #��ע������PK����
                #ALL_ROOM_INFO[r].betpoints=0 
                #ALL_ROOM_INFO[room].state=1
                
            ALL_ROOM_INFO[room].stage = 5
            ALL_ROOM_INFO[room].rivalid =rivalroom
            ALL_ROOM_INFO[room].rivallevelno =1
            ALL_ROOM_INFO[room].win =''
            ALL_ROOM_INFO[room].guess =''
            ALL_ROOM_INFO[room].ischampion =0
            ALL_ROOM_INFO[room].state = ''
            
            ALL_ROOM_INFO[rivalroom].rivalid=room
            ALL_ROOM_INFO[rivalroom].rivallevelno=0
            ALL_ROOM_INFO[rivalroom].win =''
            ALL_ROOM_INFO[rivalroom].guess =''
            ALL_ROOM_INFO[rivalroom].ischampion=0
            

                
        #print ALL_TEAM_VS    
        SendRoomInfoMessage(self)
    else:
        ALL_ROOM_BETS=[]
        for room in ALL_ROOM_INFO.keys(): #����ǲ����ͻ���
            ALL_ROOM_INFO[room].gstate=4 #��ע��ʼ��PK��ʼ
            ALL_ROOM_INFO[room].rivalid=''
            ALL_ROOM_INFO[room].rivallevelno=''
            ALL_ROOM_INFO[room].levelno=''
            ALL_ROOM_INFO[room].level=''
            ALL_ROOM_INFO[room].betpoints=0
            ALL_ROOM_INFO[room].guess=''
            ALL_ROOM_INFO[room].win='' 
            ALL_ROOM_INFO[room].stage=5 
            ALL_ROOM_INFO[room].state = 1   
            if(ALL_ROOM_INFO[room].ischampion==1):
                         ALL_ROOM_INFO[room].stage = 5 #��������Ϊ ��ս��
                         ALL_ROOM_INFO[room].state = '' 
                         ALL_TEAM_VS[0]=room
            
    SendRoomInfoMessage(self)    
def Test(self,data,logininfo):
    '''  ���ƶ˲�����Ϣ '''
    global clostControl
   
    if(clostControl==True):
        self.sendLine('**exit**')
    else:
        global ALL_ROOM_INFO
        data = ''
        for r in ALL_ROOM_INFO.keys():          
                 player = ALL_ROOM_INFO[r]
                 data+=player.getCommand()
        self.sendLine(data)
    #SendRoomInfoMessage(self)
def QuitControl(self,data,logininfo):
    '''  ���ƶ��˳� '''
    global clostControl
    print "quit controlThread ..."
    clostControl = True
    LogWrite(self,'QuitControl success')
    #self.sendLine('QuitControl success')
    
def StartControl(self,data,logininfo):
    global controlThread , clostControl
    clostControl = False
    controlThread = MyThread()
    controlThread.start()  
    #self.sendLine('StartControl success')  
    LogWrite(self,'StartControl success')
    
def ChampionExit(self,data,logininfo):
      '''  �����˳���Ϸ,��Ϸ�������˳� '''
    LogWrite(self,'Champion exit,3 seconds after the server offline')
    Notice(self,'Champion exit,3 seconds after the server offline',logininfo)
    SuccessMessage(self,"04",logininfo)#������Ϸ�˳��ɹ�
    SendRoomInfoMessage(self)#���ͻ�����Ϣ
    t = threading.Thread(target=GameStop)
    t.start()
       
def EndGame(self,data,logininfo):
    '''  ��Ϸ�������˳� '''
    LogWrite(self,'3 seconds after the server offline')
    Notice(self,'3 seconds after the server offline',logininfo)
    SuccessMessage(self,"03",logininfo)#������Ϸ�˳��ɹ�
    SendRoomInfoMessage(self)#���ͻ�����Ϣ
    t = threading.Thread(target=GameStop)
    t.start()

    
def Notice(self,data,logininfo,Me=False):
    ''' ���淢��  '''
    data = "##01^%s##" % data
    if(Me==True):
        self.sendLine(data)
    else:
        SendRoomInfoMessage(self,data)
    
def Message(self,data,logininfo,Me=False):
    ''' ������Ϣ '''
    data = "##02^%s##" % data
    if(Me==True):
        self.sendLine(data)
    else:
        SendRoomInfoMessage(self,data)
    
def ErrorMessage(self,data,logininfo,Me=False):
    ''' ������Ϣ����   '''
    data = "##03^%s##" % data
    if(Me==True):
        self.sendLine(data)
    else:
        SendRoomInfoMessage(self,data)

def SuccessMessage(self,data,logininfo,Me=False):
    ''' ��ȷ��Ϣ����   '''
    data = "##04^%s##" % data
    if(Me==True):
        self.sendLine(data)
    else:
        SendRoomInfoMessage(self,data)   

def Reset(self,data,logininfo):
    ''' ���÷�����״̬ '''
    global ALL_ROOM_APPLY,ALL_ROOM_INFO,ALL_TEAM,ALL_TEAM_VS,number_of_connections
    ALL_ROOM_APPLY={}
    ALL_ROOM_INFO={}
    ALL_TEAM = {}
    ALL_TEAM_VS = {}
    number_of_connections=0
    self.factory.clients.clear()
    if(DEBUGERR==True):
        print '��Ϸ����ȫ������...'
    Message(self,'Server data reset ',logininfo)
    #SendRoomInfoMessage(self) 

def Exit(self,data,logininfo):
        ''' �ͻ����˳� '''
        global ALL_ROOM_APPLY   
        hinfo = Checkinfo(self,logininfo)
        if(hinfo==0):
            return       
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
                                
def SendRoomInfoMessage(self,data=None):
        ''' ����ȫ���ͻ��˶���Ϣ��ָ���ͻ��� '''        
        global ALL_ROOM_INFO
        if(data==None):
            data = ''
            for r in ALL_ROOM_INFO.keys():          
                     player = ALL_ROOM_INFO[r]
                     data+=player.getCommand()

        for c in self.factory.clients:
            if(DEBUGERR==True):
                LogWrite(self,'','',data)
            c.sendLine(data)
            
        #print self.transport.getHost() 
############### �̺߳��� ###############
class MyThread(threading.Thread):
    
    def run(self):
        global gamestart,gameend,autogame
        
        gamestartlist = gamestart.split(':')
        gameendlist = gameend.split(':')
        
        gamestartflag = None
        gameendflag = None
        
        GAMESTART = AUTHCODE+'^GM^GM^0010^0'+"\r\n"
        GAMEOVER  = AUTHCODE+'^GM^GM^0011^0'+"\r\n"
        
        BITSSTART = AUTHCODE+'^GM^GM^0013^0'+"\r\n"
        BITSEND   = AUTHCODE+'^GM^GM^0013^1'+"\r\n"
                
        time.sleep(2)
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)  
        sock.connect(('localhost', CONNECT_PORT))  
        sock.setblocking(0)
        #autogame
        
        startt = None
        endt   = None
        startbitt = None
        endbitt = None
             
        while True:
             try:
                 data = sock.recv(1024)
             except socket.error,e:
                 if e.args[0] == errno.EWOULDBLOCK:
                     data = ''
             
             
             #print 'data:%s:' % data
             if '**exit**' in data:
                print 'exit thread ...'
                break  
                     
             
             nTime  = time.strftime("%H:%M:%S",time.localtime())
            
             nTimeH = string.atoi(time.strftime("%H",time.localtime()))
             nTimeM = string.atoi(time.strftime("%M",time.localtime()))
             nTimeS = string.atoi(time.strftime("%S",time.localtime()))
            
             sSTimeH = string.atoi(gamestartlist[0])
             sSTimeM = string.atoi(gamestartlist[1])
             sSTimeS = string.atoi(gamestartlist[2])
            
             sETimeH = string.atoi(gameendlist[0])
             sETimeM = string.atoi(gameendlist[1])
             sETimeS = string.atoi(gameendlist[2])
            
             if(nTimeH>=sSTimeH):
                 if(nTimeM>=sSTimeM):
                     if(nTimeS>=sSTimeS):
                         gamestartflag = True
              
             if(nTimeH>=sETimeH):
                 if(nTimeM>=sETimeM):
                     if(nTimeS>=sETimeS):
                         gameendflag = True
                         
            
             print nTimeS
             sock.send("yang^GM^GM^1000^0\r\n")
                    
             if not data:  
                 pass            
             else:
                time.sleep(1)
                if(echo_debug>2):
                    print 'autogame:',autogame
                if autogame==1:
                    if(echo_debug>2):
                        print 'in control...'
                        #��Ϸ״̬Ϊ KO��ʼ �� ���״̬Ϊ�ѱ������ڵ���2�� 
                    if(self.getPlayCount(data,'1')==0 and self.getPlayCount(data,'3',6)>=2):
                        if(echo_debug>2):
                            print '��Ҫ��ʼ��Ϸ ������'
                        if(gamestartflag==True and gameendflag==None):
                            sock.send(GAMESTART)
                        #��Ϸ��������1�� �� ��Ϸ״̬û��PK��ʼ
                    elif(self.getPlayCount(data,'1',4)>=1 and self.getPlayCount(data,'4')==0):
                        if(echo_debug>2):
                            print '��ҪPK��ʼ������'
                        nowt = time.time()
                        if(endt==None):
                            if(echo_debug>2):
                                print '�޽���'
                            if(startt==None): 
                                if(echo_debug>2):  
                                    print '��start'     
                                startt = nowt
                                endt   = startt+intervaltime*60
                        else:
                            if(echo_debug>2):
                                print 'PK��ʼ,�н���',time.strftime("%H:%M:%S",time.localtime(nowt)),'--',time.strftime("%H:%M:%S",time.localtime(startt)),'--',time.strftime("%H:%M:%S",time.localtime(endt))
                            if(nowt>endt):
                                sock.send(BITSSTART)
                                starbitt = None
                                endbitt = None    
                         #��ϷPK��ʼ����1 ��  ��ע���� ����1��                   
                    elif(self.getPlayCount(data,'4')>=1 and self.getPlayCount(data,1,8,True)>=1):
                        if(echo_debug>2):
                            print '��ҪPK���С�����'
                        nowt = time.time()
                        if(endbitt==None):
                            if(starbitt==None):
                                starbitt = nowt
                                endbitt = starbitt+bittime*60
                        else:
                             if(echo_debug>2):
                                 print 'PK����,�н���',time.strftime("%H:%M:%S",time.localtime(nowt)),'--',time.strftime("%H:%M:%S",time.localtime(starbitt)),'--',time.strftime("%H:%M:%S",time.localtime(endbitt))
                             if(nowt>endbitt):
                                sock.send(BITSEND)                                
                                startt = None
                                endt = None                                
                    elif(gameendflag==True and self.getPlayCount(data, '4',6)==0):
                         if(echo_debug>2):
                             print '������Ϸ������'
                         #time.sleep(3)
                         sock.send(GAMEOVER)                        
                         
        sock.close()
         
    def getPlayInfo(self,data,num=0,state=7):
        ''' ��ȡ�����Ϣ '''
        players = ''
        player  = ''
        restate = None
        if(data!='\r\n'):
            
            ndata = data.split("\r\n")
            countdata = len(ndata)-2
            players = ndata[countdata].split('@P@')
            #print 'players:',players
            player  = players[num].split('^')
            #print 'player:',player
            if(player!=''):
                restate = player[state]
                print '��Ϸ״̬��',player[state]
            else:
                restate = None
                print 'û�п�ʼ��Ϸ��'
    
        return restate
    def getPlayCount(self,data,state=None,num=7,more=None):
        ''' ��ȡ���״̬�¸��� '''
        players = ''
        player  = ''
        count   = 0
        recount = 0
        if(data!='\r\n'):
            
            ndata = data.split("\r\n")
            countdata = len(ndata)-2
            
            players = ndata[countdata].split('@P@')
            count = len(players) -1  
            if(state!=None):                
                for p in range(0,count):
                    player = players[p].split('^')
                    if(more!=None):
                        boints = string.atoi(player[num])
                        if(boints>=state):
                            recount = recount + 1
                    else:
                        if(player[num]==state):
                            recount = recount + 1              
                return recount
                
            else:
                return count
    
############### �������� ###############

class PubProtocol(basic.LineReceiver):
    def __init__(self, factory):         
        self.factory = factory

    def connectionMade(self):
        global number_of_connections        
        self.factory.clients.add(self)   

    def connectionLost(self,reason):
        ClientloseConnection(self,reason)               
        
    def lineReceived(self, line):
        if (DEBUGERR==True):
                print "=%s=" % line
           
        
        #for c in self.factory.clients:
        #    c.sendLine(line)
            
        loopMessage(self,line)


class PubFactory(protocol.Factory):
    def __init__(self):
        print 'start control...'
        controlThread.start()
        print 'started control ...'
        self.clients = set()

    def buildProtocol(self, addr):
        return PubProtocol(self)

controlThread = MyThread()
reactor.listenTCP(CONNECT_PORT, PubFactory())
reactor.run()

