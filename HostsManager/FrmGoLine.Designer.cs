namespace HostsManager {
	partial class FrmGoLine {
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
			this.btnConfirm = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtLine = new System.Windows.Forms.TextBox();
			this.lblLines = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnConfirm
			// 
			this.btnConfirm.Location = new System.Drawing.Point(133, 67);
			this.btnConfirm.Name = "btnConfirm";
			this.btnConfirm.Size = new System.Drawing.Size(75, 23);
			this.btnConfirm.TabIndex = 0;
			this.btnConfirm.Text = "确定";
			this.btnConfirm.UseVisualStyleBackColor = true;
			this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(224, 67);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// txtLine
			// 
			this.txtLine.Location = new System.Drawing.Point(15, 31);
			this.txtLine.Name = "txtLine";
			this.txtLine.Size = new System.Drawing.Size(284, 21);
			this.txtLine.TabIndex = 0;
			this.txtLine.Text = "1";
			// 
			// lblLines
			// 
			this.lblLines.AutoSize = true;
			this.lblLines.Font = new System.Drawing.Font("宋体", 9.5F);
			this.lblLines.Location = new System.Drawing.Point(12, 14);
			this.lblLines.Name = "lblLines";
			this.lblLines.Size = new System.Drawing.Size(33, 13);
			this.lblLines.TabIndex = 2;
			this.lblLines.Text = "行号";
			// 
			// FrmGoLine
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(313, 106);
			this.Controls.Add(this.lblLines);
			this.Controls.Add(this.txtLine);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnConfirm);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmGoLine";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "转到行";
			this.ShowIcon = false;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnConfirm;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox txtLine;
		private System.Windows.Forms.Label lblLines;

	}
}