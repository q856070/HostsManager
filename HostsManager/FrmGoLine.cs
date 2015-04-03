using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HostsManager {
	/// <summary>
	/// 跳转到指定行窗体
	/// </summary>
	public partial class FrmGoLine : Form {
		private TextBox _sourceTxt = null;

		public int Max { get { return this._sourceTxt.Lines.Length; } }
		public int Min { get { return 1; } }

		public int Line {
			get {
				return Convert.ToInt32(this.txtLine.Text) - 1;
			}
		}
		public FrmGoLine(TextBox txt) {
			InitializeComponent();
			if (txt == null) {
				throw new System.NullReferenceException("指定的文本框为空。");
			}
			_sourceTxt = txt;
			this.txtLine.KeyDown += txtLine_KeyDown;
			this.Load += (sender, e) => {
				this.txtLine.Focus();
				this.AcceptButton = this.btnConfirm;
				this.CancelButton = this.btnCancel;
				this.lblLines.Text += string.Format("({0} - {1})", this.Min, this.Max);
			};
		}

		void txtLine_KeyDown(object sender, KeyEventArgs e) {
			char c = (char)e.KeyValue;
			//只验证0 - 9的字符，验证输入数是否超出限制
			if ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)) {
				if (char.IsNumber(c)) {
					if (txtLine.SelectedText.Length > 0) {
						txtLine.Text = string.Empty;
					}
					string str = this.txtLine.Text + c.ToString();
					int num = Convert.ToInt32(str);
					if (num <= this.Max && num >= this.Min) {
						this.btnConfirm.Enabled = true;
						this.txtLine.BackColor = System.Drawing.Color.WhiteSmoke;

						return;
					}
				}
			}

			this.btnConfirm.Enabled = false;
			this.txtLine.BackColor = System.Drawing.Color.Red;
			e.Handled = true;

		}

		private void btnConfirm_Click(object sender, EventArgs e) {
			int charCount = 0;
			for (int i = 0; i < _sourceTxt.Lines.Length; i++) {
				if (i == this.Line) {
					this._sourceTxt.Select(charCount, _sourceTxt.Lines[i].Length);
					this._sourceTxt.ScrollToCaret();
					this.Close();
					break;
				}
				charCount += _sourceTxt.Lines[i].Length + 2;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e) {
			this.Close();
		}
	}
}
