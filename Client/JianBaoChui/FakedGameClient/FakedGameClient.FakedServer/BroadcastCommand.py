#-*- coding: gbk -*-
class PlayerInfo:
    '''
            玩家数据结构
    '''
    def __init__(self,id,nickname,guess='',win='',ischampion=0,stage=1,state=1,gstate='',betpoints=0,rivalid='',rivallevelno='',level='',levelno='',eof='@P@'):
        self.id = id                    #ID
        self.nickname = nickname        #昵称
        
        self.guess = guess              #出拳标识： 1，剪；2，包；3，锤
        self.win = win                  #输赢标识： 1，赢；2，输；3，平
        self.ischampion = ischampion    #是否擂主： 1，是；0，否
        
        self.stage = stage              #比赛阶段标识:1，淘汰赛；2，8强赛；3，4强赛；4，决赛；5，挑战赛
        self.state = state              #玩家状态标识:1，观战；2，已登陆；3，已报名；4，回合开始；5，回合结束；
        self.gstate = gstate            #游戏状态标志:1,KO开始;2,KO进行;3,KO结束;4:PK开始;5,PK进行;6,PK结束
        self.betpoints = betpoints      #挑战赛下注点数
        
        self.rivalid=rivalid            #对手ID
        self.rivallevelno=rivallevelno  #对手序号
        
        self.level=level                #玩家等级:0,轮滑赛赢家;1决赛组;2,4强组;3,8强组，4,16人组；5,32人组；6，64人组；7,128人组；8,256人组；9,512人组；10，1024组
        self.levelno=levelno            #玩家序号
        self.eof = eof                  #结束符,用于统计人数
    def getCommand(self,seperator='^'):
        data = str(self.id) + seperator\
            + str(self.nickname) + seperator\
            + str(self.guess) + seperator\
            + str(self.win) + seperator\
            + str(self.ischampion) + seperator\
            + str(self.stage) + seperator\
            + str(self.state) + seperator\
            + str(self.gstate) + seperator\
            + str(self.betpoints) + seperator\
            + str(self.rivalid) + seperator\
            + str(self.rivallevelno) + seperator\
            + str(self.level) + seperator\
            + str(self.levelno) + seperator\
            + str(self.eof)
        return data

class KOGameCommand:
    '''
        淘汰赛数据包
        格式: 所有PlayerInfo^比赛阶段^结束符
    '''
    def __init__(self,gamestage,players=[],eof='@KO@'):
        self.gamestage = gamestage  #比赛阶段标识：1，淘汰赛；2，8强赛；3，4强赛；4，决赛；5，赛事暂停；6，赛事终止
        self.players = players
        self.eof = eof              #结束符

    def getCmdData(self,seperator='^'):
        data = ''
        for player in self.players:
            data+=player.getCommand(seperator)
        data +=self.eof
        return data

class BetTopNCommand:
    '''
        下注排名TOP N
        格式:所有下注人的昵称^下注点数^结束符
        1.结束符:@BET@     客户端根据结束符判断是否下注排名广播        
    '''
    def __init__(self,players=[],eof='@BET@'):
        self.players = players
        self.eof = eof
        
    def getCmdData(self,seperator='^'):
        data = ''
        for player in self.players:
            data+=str(player.nickname) + str(seperator) + str(player.betpoints) + str(seperator) + self.eof
        return data

class ChallengeCommand:
    '''
        挑战赛数据包
        格式:擂主PlayerInfo^挑战者PlayerInfo^结束符
    '''
    def __init__(self,p1,p2,eof='@CH@'):
        self.p1 = p1
        self.p2 = p2
        self.eof = eof
    def getCmdData(self,seperator='^'):
        data = self.p1.getCommand(seperator) + self.p2.getCommand(seperator) + self.eof
        return data 
