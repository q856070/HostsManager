namespace TcAddinSetup {
    partial class FrmMain {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.btn_GO = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_SoftPath = new System.Windows.Forms.TextBox();
            this.btn_SelectSoftPath = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_GO
            // 
            this.btn_GO.Location = new System.Drawing.Point(142, 148);
            this.btn_GO.Name = "btn_GO";
            this.btn_GO.Size = new System.Drawing.Size(85, 44);
            this.btn_GO.TabIndex = 0;
            this.btn_GO.Text = "安装";
            this.btn_GO.UseVisualStyleBackColor = true;
            this.btn_GO.Click += new System.EventHandler(this.btn_GO_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "选择安装路径：";
            // 
            // txt_SoftPath
            // 
            this.txt_SoftPath.Location = new System.Drawing.Point(12, 115);
            this.txt_SoftPath.Name = "txt_SoftPath";
            this.txt_SoftPath.Size = new System.Drawing.Size(305, 21);
            this.txt_SoftPath.TabIndex = 3;
            // 
            // btn_SelectSoftPath
            // 
            this.btn_SelectSoftPath.Location = new System.Drawing.Point(323, 113);
            this.btn_SelectSoftPath.Name = "btn_SelectSoftPath";
            this.btn_SelectSoftPath.Size = new System.Drawing.Size(42, 23);
            this.btn_SelectSoftPath.TabIndex = 1;
            this.btn_SelectSoftPath.Text = "...";
            this.btn_SelectSoftPath.UseVisualStyleBackColor = true;
            this.btn_SelectSoftPath.Click += new System.EventHandler(this.btn_SelectSoftPath_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(377, 97);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 204);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txt_SoftPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_SelectSoftPath);
            this.Controls.Add(this.btn_GO);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "工具集安装";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_GO;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_SoftPath;
        private System.Windows.Forms.Button btn_SelectSoftPath;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

