namespace FakedGameClient
{
    partial class FormAutoGame
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
            this.btnQuitGame = new System.Windows.Forms.Button();
            this.grpGameInfo = new System.Windows.Forms.GroupBox();
            this.txtReport = new System.Windows.Forms.RichTextBox();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.grpOperation = new System.Windows.Forms.GroupBox();
            this.btnShowAllPlayer = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.grpGameInfo.SuspendLayout();
            this.grpOperation.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnQuitGame
            // 
            this.btnQuitGame.Location = new System.Drawing.Point(163, 16);
            this.btnQuitGame.Name = "btnQuitGame";
            this.btnQuitGame.Size = new System.Drawing.Size(63, 23);
            this.btnQuitGame.TabIndex = 13;
            this.btnQuitGame.Text = "终止";
            this.btnQuitGame.UseVisualStyleBackColor = true;
            this.btnQuitGame.Click += new System.EventHandler(this.btnQuitGame_Click);
            // 
            // grpGameInfo
            // 
            this.grpGameInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpGameInfo.Controls.Add(this.txtReport);
            this.grpGameInfo.Location = new System.Drawing.Point(12, 96);
            this.grpGameInfo.Name = "grpGameInfo";
            this.grpGameInfo.Size = new System.Drawing.Size(237, 403);
            this.grpGameInfo.TabIndex = 16;
            this.grpGameInfo.TabStop = false;
            this.grpGameInfo.Text = "比赛情况";
            // 
            // txtReport
            // 
            this.txtReport.BackColor = System.Drawing.SystemColors.Info;
            this.txtReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReport.Location = new System.Drawing.Point(3, 17);
            this.txtReport.Name = "txtReport";
            this.txtReport.Size = new System.Drawing.Size(231, 383);
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
            this.txtLog.Location = new System.Drawing.Point(255, 12);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(443, 487);
            this.txtLog.TabIndex = 17;
            this.txtLog.Text = "";
            this.txtLog.WordWrap = false;
            // 
            // grpOperation
            // 
            this.grpOperation.Controls.Add(this.btnShowAllPlayer);
            this.grpOperation.Controls.Add(this.btnPause);
            this.grpOperation.Controls.Add(this.btnConnect);
            this.grpOperation.Controls.Add(this.btnQuitGame);
            this.grpOperation.Location = new System.Drawing.Point(12, 12);
            this.grpOperation.Name = "grpOperation";
            this.grpOperation.Size = new System.Drawing.Size(237, 78);
            this.grpOperation.TabIndex = 18;
            this.grpOperation.TabStop = false;
            this.grpOperation.Text = "操作";
            // 
            // btnShowAllPlayer
            // 
            this.btnShowAllPlayer.Location = new System.Drawing.Point(7, 45);
            this.btnShowAllPlayer.Name = "btnShowAllPlayer";
            this.btnShowAllPlayer.Size = new System.Drawing.Size(124, 24);
            this.btnShowAllPlayer.TabIndex = 20;
            this.btnShowAllPlayer.Text = "显示所有玩家信息";
            this.btnShowAllPlayer.UseVisualStyleBackColor = true;
            this.btnShowAllPlayer.Click += new System.EventHandler(this.btnShowAllPlayer_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(87, 16);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(62, 23);
            this.btnPause.TabIndex = 20;
            this.btnPause.Text = "暂停";
            this.btnPause.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(7, 16);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(62, 23);
            this.btnConnect.TabIndex = 19;
            this.btnConnect.Text = "启动测试";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // FormAutoGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 511);
            this.Controls.Add(this.grpOperation);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.grpGameInfo);
            this.MaximizeBox = false;
            this.Name = "FormAutoGame";
            this.Text = "游戏主窗口";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormAutoGame_FormClosed);
            this.Load += new System.EventHandler(this.FormAutoGame_Load);
            this.grpGameInfo.ResumeLayout(false);
            this.grpOperation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnQuitGame;
        private System.Windows.Forms.GroupBox grpGameInfo;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.GroupBox grpOperation;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.RichTextBox txtReport;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnShowAllPlayer;
    }
}