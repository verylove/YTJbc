using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FakedGameClient
{
    public partial class PlayerInfoControl : UserControl
    {
        public string ID { get { return lblId.Text; } set { lblId.Text = value; } }
        public string Name { get { return lblName.Text; } set { lblName.Text = value; } }
        public string Gesture { get { return lblGesture.Text; } set { lblGesture.Text = value; } }
        public string RoundResult { get { return lblRoundResult.Text; } set { lblRoundResult.Text = value; } }
        public string PlayerStatus { get { return lblPlayerStatus.Text; } set { lblPlayerStatus.Text = value; } }
        public string GameStatus { get { return lblGameStage.Text; } set { lblGameStage.Text = value; } }
        public string PlayerState { get { return lblPlayerState.Text; } set { lblPlayerState.Text = value; } }
        public string GameState { get { return lblGameState.Text; } set { lblGameState.Text = value; } }
        public string BetPoint { get { return lblBetPoint.Text; } set { lblBetPoint.Text = value; } }
        public string RivalID { get { return lblRivalId.Text; } set { lblRivalId.Text = value; } }
        public string Level { get { return lblLevel.Text; } set { lblLevel.Text = value; } }
        public string LvNo { get { return lblLvNo.Text; } set { lblLvNo.Text = value; } }
        public string RivalLvNo { get { return lblRivalLvNo.Text; } set { lblRivalLvNo.Text = value; } }
        public string CurPoint { get { return lblCurPoint.Text; } set { lblCurPoint.Text = value; } }
        public PlayerInfoControl()
        {
            InitializeComponent();
        }
    }
}
