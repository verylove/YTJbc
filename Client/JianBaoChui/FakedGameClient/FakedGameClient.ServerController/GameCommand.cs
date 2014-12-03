using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FakedGameClient.ServerController
{
    public class GameCommand
    {
        public static string AuthCode = "yang";
        public static string ID = "GM";
        public static string StartGame
        {
            get
            {
                return string.Format("{0}^{1}^{2}^0010^0", GameCommand.AuthCode, GameCommand.ID, GameCommand.ID);
            }
        }

        public static string StopGame
        {
            get
            {
                return string.Format("{0}^{1}^{2}^0011^0", GameCommand.AuthCode, GameCommand.ID, GameCommand.ID);
            }
        }

        public static string ResetGame
        {
            get
            {
                return string.Format("{0}^{1}^{2}^0012^1", GameCommand.AuthCode, GameCommand.ID, GameCommand.ID);
            }
        }

        public static string StartBet
        {
            get
            {
                return string.Format("{0}^{1}^{2}^0013^0", GameCommand.AuthCode, GameCommand.ID, GameCommand.ID);
            }
        }

        public static string StopBet
        {
            get
            {
                return string.Format("{0}^{1}^{2}^0013^1", GameCommand.AuthCode, GameCommand.ID, GameCommand.ID);
            }
        }

        public static string StartKO = "";
        public static string StopKO = "";
        public static string StartPK = "";
        public static string StopPK = "";

        public static string SystemNotice
        {
            get
            {
                return string.Format("{0}^{1}^{2}^0014^{3}", GameCommand.AuthCode, GameCommand.ID, GameCommand.ID,"{0}");
            }
        }

        public static string GameMessage
        {
            get
            {
                return string.Format("{0}^{1}^{2}^0015^{3}", GameCommand.AuthCode, GameCommand.ID, GameCommand.ID, "{0}");
            }
        }

        public static string UpdateSetting
        {
            get
            {
                return string.Format("{0}^{1}^{2}^0016^0", GameCommand.AuthCode, GameCommand.ID, GameCommand.ID);
            }
        }
    }
}
