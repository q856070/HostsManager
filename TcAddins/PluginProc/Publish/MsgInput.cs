using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TcAddins.PluginProc.Publish {
    public partial class MsgInput : Form {
        public MsgInput() {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 获取用户输入的文本
        /// </summary>
        public string Msg {
            get {
                return txt_Msg.Text;
            }
        }

        private void btn_OK_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void txt_Msg_KeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.Enter) {
                btn_OK_Click(btn_OK, EventArgs.Empty);
            }
        }
    }
}
