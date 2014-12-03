
namespace FingerGuessingGameAndroid.MainUI
{
    /// <summary>
    /// 玩家状态
    /// </summary>
    public enum PlayerState
    {
        Unknown = 0,
        Watching = 1,               //观战
        Connected = 2,              //已连接
        Signed = 3,                 //已报名
        //Betted = 3,                 //已下注
        RoundStart = 4,             //回合开始
        RoundEnd = 5,                //回合结束
        Betted                      //已下注
    }

    /// <summary>
    /// 游戏阶段状态
    /// </summary>
    public enum GameState
    {
        Unknown = 0,
        KOStart = 1,                //KO赛开始
        KO = 2,                     //KO赛中
        KOEnd = 3,                  //KO赛终
        PKStart = 4,                //PK赛开始,也叫下注开始
        PK = 5,                     //PK赛中
        PKEnd = 6                   //PK赛终
    }

    public enum GuessGesture
    {
        Unknown = 0,
        Jian = 1,
        Bao = 2,
        Chui = 3
    }

    /// <summary>
    /// 比赛阶段
    /// </summary>
    public enum GameStatus
    {
        Unknown = 0,
        Qualifier = 1,      //预选赛
        Final8 = 2,         //8强
        Final4 = 3,         //4强
        Final = 4,         //决赛
        PK = 5              //PK赛
    }

    /// <summary>
    /// 玩家身份
    /// </summary>
    public enum PlayerStatus
    {
        //Unknown = 0,
        //Audience = 1,   //观众
        //Competitor = 2, //参赛者
        Winner = 1,     //擂主
        Defier = 0      //挑战者
    }

    public enum RoundResult
    {
        Unknown = 0,
        Win = 1,
        Lose = 2,
        Draw = 3
    }
}
