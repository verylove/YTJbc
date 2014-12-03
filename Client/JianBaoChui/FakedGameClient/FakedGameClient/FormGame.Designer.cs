namespace FakedGameClient
{
    partial class FormGame
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnBet = new System.Windows.Forms.Button();
            this.btnQuitGame = new System.Windows.Forms.Button();
            this.btnWatching = new System.Windows.Forms.Button();
            this.btnSign = new System.Windows.Forms.Button();
            this.btnChui = new System.Windows.Forms.Button();
            this.btnBao = new System.Windows.Forms.Button();
            this.btnJian = new System.Windows.Forms.Button();
            this.grpGameInfo = new System.Windows.Forms.GroupBox();
            this.txtReport = new System.Windows.Forms.RichTextBox();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.grpOperation = new System.Windows.Forms.GroupBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.cmbPoint = new System.Windows.Forms.ComboBox();
            this.pi = new FakedGameClient.PlayerInfoControl();
            this.grpGameInfo.SuspendLayout();
            this.grpOperation.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBet
            // 
            this.btnBet.Enabled = false;
            this.btnBet.Location = new System.Drawing.Point(75, 82);
            this.btnBet.Name = "btnBet";
            this.btnBet.Size = new System.Drawing.Size(63, 23);
            this.btnBet.TabIndex = 15;
            this.btnBet.Text = "下注";
            this.btnBet.UseVisualStyleBackColor = true;
            this.btnBet.Click += new System.EventHandler(this.btnBet_Click);
            // 
            // btnQuitGame
            // 
            this.btnQuitGame.Location = new System.Drawing.Point(144, 82);
            this.btnQuitGame.Name = "btnQuitGame";
            this.btnQuitGame.Size = new System.Drawing.Size(63, 23);
            this.btnQuitGame.TabIndex = 13;
            this.btnQuitGame.Text = "退出";
            this.btnQuitGame.UseVisualStyleBackColor = true;
            this.btnQuitGame.Click += new System.EventHandler(this.btnQuitGame_Click);
            // 
            // btnWatching
            // 
            this.btnWatching.Enabled = false;
            this.btnWatching.Location = new System.Drawing.Point(144, 16);
            this.btnWatching.Name = "btnWatching";
            this.btnWatching.Size = new System.Drawing.Size(63, 23);
            this.btnWatching.TabIndex = 14;
            this.btnWatching.Text = "观战";
            this.btnWatching.UseVisualStyleBackColor = true;
            this.btnWatching.Click += new System.EventHandler(this.btnWatching_Click);
            // 
            // btnSign
            // 
            this.btnSign.Enabled = false;
            this.btnSign.Location = new System.Drawing.Point(75, 16);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(63, 23);
            this.btnSign.TabIndex = 12;
            this.btnSign.Text = "报名";
            this.btnSign.UseVisualStyleBackColor = true;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // btnChui
            // 
            this.btnChui.Enabled = false;
            this.btnChui.Location = new System.Drawing.Point(144, 49);
            this.btnChui.Name = "btnChui";
            this.btnChui.Size = new System.Drawing.Size(63, 23);
            this.btnChui.TabIndex = 11;
            this.btnChui.Text = "锤";
            this.btnChui.UseVisualStyleBackColor = true;
            this.btnChui.Click += new System.EventHandler(this.btnChui_Click);
            // 
            // btnBao
            // 
            this.btnBao.Enabled = false;
            this.btnBao.Location = new System.Drawing.Point(75, 49);
            this.btnBao.Name = "btnBao";
            this.btnBao.Size = new System.Drawing.Size(63, 23);
            this.btnBao.TabIndex = 10;
            this.btnBao.Text = "包";
            this.btnBao.UseVisualStyleBackColor = true;
            this.btnBao.Click += new System.EventHandler(this.btnBao_Click);
            // 
            // btnJian
            // 
            this.btnJian.Enabled = false;
            this.btnJian.Location = new System.Drawing.Point(6, 49);
            this.btnJian.Name = "btnJian";
            this.btnJian.Size = new System.Drawing.Size(63, 23);
            this.btnJian.TabIndex = 9;
            this.btnJian.Text = "剪";
            this.btnJian.UseVisualStyleBackColor = true;
            this.btnJian.Click += new System.EventHandler(this.btnJian_Click);
            // 
            // grpGameInfo
            // 
            this.grpGameInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpGameInfo.Controls.Add(this.txtReport);
            this.grpGameInfo.Location = new System.Drawing.Point(12, 437);
            this.grpGameInfo.Name = "grpGameInfo";
            this.grpGameInfo.Size = new System.Drawing.Size(222, 23);
            this.grpGameInfo.TabIndex = 16;
            this.grpGameInfo.TabStop = false;
            this.grpGameInfo.Text = "比赛情况";
            // 
            // txtReport
            // 
            this.txtReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReport.Location = new System.Drawing.Point(3, 17);
            this.txtReport.Name = "txtReport";
            this.txtReport.Size = new System.Drawing.Size(216, 3);
            this.txtReport.TabIndex = 19;
            this.txtReport.Text = "";
            this.txtReport.WordWrap = false;
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.Location = new System.Drawing.Point(240, 12);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(429, 448);
            this.txtLog.TabIndex = 17;
            this.txtLog.Text = "";
            this.txtLog.WordWrap = false;
            // 
            // grpOperation
            // 
            this.grpOperation.Controls.Add(this.btnConnect);
            this.grpOperation.Controls.Add(this.cmbPoint);
            this.grpOperation.Controls.Add(this.btnSign);
            this.grpOperation.Controls.Add(this.btnJian);
            this.grpOperation.Controls.Add(this.btnBao);
            this.grpOperation.Controls.Add(this.btnBet);
            this.grpOperation.Controls.Add(this.btnChui);
            this.grpOperation.Controls.Add(this.btnQuitGame);
            this.grpOperation.Controls.Add(this.btnWatching);
            this.grpOperation.Location = new System.Drawing.Point(12, 12);
            this.grpOperation.Name = "grpOperation";
            this.grpOperation.Size = new System.Drawing.Size(222, 121);
            this.grpOperation.TabIndex = 18;
            this.grpOperation.TabStop = false;
            this.grpOperation.Text = "操作";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(7, 16);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(62, 23);
            this.btnConnect.TabIndex = 19;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // cmbPoint
            // 
            this.cmbPoint.Enabled = false;
            this.cmbPoint.FormattingEnabled = true;
            this.cmbPoint.Items.AddRange(new object[] {
            "100",
            "200",
            "500",
            "1000"});
            this.cmbPoint.Location = new System.Drawing.Point(6, 82);
            this.cmbPoint.Name = "cmbPoint";
            this.cmbPoint.Size = new System.Drawing.Size(63, 20);
            this.cmbPoint.TabIndex = 19;
            this.cmbPoint.Text = "200";
            // 
            // pi
            // 
            this.pi.AutoSize = true;
            this.pi.BetPoint = "";
            this.pi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pi.GameState = "";
            this.pi.GameStatus = "";
            this.pi.Gesture = "";
            this.pi.ID = "";
            this.pi.Level = "";
            this.pi.Location = new System.Drawing.Point(36, 139);
            this.pi.LvNo = "";
            this.pi.Name = "pi";
            this.pi.PlayerName = "";
            this.pi.PlayerState = "";
            this.pi.PlayerStatus = "";
            this.pi.RivalID = "";
            this.pi.RivalLvNo = "";
            this.pi.RoundResult = "";
            this.pi.Size = new System.Drawing.Size(168, 292);
            this.pi.TabIndex = 21;
            // 
            // FormGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 472);
            this.Controls.Add(this.pi);
            this.Controls.Add(this.grpOperation);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.grpGameInfo);
            this.Name = "FormGame";
            this.Text = "游戏主窗口";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormGame_FormClosed);
            this.Load += new System.EventHandler(this.FormGame_Load);
            this.grpGameInfo.ResumeLayout(false);
            this.grpOperation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBet;
        private System.Windows.Forms.Button btnQuitGame;
        private System.Windows.Forms.Button btnWatching;
        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.Button btnChui;
        private System.Windows.Forms.Button btnBao;
        private System.Windows.Forms.Button btnJian;
        private System.Windows.Forms.GroupBox grpGameInfo;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.GroupBox grpOperation;
        private System.Windows.Forms.ComboBox cmbPoint;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.RichTextBox txtReport;
        private PlayerInfoControl pi;
    }
}