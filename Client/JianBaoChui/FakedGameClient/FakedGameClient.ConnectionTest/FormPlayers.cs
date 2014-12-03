using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace FakedGameClient.ConnectionTest
{
    public partial class FormPlayers : Form
    {
        List<GamePlayer> _playerList = new List<GamePlayer>();
        Thread _updateThread = null;

        public List<GamePlayer> PlayerList
        {
            get { return _playerList; }
            set { _playerList = value; }
        }
        public FormPlayers()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void UpdatePlayers()
        {
            //while (true)
            //{
            flowPanel.Controls.Clear();

            foreach (var player in _playerList.OrderBy(x => x.ID))
            {
                PlayerInfoControl playerCtrl = new PlayerInfoControl()
                {
                    ID = player.ID,
                    Name = player.Name,
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
                    RivalLvNo = player.RivalLocX.ToString(),
                    CurPoint=player.CurPoint.ToString()
                };

                flowPanel.Controls.Add(playerCtrl);
                //flowPanel.Refresh();
                //}
                //Thread.Sleep(2000);
            }
        }

        private void FormPlayers_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void FormPlayers_Shown(object sender, EventArgs e)
        {
            UpdatePlayers();
        }

        private void FormPlayers_Load(object sender, EventArgs e)
        {
            //_updateThread = new Thread(new ThreadStart(UpdatePlayers));
            //_updateThread.Start();
        }
    }
}
