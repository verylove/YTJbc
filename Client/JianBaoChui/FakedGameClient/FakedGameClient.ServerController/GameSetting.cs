using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FakedGameClient.ServerController
{
    public class GameSetting
    {
        /// <summary>
        /// 最大房间数两
        /// </summary>
        public int MaxRoomCount { get; set; }

        /// <summary>
        /// 最大玩家数量
        /// </summary>
        public int MaxPlayerCount { get; set; }

        /// <summary>
        /// 游戏开始时间
        /// </summary>
        public DateTime GameBeginTime { get; set; }

        /// <summary>
        /// 游戏结束时间
        /// </summary>
        public DateTime GameEndTime { get; set; }

        /// <summary>
        /// 中场休息时间(分钟)
        /// </summary>
        public int HalfTime { get; set; }

        /// <summary>
        /// 下注时间
        /// </summary>
        public int BetTime { get; set; }

        /// <summary>
        /// 下次下注等待时间
        /// </summary>
        public int NextBetTime { get; set; }

        /// <summary>
        /// 大奖图片
        /// </summary>
        public Bitmap AwardsImage { get; set; }
    }
}
