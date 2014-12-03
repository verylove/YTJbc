using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FakedGameClient.CmdGenerator
{
    public partial class FormMain : Form
    {
        string EOF = "\r\n";
        public FormMain()
        {
            InitializeComponent();
        }
        #region 窗体事件

        private void FormMain_Load(object sender, EventArgs e)
        {
            cmbGameState.SelectedIndex = 0;
            cmbGameStatus.SelectedIndex = 0;
            cmbPlayerGesture.SelectedIndex = 0;
            cmbPlayerState.SelectedIndex = 0;
            cmbPlayerStatus.SelectedIndex = 0;
            cmbRivalGesture.SelectedIndex = 0;
            cmbRoundResult.SelectedIndex = 0;

            cmbGameState2.SelectedIndex = 0;
            cmbGameStatus2.SelectedIndex = 0;
            cmbPlayerGesture2.SelectedIndex = 0;
            cmbPlayerState2.SelectedIndex = 0;
            cmbPlayerStatus2.SelectedIndex = 0;
            cmbRivalGesture2.SelectedIndex = 0;
            cmbRoundResult2.SelectedIndex = 0;
        }

        private void btnMake_Click(object sender, EventArgs e)
        {
            string cmd = MakePlayerCmdString() + MakePlayer2CmdString() + EOF;
            txtCmd.Text = cmd;
            if (chkSaveToFile.Checked)
            {
                File.WriteAllText(Properties.Settings.Default.CmdFilePath, cmd, Encoding.Default);
            }
        }

        #endregion 窗体事件

        #region 私有方法
        private string MakePlayerCmdString()
        {
            string cmdFormat = "{0}^{1}^{2}^{3}^{4}^{5}^{6}^{7}^{8}^{9}^{10}^{11}^{12}^{13}@P@";
            string cmd = string.Format(cmdFormat,
                txtEventCode.Text,
                txtPlayerId.Text,
                cmbGameState.SelectedIndex.ToString(),
                txtPlayerName.Text,
                cmbGameStatus.SelectedIndex.ToString(),
                txtLevel.Text.ToString(),
                txtLevelNo.Text.ToString(),
                cmbPlayerGesture.SelectedIndex.ToString(),
                cmbRoundResult.SelectedIndex.ToString(),
                txtRivalName.Text,
                cmbRivalGesture.SelectedIndex.ToString(),
                txtBetPoint.Text,
                cmbPlayerStatus.SelectedIndex.ToString(),
                cmbPlayerState.SelectedIndex.ToString()
                );
            return cmd;
        }

        private string MakePlayer2CmdString()
        {
            string cmdFormat = "{0}^{1}^{2}^{3}^{4}^{5}^{6}^{7}^{8}^{9}^{10}^{11}^{12}^{13}@P@";
            string cmd = string.Format(cmdFormat,
                txtEventCode2.Text,
                txtPlayerId2.Text,
                cmbGameState2.SelectedIndex.ToString(),
                txtPlayerName2.Text,
                cmbGameStatus2.SelectedIndex.ToString(),
                txtLevel2.Text.ToString(),
                txtLevelNo2.Text.ToString(),
                cmbPlayerGesture2.SelectedIndex.ToString(),
                cmbRoundResult2.SelectedIndex.ToString(),
                txtRivalName2.Text,
                cmbRivalGesture2.SelectedIndex.ToString(),
                txtBetPoint2.Text,
                cmbPlayerStatus2.SelectedIndex.ToString(),
                cmbPlayerState2.SelectedIndex.ToString()
                );
            return cmd;
        }

        #endregion  私有方法
    }
}
