using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Utilitys;
using System.IO;
using Utilitys.Modul;
using System.Diagnostics;
using System.Net;

namespace ProjectsSvnCfg {
    public partial class FrmMain : Form {

        private List<MProject> currNewProjectCfg = new List<MProject>();

        private bool saveFinish = false;

        public FrmMain() {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e) {
            BindShow();
        }

        private void BindShow() {
            txt_ServerAddress.Text = PublishHelper.GetServerAddress(Application.StartupPath);
            txt_CodeDir.Text = PublishHelper.GetCodeDir(Application.StartupPath);
            txt_CoderName.Text = PublishHelper.GetCurrCoderName(Application.StartupPath);
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e) {
            if (!saveFinish) {
                Console.Write("Unfinished");
            }
        }

        private void btn_ServerAddressCheck_Click(object sender, EventArgs e) {
            if (ServerAddressCheck()) {
                MessageBox.Show("成功!");
            }
        }

        private bool ServerAddressCheck() {
            txt_ServerAddress.Text = txt_ServerAddress.Text.Trim();
            if (txt_ServerAddress.Text.IsEmpty()) {
                MessageBox.Show("请先填写发布器服务地址");
                txt_ServerAddress.Focus();
                return false;
            }
            string errMsg;
            var allCoderList = PublishHelper.GetAllCoder(Application.StartupPath, txt_ServerAddress.Text, out errMsg);
            if (allCoderList == null) {
                MessageBox.Show(errMsg);
                txt_ServerAddress.SelectAll();
                txt_ServerAddress.Focus();
                return false;
            }
            return true;
        }

        private void btn_CoderNameCheck_Click(object sender, EventArgs e) {
            if (CoderNameCheck()) {
                MessageBox.Show("成功!");
            }
        }
        private bool CoderNameCheck() {
            txt_ServerAddress.Text = txt_ServerAddress.Text.Trim();
            txt_CoderName.Text = txt_CoderName.Text.Trim();
            if (txt_ServerAddress.Text.IsEmpty()) {
                MessageBox.Show("请先填写发布器服务地址");
                txt_ServerAddress.Focus();
                return false;
            }
            if (txt_CoderName.Text.IsEmpty()) {
                MessageBox.Show("请先填写发布器服务地址");
                txt_ServerAddress.Focus();
                return false;
            }
            string errMsg;
            var allCoderList = PublishHelper.GetAllCoder(Application.StartupPath, txt_ServerAddress.Text, out errMsg);
            if (allCoderList == null) {
                MessageBox.Show(errMsg);
                txt_ServerAddress.SelectAll();
                txt_ServerAddress.Focus();
                return false;
            }

            var currCodeObj = allCoderList.FirstOrDefault(c => c.Name.Equals(txt_CoderName.Text));
            if (currCodeObj == null || Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(a => a.ToString().Equals(currCodeObj.IP)) == null) {
                MessageBox.Show("你的名字不在发布器的服务成员中!");
                txt_ServerAddress.SelectAll();
                txt_ServerAddress.Focus();
                return false;
            }

            return true;
        }

        private void btn_SaveGO_Click(object sender, EventArgs e) {
            if (!ServerAddressCheck()) return;
            if (!CoderNameCheck()) return;

            var codeDir = txt_CodeDir.Text = txt_CodeDir.Text.Trim();
            if (!Directory.Exists(codeDir)) {
                MessageBox.Show("code目录不存在!请重新配置!");
                return;
            }
            PublishHelper.SetServerAddress(Application.StartupPath, txt_ServerAddress.Text);
            PublishHelper.SetCodeDir(Application.StartupPath, codeDir);
            PublishHelper.SetCurrCoderName(Application.StartupPath, txt_CoderName.Text);

            MessageBox.Show("保存成功!");
        }

        private void btn_CodeDirSel_Click(object sender, EventArgs e) {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                string dir = this.folderBrowserDialog1.SelectedPath;
                if (Directory.Exists(dir)) {
                    txt_CodeDir.Text = dir;
                }
            }
        }
    }
}
