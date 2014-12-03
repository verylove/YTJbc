namespace FakedGameClient.ConnectionTest
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnJian = new System.Windows.Forms.Button();
            this.btnBao = new System.Windows.Forms.Button();
            this.btnChui = new System.Windows.Forms.Button();
            this.btnSign = new System.Windows.Forms.Button();
            this.btnWatching = new System.Windows.Forms.Button();
            this.btnQuitGame = new System.Windows.Forms.Button();
            this.btnBet = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnClearClients = new System.Windows.Forms.Button();
            this.grpClientLst = new System.Windows.Forms.GroupBox();
            this.flowPnl = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.btnReverseCheck = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnGameStart = new System.Windows.Forms.Button();
            this.btnGameEnd = new System.Windows.Forms.Button();
            this.btnPlayerInfo = new System.Windows.Forms.Button();
            this.txtBetPoint = new System.Windows.Forms.TextBox();
            this.btnResetData = new System.Windows.Forms.Button();
            this.btnBetEnd = new System.Windows.Forms.Button();
            this.btnBetStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkAutoScroll = new System.Windows.Forms.CheckBox();
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.grpClientLst.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(495, 60);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "链接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnJian
            // 
            this.btnJian.Location = new System.Drawing.Point(3, 31);
            this.btnJian.Name = "btnJian";
            this.btnJian.Size = new System.Drawing.Size(75, 23);
            this.btnJian.TabIndex = 1;
            this.btnJian.Text = "剪";
            this.btnJian.UseVisualStyleBackColor = true;
            this.btnJian.Click += new System.EventHandler(this.btnJian_Click);
            // 
            // btnBao
            // 
            this.btnBao.Location = new System.Drawing.Point(84, 31);
            this.btnBao.Name = "btnBao";
            this.btnBao.Size = new System.Drawing.Size(75, 23);
            this.btnBao.TabIndex = 2;
            this.btnBao.Text = "包";
            this.btnBao.UseVisualStyleBackColor = true;
            this.btnBao.Click += new System.EventHandler(this.btnBao_Click);
            // 
            // btnChui
            // 
            this.btnChui.Location = new System.Drawing.Point(165, 31);
            this.btnChui.Name = "btnChui";
            this.btnChui.Size = new System.Drawing.Size(75, 23);
            this.btnChui.TabIndex = 3;
            this.btnChui.Text = "锤";
            this.btnChui.UseVisualStyleBackColor = true;
            this.btnChui.Click += new System.EventHandler(this.btnChui_Click);
            // 
            // btnSign
            // 
            this.btnSign.Location = new System.Drawing.Point(84, 2);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(75, 23);
            this.btnSign.TabIndex = 6;
            this.btnSign.Text = "报名";
            this.btnSign.UseVisualStyleBackColor = true;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // btnWatching
            // 
            this.btnWatching.Location = new System.Drawing.Point(165, 2);
            this.btnWatching.Name = "btnWatching";
            this.btnWatching.Size = new System.Drawing.Size(75, 23);
            this.btnWatching.TabIndex = 7;
            this.btnWatching.Text = "观战";
            this.btnWatching.UseVisualStyleBackColor = true;
            this.btnWatching.Click += new System.EventHandler(this.btnWatching_Click);
            // 
            // btnQuitGame
            // 
            this.btnQuitGame.Location = new System.Drawing.Point(165, 60);
            this.btnQuitGame.Name = "btnQuitGame";
            this.btnQuitGame.Size = new System.Drawing.Size(75, 23);
            this.btnQuitGame.TabIndex = 7;
            this.btnQuitGame.Text = "退出";
            this.btnQuitGame.UseVisualStyleBackColor = true;
            this.btnQuitGame.Click += new System.EventHandler(this.btnQuitGame_Click);
            // 
            // btnBet
            // 
            this.btnBet.Location = new System.Drawing.Point(84, 60);
            this.btnBet.Name = "btnBet";
            this.btnBet.Size = new System.Drawing.Size(75, 23);
            this.btnBet.TabIndex = 8;
            this.btnBet.Text = "下注";
            this.btnBet.UseVisualStyleBackColor = true;
            this.btnBet.Click += new System.EventHandler(this.btnBet_Click);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(3, 219);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(650, 278);
            this.txtLog.TabIndex = 10;
            this.txtLog.Text = "";
            this.txtLog.WordWrap = false;
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(525, 2);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(79, 23);
            this.btnNew.TabIndex = 11;
            this.btnNew.Text = "新增客户端";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnClearClients
            // 
            this.btnClearClients.Location = new System.Drawing.Point(610, 2);
            this.btnClearClients.Name = "btnClearClients";
            this.btnClearClients.Size = new System.Drawing.Size(41, 23);
            this.btnClearClients.TabIndex = 12;
            this.btnClearClients.Text = "删除";
            this.btnClearClients.UseVisualStyleBackColor = true;
            this.btnClearClients.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // grpClientLst
            // 
            this.grpClientLst.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpClientLst.Controls.Add(this.flowPnl);
            this.grpClientLst.Location = new System.Drawing.Point(3, 87);
            this.grpClientLst.Name = "grpClientLst";
            this.grpClientLst.Size = new System.Drawing.Size(650, 99);
            this.grpClientLst.TabIndex = 13;
            this.grpClientLst.TabStop = false;
            this.grpClientLst.Text = "客户端列表";
            // 
            // flowPnl
            // 
            this.flowPnl.AutoScroll = true;
            this.flowPnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPnl.Location = new System.Drawing.Point(3, 17);
            this.flowPnl.Name = "flowPnl";
            this.flowPnl.Size = new System.Drawing.Size(644, 79);
            this.flowPnl.TabIndex = 10;
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(260, 193);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(75, 23);
            this.btnClearLog.TabIndex = 14;
            this.btnClearLog.Text = "清空日志";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Location = new System.Drawing.Point(82, 192);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(76, 23);
            this.btnCheckAll.TabIndex = 15;
            this.btnCheckAll.Text = "全选";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // btnReverseCheck
            // 
            this.btnReverseCheck.Location = new System.Drawing.Point(163, 192);
            this.btnReverseCheck.Name = "btnReverseCheck";
            this.btnReverseCheck.Size = new System.Drawing.Size(75, 23);
            this.btnReverseCheck.TabIndex = 16;
            this.btnReverseCheck.Text = "反选";
            this.btnReverseCheck.UseVisualStyleBackColor = true;
            this.btnReverseCheck.Click += new System.EventHandler(this.btnReverseCheck_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(576, 58);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 17;
            this.btnDisconnect.Text = "断开";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(3, 2);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 18;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnGameStart
            // 
            this.btnGameStart.BackColor = System.Drawing.SystemColors.Info;
            this.btnGameStart.Location = new System.Drawing.Point(253, 2);
            this.btnGameStart.Name = "btnGameStart";
            this.btnGameStart.Size = new System.Drawing.Size(75, 23);
            this.btnGameStart.TabIndex = 19;
            this.btnGameStart.Text = "比赛开始";
            this.btnGameStart.UseVisualStyleBackColor = false;
            this.btnGameStart.Click += new System.EventHandler(this.btnGameStart_Click);
            // 
            // btnGameEnd
            // 
            this.btnGameEnd.Location = new System.Drawing.Point(334, 2);
            this.btnGameEnd.Name = "btnGameEnd";
            this.btnGameEnd.Size = new System.Drawing.Size(75, 23);
            this.btnGameEnd.TabIndex = 19;
            this.btnGameEnd.Text = "比赛结束";
            this.btnGameEnd.UseVisualStyleBackColor = false;
            this.btnGameEnd.Click += new System.EventHandler(this.btnGameEnd_Click);
            // 
            // btnPlayerInfo
            // 
            this.btnPlayerInfo.BackColor = System.Drawing.Color.Navy;
            this.btnPlayerInfo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPlayerInfo.ForeColor = System.Drawing.Color.Lime;
            this.btnPlayerInfo.Location = new System.Drawing.Point(4, 192);
            this.btnPlayerInfo.Name = "btnPlayerInfo";
            this.btnPlayerInfo.Size = new System.Drawing.Size(75, 23);
            this.btnPlayerInfo.TabIndex = 20;
            this.btnPlayerInfo.Text = "玩家状况";
            this.btnPlayerInfo.UseVisualStyleBackColor = false;
            this.btnPlayerInfo.Click += new System.EventHandler(this.btnPlayerInfo_Click);
            // 
            // txtBetPoint
            // 
            this.txtBetPoint.Location = new System.Drawing.Point(3, 60);
            this.txtBetPoint.Name = "txtBetPoint";
            this.txtBetPoint.Size = new System.Drawing.Size(75, 21);
            this.txtBetPoint.TabIndex = 21;
            this.txtBetPoint.Text = "200";
            // 
            // btnResetData
            // 
            this.btnResetData.Location = new System.Drawing.Point(415, 2);
            this.btnResetData.Name = "btnResetData";
            this.btnResetData.Size = new System.Drawing.Size(64, 52);
            this.btnResetData.TabIndex = 22;
            this.btnResetData.Text = "重置状态";
            this.btnResetData.UseVisualStyleBackColor = true;
            this.btnResetData.Click += new System.EventHandler(this.btnResetData_Click);
            // 
            // btnBetEnd
            // 
            this.btnBetEnd.Location = new System.Drawing.Point(334, 31);
            this.btnBetEnd.Name = "btnBetEnd";
            this.btnBetEnd.Size = new System.Drawing.Size(75, 23);
            this.btnBetEnd.TabIndex = 23;
            this.btnBetEnd.Text = "下注结束";
            this.btnBetEnd.UseVisualStyleBackColor = true;
            this.btnBetEnd.Click += new System.EventHandler(this.btnBetEnd_Click);
            // 
            // btnBetStart
            // 
            this.btnBetStart.Location = new System.Drawing.Point(253, 31);
            this.btnBetStart.Name = "btnBetStart";
            this.btnBetStart.Size = new System.Drawing.Size(75, 23);
            this.btnBetStart.TabIndex = 24;
            this.btnBetStart.Text = "下注开始";
            this.btnBetStart.UseVisualStyleBackColor = true;
            this.btnBetStart.Click += new System.EventHandler(this.btnBetStart_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(246, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1, 82);
            this.label1.TabIndex = 25;
            this.label1.Text = "      ";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Location = new System.Drawing.Point(485, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1, 82);
            this.label2.TabIndex = 26;
            this.label2.Text = "      ";
            // 
            // chkAutoScroll
            // 
            this.chkAutoScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAutoScroll.AutoSize = true;
            this.chkAutoScroll.Checked = true;
            this.chkAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoScroll.Location = new System.Drawing.Point(581, 197);
            this.chkAutoScroll.Name = "chkAutoScroll";
            this.chkAutoScroll.Size = new System.Drawing.Size(72, 16);
            this.chkAutoScroll.TabIndex = 27;
            this.chkAutoScroll.Text = "自动滚屏";
            this.chkAutoScroll.UseVisualStyleBackColor = true;
            // 
            // skinEngine1
            // 
            this.skinEngine1.@__DrawButtonFocusRectangle = true;
            this.skinEngine1.DisabledButtonTextColor = System.Drawing.Color.Gray;
            this.skinEngine1.DisabledMenuFontColor = System.Drawing.SystemColors.GrayText;
            this.skinEngine1.InactiveCaptionColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = null;
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.Location = new System.Drawing.Point(415, 60);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(64, 23);
            this.btnSendMsg.TabIndex = 28;
            this.btnSendMsg.Text = "发送";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            this.btnSendMsg.Click += new System.EventHandler(this.btnSendMsg_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(253, 62);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(156, 21);
            this.txtMsg.TabIndex = 29;
            this.txtMsg.Text = "yang^GM^GM^1111^0";
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(495, 3);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(24, 21);
            this.txtCount.TabIndex = 30;
            this.txtCount.Text = "1";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 501);
            this.Controls.Add(this.txtCount);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.btnSendMsg);
            this.Controls.Add(this.chkAutoScroll);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBetEnd);
            this.Controls.Add(this.btnBetStart);
            this.Controls.Add(this.btnResetData);
            this.Controls.Add(this.txtBetPoint);
            this.Controls.Add(this.btnPlayerInfo);
            this.Controls.Add(this.btnGameEnd);
            this.Controls.Add(this.btnGameStart);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnReverseCheck);
            this.Controls.Add(this.btnCheckAll);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.grpClientLst);
            this.Controls.Add(this.btnClearClients);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnBet);
            this.Controls.Add(this.btnQuitGame);
            this.Controls.Add(this.btnWatching);
            this.Controls.Add(this.btnSign);
            this.Controls.Add(this.btnChui);
            this.Controls.Add(this.btnBao);
            this.Controls.Add(this.btnJian);
            this.Controls.Add(this.btnConnect);
            this.Name = "FormMain";
            this.Text = "模拟猜拳客户端";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.grpClientLst.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnJian;
        private System.Windows.Forms.Button btnBao;
        private System.Windows.Forms.Button btnChui;
        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.Button btnWatching;
        private System.Windows.Forms.Button btnQuitGame;
        private System.Windows.Forms.Button btnBet;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnClearClients;
        private System.Windows.Forms.GroupBox grpClientLst;
        private System.Windows.Forms.FlowLayoutPanel flowPnl;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.Button btnReverseCheck;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnGameStart;
        private System.Windows.Forms.Button btnGameEnd;
        private System.Windows.Forms.Button btnPlayerInfo;
        private System.Windows.Forms.TextBox txtBetPoint;
        private System.Windows.Forms.Button btnResetData;
        private System.Windows.Forms.Button btnBetEnd;
        private System.Windows.Forms.Button btnBetStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkAutoScroll;
        private Sunisoft.IrisSkin.SkinEngine skinEngine1;
        private System.Windows.Forms.Button btnSendMsg;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.TextBox txtCount;
    }
}

