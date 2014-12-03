using System;
namespace FakedGameClient
{
    public class GameUtil
    {
        public static string GetGestureName(GuessGesture g)
        {
            string gesture = string.Empty;
            switch (g)
            {
                case GuessGesture.Bao:
                    gesture = "包子";
                    break;
                case GuessGesture.Jian:
                    gesture = "剪子";
                    break;
                case GuessGesture.Chui:
                    gesture = "锤子";
                    break;
                default:
                    gesture = "未知";
                    break;
            }
            return gesture;
        }

        //1:g1胜 2:g2胜 0:平
        public static int Guess(GuessGesture g1, GuessGesture g2)
        {
            int result = g1 - g2;
            if (-1 == result || 2 == result)
            {
                return 1;
            }
            else if (0 == result)
            {
                return 0;
            }
            else
            {
                return 2;
            }
        }

        public static string GetGameStatusName(GameStatus s)
        {
            string status = string.Empty;
            switch (s)
            {
                case GameStatus.Final:
                    status = "决赛";
                    break;
                case GameStatus.Final4:
                    status = "4强赛";
                    break;
                case GameStatus.Final8:
                    status = "8强赛";
                    break;
                case GameStatus.PK:
                    status = "PK赛";
                    break;
                case GameStatus.Qualifier:
                    status = "预选赛";
                    break;
                case GameStatus.Ex:
                    status = "附加赛";
                    break;
                default:
                    status = ((int)s).ToString();
                    break;
            }
            return status;
        }

        public static string GetGameStateName(GameState gs)
        {
            string state = string.Empty;
            switch (gs)
            {
                case GameState.KOStart:
                    state = "KO开赛"; break;
                case GameState.KO:
                    state = "KO擂主"; break;
                case GameState.KOEnd:
                    state = "KO赛终"; break;
                case GameState.PKStart:
                    state = "PK开赛"; break;
                case GameState.PK:
                    state = "PK赛中"; break;
                case GameState.PKEnd:
                    state = "PK赛终"; break;
                default:
                    state = "未知"; break;
            }
            return state;
        }

        public static string GetPlayerStateName(PlayerState ps)
        {
            string state = string.Empty;
            switch (ps)
            {
                case PlayerState.Connected:
                    state = "已连接"; break;
                case PlayerState.Signed:
                    state = "已报名"; break;
                case PlayerState.Watching:
                    state = "观战"; break;
                case PlayerState.RoundStart:
                    state = "回合开始"; break;
                case PlayerState.RoundEnd:
                    state = "回合结束"; break;
                //case PlayerState.Betted:
                //    state = "已下注"; break;
                default:
                    state = "未知"; break;
            }
            return state;
        }

        public static string GetRoundResultName(RoundResult rr)
        {
            string result = string.Empty;
            switch (rr)
            {
                case RoundResult.Win:
                    result = "获胜";
                    break;
                case RoundResult.Lose:
                    result = "失利";
                    break;
                case RoundResult.Draw:
                    result = "不分胜负";
                    break;
                default:
                    result = "未知";
                    break;
            }
            return result;
        }

        public static string GetPlayerStatusName(PlayerStatus ps)
        {
            string result = string.Empty;
            switch (ps)
            {
                case PlayerStatus.Defier:
                    result = "未知";
                    break;
                case PlayerStatus.Winner:
                    result = "擂主";
                    break;
                default:
                    result = "未知";
                    break;
            }
            return result;
        }

        //获得[1~max]范围内随机数,包含边界
        public static int Random(int max)
        {
            return DateTime.Now.TimeOfDay.Milliseconds % max + 1;
        }
    }
}