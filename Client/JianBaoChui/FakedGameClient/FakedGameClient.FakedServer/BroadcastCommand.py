#-*- coding: gbk -*-
class PlayerInfo:
    '''
            ������ݽṹ
    '''
    def __init__(self,id,nickname,guess='',win='',ischampion=0,stage=1,state=1,gstate='',betpoints=0,rivalid='',rivallevelno='',level='',levelno='',eof='@P@'):
        self.id = id                    #ID
        self.nickname = nickname        #�ǳ�
        
        self.guess = guess              #��ȭ��ʶ�� 1������2������3����
        self.win = win                  #��Ӯ��ʶ�� 1��Ӯ��2���䣻3��ƽ
        self.ischampion = ischampion    #�Ƿ������� 1���ǣ�0����
        
        self.stage = stage              #�����׶α�ʶ:1����̭����2��8ǿ����3��4ǿ����4��������5����ս��
        self.state = state              #���״̬��ʶ:1����ս��2���ѵ�½��3���ѱ�����4���غϿ�ʼ��5���غϽ�����
        self.gstate = gstate            #��Ϸ״̬��־:1,KO��ʼ;2,KO����;3,KO����;4:PK��ʼ;5,PK����;6,PK����
        self.betpoints = betpoints      #��ս����ע����
        
        self.rivalid=rivalid            #����ID
        self.rivallevelno=rivallevelno  #�������
        
        self.level=level                #��ҵȼ�:0,�ֻ���Ӯ��;1������;2,4ǿ��;3,8ǿ�飬4,16���飻5,32���飻6��64���飻7,128���飻8,256���飻9,512���飻10��1024��
        self.levelno=levelno            #������
        self.eof = eof                  #������,����ͳ������
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
        ��̭�����ݰ�
        ��ʽ: ����PlayerInfo^�����׶�^������
    '''
    def __init__(self,gamestage,players=[],eof='@KO@'):
        self.gamestage = gamestage  #�����׶α�ʶ��1����̭����2��8ǿ����3��4ǿ����4��������5��������ͣ��6��������ֹ
        self.players = players
        self.eof = eof              #������

    def getCmdData(self,seperator='^'):
        data = ''
        for player in self.players:
            data+=player.getCommand(seperator)
        data +=self.eof
        return data

class BetTopNCommand:
    '''
        ��ע����TOP N
        ��ʽ:������ע�˵��ǳ�^��ע����^������
        1.������:@BET@     �ͻ��˸��ݽ������ж��Ƿ���ע�����㲥        
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
        ��ս�����ݰ�
        ��ʽ:����PlayerInfo^��ս��PlayerInfo^������
    '''
    def __init__(self,p1,p2,eof='@CH@'):
        self.p1 = p1
        self.p2 = p2
        self.eof = eof
    def getCmdData(self,seperator='^'):
        data = self.p1.getCommand(seperator) + self.p2.getCommand(seperator) + self.eof
        return data 
