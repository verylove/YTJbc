using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FakedGameClient
{
    public class GamePlayer
    {
        public string ID { get; set; }

        //名字
        public string Name { get; set; }

        //本人出拳手势    剪/包/锤
        public GuessGesture Gesture { get; set; }

        //本轮比赛结果
        public RoundResult RoundResult { get; set; }

        //本人身份    观众/参赛者/擂主/挑战者...
        public PlayerStatus PlayerStatus { get; set; }

        //当前游戏阶段    海选/四强/PK赛...
        public GameStatus GameStatus { get; set; }

        //当前玩家状态    报名/等待/赛中/观战...
        public PlayerState PlayerState { get; set; }

        //当前游戏状态    淘汰赛开始/结束/PK赛开始/结束...
        public GameState GameState { get; set; }

        //下注点数
        public int BetPoint { get; set; }

        //对手
        public string RivalID { get; set; }

        //对手
        public GamePlayer Rival { get; set; }

        //对手的x坐标
        public int RivalLocX { get; set; }

        //当前y坐标(Level)
        public int LocY { get; set; }

        //本人的x坐标
        public int LocX { get; set; }

        //当前积分
        public int CurPoint { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class GamePlayerComparer : IEqualityComparer<GamePlayer>
    {
        public bool Equals(GamePlayer x, GamePlayer y)
        {
            return x.RivalID.Equals(y.ID);
        }

        public int GetHashCode(GamePlayer obj)
        {
            return 0;
        }
    }
}
