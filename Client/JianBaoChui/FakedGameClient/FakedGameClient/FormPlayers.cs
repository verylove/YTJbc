using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FakedGameClient
{
    public partial class FormPlayers : Form
    {
        List<GamePlayer> _playerList = new List<GamePlayer>();
        public FormPlayers(List<GamePlayer> playerList)
        {
            _playerList = playerList;
            InitializeComponent();
        }

        private void FormPlayers_Load(object sender, EventArgs e)
        {
            foreach (var player in _playerList.OrderBy(x => x.ID)) 
            {
                PlayerInfoControl playerCtrl = new PlayerInfoControl()
                {
                    ID = player.ID,
                    PlayerName = player.Name,
                    Gesture = GameUtil.GetGestureName(player.Gesture),
                    RoundResult = GameUtil.GetRoundResultName(player.RoundResult),
                    PlayerStatus = GameUtil.GetPlayerStatusName(player.PlayerStatus),
                    PlayerState = GameUtil.GetPlayerStateName(player.PlayerState),
                    GameStatus = GameUtil.GetGameStatusName(player.GameStatus),
                    GameState = GameUtil.GetGameStateName(player.GameState),
                    BetPoint = player.BetPoint.ToString(),
                    RivalID = player.RivalID,
                    Level = player.LocY.ToString(),
                    LvNo = player.LocX.ToString(),
                    RivalLvNo = player.RivalLocX.ToString()
                };

                flowPanel.Controls.Add(playerCtrl);
            }
        }
    }
}
