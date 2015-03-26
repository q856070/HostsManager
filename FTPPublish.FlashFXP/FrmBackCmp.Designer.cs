namespace FTPPublish.FlashFXP {
    partial class FrmBackCmp {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_OpenBackDir = new System.Windows.Forms.Button();
            this.txt_ShowBackDir = new System.Windows.Forms.TextBox();
            this.btn_OpenBackDirInFTP = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.p_WebCfgHand = new System.Windows.Forms.Panel();
            this.lab_WebCfgHandContinue = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_HandUpWebCfg = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.p_WebCfgHand.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(3, 3);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(145, 23);
            this.btn_OK.TabIndex = 0;
            this.btn_OK.Text = "继续上传";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(414, 3);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "备份到:";
            // 
            // btn_OpenBackDir
            // 
            this.btn_OpenBackDir.Location = new System.Drawing.Point(199, 42);
            this.btn_OpenBackDir.Name = "btn_OpenBackDir";
            this.btn_OpenBackDir.Size = new System.Drawing.Size(174, 23);
            this.btn_OpenBackDir.TabIndex = 3;
            this.btn_OpenBackDir.Text = "在资源管理器中打开备份目录";
            this.btn_OpenBackDir.UseVisualStyleBackColor = true;
            this.btn_OpenBackDir.Click += new System.EventHandler(this.btn_OpenBackDir_Click);
            // 
            // txt_ShowBackDir
            // 
            this.txt_ShowBackDir.Location = new System.Drawing.Point(70, 15);
            this.txt_ShowBackDir.Name = "txt_ShowBackDir";
            this.txt_ShowBackDir.ReadOnly = true;
            this.txt_ShowBackDir.Size = new System.Drawing.Size(402, 21);
            this.txt_ShowBackDir.TabIndex = 4;
            // 
            // btn_OpenBackDirInFTP
            // 
            this.btn_OpenBackDirInFTP.Location = new System.Drawing.Point(70, 42);
            this.btn_OpenBackDirInFTP.Name = "btn_OpenBackDirInFTP";
            this.btn_OpenBackDirInFTP.Size = new System.Drawing.Size(123, 23);
            this.btn_OpenBackDirInFTP.TabIndex = 2;
            this.btn_OpenBackDirInFTP.Text = "FTP中打开备份目录";
            this.btn_OpenBackDirInFTP.UseVisualStyleBackColor = true;
            this.btn_OpenBackDirInFTP.Click += new System.EventHandler(this.btn_OpenBackDirInFTP_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.p_WebCfgHand);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(497, 191);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btn_OpenBackDirInFTP);
            this.panel1.Controls.Add(this.txt_ShowBackDir);
            this.panel1.Controls.Add(this.btn_OpenBackDir);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(492, 72);
            this.panel1.TabIndex = 6;
            // 
            // p_WebCfgHand
            // 
            this.p_WebCfgHand.Controls.Add(this.lab_WebCfgHandContinue);
            this.p_WebCfgHand.Controls.Add(this.label2);
            this.p_WebCfgHand.Controls.Add(this.btn_HandUpWebCfg);
            this.p_WebCfgHand.Location = new System.Drawing.Point(3, 81);
            this.p_WebCfgHand.Name = "p_WebCfgHand";
            this.p_WebCfgHand.Size = new System.Drawing.Size(492, 63);
            this.p_WebCfgHand.TabIndex = 71;
            this.p_WebCfgHand.Visible = false;
            // 
            // lab_WebCfgHandContinue
            // 
            this.lab_WebCfgHandContinue.AutoSize = true;
            this.lab_WebCfgHandContinue.Location = new System.Drawing.Point(9, 41);
            this.lab_WebCfgHandContinue.Name = "lab_WebCfgHandContinue";
            this.lab_WebCfgHandContinue.Size = new System.Drawing.Size(227, 12);
            this.lab_WebCfgHandContinue.TabIndex = 21;
            this.lab_WebCfgHandContinue.Text = "↓如果已经确认修改完,可继续下面的上传";
            this.lab_WebCfgHandContinue.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(9, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(323, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "本次发布文件中包含Web.config文件,请手动修改上传后继续";
            // 
            // btn_HandUpWebCfg
            // 
            this.btn_HandUpWebCfg.Location = new System.Drawing.Point(338, 6);
            this.btn_HandUpWebCfg.Name = "btn_HandUpWebCfg";
            this.btn_HandUpWebCfg.Size = new System.Drawing.Size(75, 23);
            this.btn_HandUpWebCfg.TabIndex = 0;
            this.btn_HandUpWebCfg.Text = "手动上传";
            this.btn_HandUpWebCfg.UseVisualStyleBackColor = true;
            this.btn_HandUpWebCfg.Click += new System.EventHandler(this.btn_HandUpWebCfg_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_Cancel);
            this.panel2.Controls.Add(this.btn_OK);
            this.panel2.Location = new System.Drawing.Point(3, 150);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(492, 31);
            this.panel2.TabIndex = 6;
            // 
            // FrmBackCmp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(497, 191);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBackCmp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "完成备份,开始上传";
            this.Load += new System.EventHandler(this.FrmBackCmp_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.p_WebCfgHand.ResumeLayout(false);
            this.p_WebCfgHand.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_OpenBackDir;
        private System.Windows.Forms.TextBox txt_ShowBackDir;
        private System.Windows.Forms.Button btn_OpenBackDirInFTP;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel p_WebCfgHand;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_HandUpWebCfg;
        private System.Windows.Forms.Label lab_WebCfgHandContinue;
    }
}