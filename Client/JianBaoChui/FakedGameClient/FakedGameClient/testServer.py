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

############### 常量定义 ###############

global  ALL_ROOM_INFO #全部房间信息
global  ALL_ROOM_APPLY #全部报名用户
global  SEPARATOR   #信息分隔符
global  CONNECT_PORT #连接端口号 
global  AUTHCODE #授权码
global  ALL_TEAM  #分组情况
global  DEBUGERR   #是否输出调式信息
ALL_TEAM = {} #TeamNode 类型 ，分组情况 示例：ALL_TEAM['B'][0].pLevel， PlayerInfo类型：Player = ALL_TEAM['B'][0].data

LEVEL={} #游戏局数定义 ,name:组识别名,count,组最大客户数
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

global  number_of_connections  #当前连接数量
global  max_connections           #最大连接数量
############### 初始化 ###############

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


############### 公共函数 ###############
def LogWrite(self,str,line=''):
        ''' 记录日志  '''
        
        logstr = FormatlogMsg(self,str,line)
        logFile.write( logstr )
        if (DEBUGERR==True):
                log.msg( str )

def FormatlogMsg(self,replay='',line=''):
        ''' 格式化日志样式 '''
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
        '''房间登陆认证信息验证函数 '''
        info = logininfo.split("^")
        if(info[0]==AUTHCODE):
                return 1
        else:
                return 0

def GetRoomId_InAuth(logininfo):
        '''获取房间编号'''
        info = logininfo.split("^")
        roomid  = info[1]
        if(roomid!=""):         
                return roomid;
        else:
                return 0

def GetRoomType_InAuth(logininfo):
        ''' 获取指令'''
        info = logininfo.split("^")
        return info[3]
        
def GetRoomData_InAuth(logininfo):
        ''' 获取指令值 '''
        info = logininfo.split("^")
        return info[4]

def Checkinfo(self,line):
        ''' 验证通信数据可信度  '''
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
                Login(self,data,logininfo)#登陆
        elif(type=='0002'):
                Apply(self,data,logininfo)#报名
        elif(type=='0003'):
                Punches(self,data,logininfo)#出拳
        elif(type=='0004'):
                Bet(self,data,logininfo)#下注
        elif(type=='0005'):
                Team(self,data,logininfo)#分组
        elif(type=='0000'):
                Exit(self,data,logininfo)#退出
        else:
                Login(self,data,logininfo)#默认为登陆
                
def ClientloseConnection(self,rid=''):
        ''' 断开于客户断连接 '''
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
    ''' 查找相同父亲的另一个孩子  '''
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
    ''' 计算p1 p2 谁胜出，1剪刀，2包，3锤，返回1:g1胜，2:g2胜,0:平  '''
    result = g1 - g2;
    if(result==-1 or result==2):
        return 1
    elif(result==0):
        return 0
    else:
        return 2
    
    
############### 功能函数 ###############

def Login(self,data,line):
        ''' 客户端登陆 '''
        global ALL_ROOM_INFO
        Checkinfo(self,line)    
        AddRoom(self,line)

def AddRoom(self,logininfo):
        '''  增加新的房间到在线列表 ''' 
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
        ''' 客户端报名参赛 ''' 
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
        
        #E={} #模拟客户端
        #for e in range(1,17):
        #    E[e]=e
        
        #LogWrite(self,'ALL_ROOM_APPLY :',ALL_ROOM_APPLY)
        
        E = ALL_ROOM_APPLY
        
        length = len(E)  #客户端总数
        LogWrite(self,'Client Count is %d' % length)
        gameround = 1 #初始游戏局数
        for i in LEVEL:
            if length > LEVEL[i]['count']: 
                gameround=i+1 #确定局数      
          
        LogWrite(self,'need %d round ' % gameround) #输出局数 
        
        #游戏分组数据二叉数初始化
        for t in range(gameround,-1,-1):
            tmp = {}
            for j in range(0,LEVEL[t]['count']):
                key = '%s%d' % (LEVEL[t]['name'],j)
                name = '%s' % (key)
                tmp[j]=TeamNode(name)  
              
            ALL_TEAM[LEVEL[t]['name']]= tmp
        
        #游戏分组数据二叉数初始化关联关系
        for t in range(gameround,-1,-1):
            tmp = {}
            i = 0 
            for j in range(0,LEVEL[t]['count']): 
               
                childLevel = LEVEL[t+1]['name'] #子的级别名称
                
                #默认没有子节点
                leftchild  = None
                rightchild = None
        
                if(t != gameround):#如果不是最底部 
                    leftchild  = ALL_TEAM[childLevel][i] #0 2 4 6
                    i=i+1
                    rightchild = ALL_TEAM[childLevel][i] #1 3 5 7
                i=i+1               
        
                #默认初始化为顶级
                parentLevel = '-1'
                parentNo   = '-1'
                
                if(t-1>=0):#如果不是最顶部的
                    parentLevel = LEVEL[t-1]['name'] #父的级别名称
                    parentNo   = j//2 
                
                Level  = LEVEL[t]['name'] #当前级别名字
                
                ALL_TEAM[Level][j].lchild = leftchild
                ALL_TEAM[Level][j].rchild = rightchild
                ALL_TEAM[Level][j].pLevel = parentLevel
                ALL_TEAM[Level][j].pNo  =  parentNo
       
        #print sorted(ALL_TEAM) #测试显示分组初始化情况
        #print ALL_TEAM #测试显示分组初始化数据
        i=0
        for e in E.keys():
            Level = LEVEL[gameround]['name']
            ALL_TEAM[Level][i].data = E[e] #初始化PLAYER
            i=i+1

        #print ALL_TEAM 
        tree=BinaryTree(ALL_TEAM['A'][0])

        #测试输出全部情况
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
                #    print note.name,'上一级:',note.pLevel,note.pNo,'左孩子:',note.lchild.name,'右孩子:',note.rchild.name
                #else:
                    #pass
                #    print note.name,'上一级:',note.pLevel,note.pNo
        #i=0
        #for e in E.keys():
        #    Level = LEVEL[gameround]['name']
        #    print ALL_TEAM[Level][i].name,ALL_TEAM[Level][i].data.id
        #    i=i+1      
            
        SendRoomInfoMessage(self)           
        
def Punches(self,data,logininfo):
        ''' 客户端出拳动作,会计算输赢结果 '''
        global ALL_ROOM_APPLY   
        Checkinfo(self,logininfo)       
        flag = 0
        info = logininfo.split("^")
        roomid  = info[1]
        roomnick = info[2]      
        for room in ALL_ROOM_APPLY.keys(): #如果是参赛客户端
                if(room==roomid):
                        flag = 1
        if flag == 1:
                #当前客户端
                player = ALL_ROOM_INFO[roomid]
                player.guess = data
                #对手客户端
                rival  = ALL_TEAM[LEVEL[player.level]['name']][player.rivallevelno]
                rivalplayer = rival.data;
                
                if(rivalplayer.guess!=''):
                   result =  Guess(player.guess,rivalplayer.guess)
                    
                ALL_ROOM_INFO[roomid] = player
                ALL_ROOM_APPLY[roomid]=ALL_ROOM_INFO[roomid]
        SendRoomInfoMessage(self)        

def Exit(self,data,logininfo):
        ''' 客户端退出 '''
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
        ''' 处理消息循环  '''         
        global ALL_ROOM_INFO    
        type = GetRoomType_InAuth(line)
        data =  GetRoomData_InAuth(line)
        switch(self,type,data,line)
                                
def SendRoomInfoMessage(self):
        ''' 发布全部客户端端信息到指定客户端 '''        
        global ALL_ROOM_INFO
        data = ''
        for r in ALL_ROOM_INFO.keys():          
                 player = ALL_ROOM_INFO[r]
                 data+=player.getCommand()

        for c in self.factory.clients:
            c.sendLine(data)
            
        #print self.transport.getHost() 
############### 线程函数 ###############


            
    
        
    


  
############### 程序主体 ###############

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

