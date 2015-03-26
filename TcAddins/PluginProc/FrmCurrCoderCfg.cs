using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utilitys;

namespace TcAddins.PluginProc {
    public partial class FrmCurrCoderCfg : Form {
        private string _startupPath;
        public FrmCurrCoderCfg(string startupPath) {
            _startupPath = startupPath;
            InitializeComponent();
        }

        private void btn_OK_Click(object sender, EventArgs e) {
            string name = txt_Name.Text = txt_Name.Text.Trim();

            if (string.IsNullOrEmpty(name)) {
                MessageBox.Show("请输入你的姓名");
                txt_Name.Focus();
            }

            PublishHelper.SetCurrCoderName(_startupPath, name);

            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
