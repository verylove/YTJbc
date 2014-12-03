namespace FakedGameClient.ServerController
{
    partial class FormWaiting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWaiting));
            this.lblNotice = new System.Windows.Forms.Label();
            this.picLoading = new System.Windows.Forms.PictureBox();
            this.btnExit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNotice
            // 
            this.lblNotice.Location = new System.Drawing.Point(22, 19);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.Size = new System.Drawing.Size(342, 21);
            this.lblNotice.TabIndex = 0;
            this.lblNotice.Text = "正在连接服务器...";
            this.lblNotice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picLoading
            // 
            this.picLoading.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picLoading.Image = global::FakedGameClient.ServerController.Properties.Resources.connecting;
            this.picLoading.Location = new System.Drawing.Point(24, 49);
            this.picLoading.Name = "picLoading";
            this.picLoading.Size = new System.Drawing.Size(340, 17);
            this.picLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLoading.TabIndex = 1;
            this.picLoading.TabStop = false;
            this.picLoading.Paint += new System.Windows.Forms.PaintEventHandler(this.picLoading_Paint);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(158, 78);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "取消";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // FormWaiting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 113);
            this.ControlBox = false;
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.picLoading);
            this.Controls.Add(this.lblNotice);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormWaiting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "剪包锤游戏管理程序";
            this.Load += new System.EventHandler(this.FormWaiting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picLoading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblNotice;
        private System.Windows.Forms.PictureBox picLoading;
        private System.Windows.Forms.Button btnExit;
    }
}