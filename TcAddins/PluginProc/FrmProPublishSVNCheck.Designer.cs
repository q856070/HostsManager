namespace TcAddins.PluginProc {
    partial class FrmProPublishSVNCheck {
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
            this.txt_Log = new System.Windows.Forms.TextBox();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Skip = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_Log
            // 
            this.txt_Log.Location = new System.Drawing.Point(12, 12);
            this.txt_Log.Multiline = true;
            this.txt_Log.Name = "txt_Log";
            this.txt_Log.Size = new System.Drawing.Size(464, 208);
            this.txt_Log.TabIndex = 0;
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(401, 230);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 1;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_Skip
            // 
            this.btn_Skip.Location = new System.Drawing.Point(12, 230);
            this.btn_Skip.Name = "btn_Skip";
            this.btn_Skip.Size = new System.Drawing.Size(75, 23);
            this.btn_Skip.TabIndex = 2;
            this.btn_Skip.Text = "跳过(慎用)";
            this.btn_Skip.UseVisualStyleBackColor = true;
            this.btn_Skip.Click += new System.EventHandler(this.btn_Skip_Click);
            // 
            // FrmProPublishSVNCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 265);
            this.Controls.Add(this.btn_Skip);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.txt_Log);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmProPublishSVNCheck";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "检测全部成员SVN是否都提交了";
            this.Load += new System.EventHandler(this.FrmProPublishSVNCheck_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmProPublishSVNCheck_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmProPublishSVNCheck_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Log;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Skip;
    }
}