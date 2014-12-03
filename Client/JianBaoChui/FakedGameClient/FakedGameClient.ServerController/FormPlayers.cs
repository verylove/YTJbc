using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FakedGameClient.ServerController
{
    public partial class FormPlayers : Form
    {
        private List<GamePlayer> _playerList = new List<GamePlayer>();
        public List<GamePlayer> PlayerList
        {
            get { return _playerList; }
            set { _playerList = value; }
        }

        private DataTable _playerTable;

        private Thread _thread;
        private bool _flag = true;
        private const int _refreshInterval = 5000;     //玩家列表刷新间隔,毫秒

        #region 私有方法
        private void InitPlayerTable()
        {
            if (_playerTable == null)
            {
                _playerTable = new DataTable();
            }
            else
            {
                _playerTable.Columns.Clear();
            }
            _playerTable.Columns.Add("玩家ID", typeof(string));
            _playerTable.Columns.Add("姓名", typeof(string));
            _playerTable.Columns.Add("玩家状态", typeof(string));
            _playerTable.Columns.Add("出拳手势", typeof(string));
            _playerTable.Columns.Add("本轮结果", typeof(string));
            _playerTable.Columns.Add("是否擂主", typeof(string));
            _playerTable.Columns.Add("游戏阶段", typeof(string));
            _playerTable.Columns.Add("游戏状态", typeof(string));
            _playerTable.Columns.Add("下注点数", typeof(string));
            _playerTable.Columns.Add("对手ID", typeof(string));
            _playerTable.Columns.Add("等级", typeof(string));
            _playerTable.Columns.Add("组号", typeof(string));
            _playerTable.Columns.Add("对手组号", typeof(string));
        }

        private void InitThread()
        {
            _thread = new Thread(new ThreadStart(UpdateUI));
        }

        private void ConvertToDatatable()
        {
            _playerTable.Clear();
            foreach (var player in _playerList)
            {
                DataRow dr = _playerTable.NewRow();
                dr["玩家ID"] = player.ID;
                dr["姓名"] = player.Name;
                dr["玩家状态"] = GameUtil.GetPlayerStateName(player.PlayerState);
                dr["出拳手势"] = GameUtil.GetGestureName(player.Gesture);
                dr["本轮结果"] = GameUtil.GetRoundResultName(player.RoundResult);
                dr["是否擂主"] = GameUtil.GetPlayerStatusName(player.PlayerStatus);
                dr["游戏阶段"] = GameUtil.GetGameStatusName(player.GameStatus);
                dr["游戏状态"] = GameUtil.GetGameStateName(player.GameState);
                dr["下注点数"] = player.BetPoint;
                dr["对手ID"] = player.RivalID;
                dr["等级"] = player.LocY;
                dr["组号"] = player.LocX;
                dr["对手组号"] = player.RivalLocX;

                _playerTable.Rows.Add(dr);
            }
            _playerTable.DefaultView.Sort = "玩家ID";
        }

        private void UpdateUI()
        {
            while (_flag)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    if (chkAutoRefresh.Checked)
                    {
                        ConvertToDatatable();

                        if (!cmbPlayer.Focused)
                        {
                            InitCmbPlayer();
                        }
                        InitDataGridView();
                    }
                    InitGameStatePanel();
                }));
                Thread.Sleep(_refreshInterval);
            }
        }

        private void InitDataGridView()
        {
            dgvPlayer.DataSource = _playerTable;
        }

        private void InitCmbPlayer()
        {
            cmbPlayer.DataSource = _playerTable.DefaultView.ToTable();
            cmbPlayer.DisplayMember = "玩家ID";
            cmbPlayer.ValueMember = "玩家ID";
            cmbPlayer.AutoCompleteMode = AutoCompleteMode.Append;

            //var acsc = new AutoCompleteSource(); 
            //foreach (DataRow row in _playerTable.Rows)
            //{

            //    acsc.Add(row["玩家ID"].ToString());
            //}
            cmbPlayer.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void InitGameStatePanel()
        {
            lblPlayerCount.Text = _playerList.Where(x => x.PlayerState >= PlayerState.Watching).Count().ToString();
            lblWatcherCount.Text = _playerList.Where(x => x.PlayerState == PlayerState.Watching).Count().ToString();

            if (_playerList.Count > 0)
            {
                GamePlayer champion = _playerList.Where(x => x.PlayerStatus == PlayerStatus.Winner).FirstOrDefault();
                if (champion != null)
                {
                    lblChampion.Text = champion.ID;
                }
                else
                {
                    lblChampion.Text = "未决";
                }

                GamePlayer player = _playerList.Where(x => x.PlayerState > PlayerState.Connected || x.PlayerState == PlayerState.Watching).FirstOrDefault();
                if (player != null)
                {
                    lblGameState.Text = GameUtil.GetGameStateName(player.GameState);
                }
                else
                {
                    lblGameState.Text = "未知";
                }
            }
        }

        #endregion

        public FormPlayers()
        {
            InitializeComponent();
        }

        private void FormPlayers_Load(object sender, EventArgs e)
        {
        }

        private void FormPlayers_Shown(object sender, EventArgs e)
        {
            InitPlayerTable();
            InitDataGridView();
            InitCmbPlayer();
            InitGameStatePanel();
            InitThread();
            //当显示该窗体的时候触发更新玩家列表线程
            _flag = true;
            _thread.Start();
        }

        private void btnLocate_Click(object sender, EventArgs e)
        {
            if (cmbPlayer.SelectedValue == null)
            {
                MessageBox.Show(this, "没有找到该玩家ID!", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var playerId = cmbPlayer.SelectedValue.ToString();
            foreach (DataGridViewRow row in dgvPlayer.Rows)
            {
                if (playerId == row.Cells["玩家ID"].Value.ToString())
                {
                    row.Selected = true;
                    dgvPlayer.FirstDisplayedScrollingRowIndex = row.Index;
                }
            }
        }

        private void FormPlayers_FormClosing(object sender, FormClosingEventArgs e)
        {
            _flag = false;
            _thread.Abort();
            e.Cancel = true;
            this.Hide();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ConvertToDatatable();
            InitCmbPlayer();
            InitDataGridView();
        }
    }
}
