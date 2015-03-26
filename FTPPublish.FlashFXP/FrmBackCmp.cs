using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace FTPPublish.FlashFXP {
    public partial class FrmBackCmp : Form {

        private string _flashFXPPath;
        private string _flashFXPPwd;
        private string _siteName;
        private string _publishCurrDir;
        private string _upDir;
        private string _backDir;
        private string _ftpPath;
        private List<MPublishItem> _plublshItems;

        private string _arPwd = "";

        public FrmBackCmp(string flashFXPPath, string flashFXPPwd, string siteName, string publishCurrDir, string upDir, string backDir, string ftpPath, List<MPublishItem> plublshItems) {
            _flashFXPPath = flashFXPPath;
            _flashFXPPwd = flashFXPPwd;
            _siteName = siteName;
            _publishCurrDir = publishCurrDir;
            _upDir = upDir;
            _backDir = backDir;
            _ftpPath = ftpPath;
            _plublshItems = plublshItems;
            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            if (!string.IsNullOrEmpty(_flashFXPPwd)) {
                _arPwd = "-pass=\"" + _flashFXPPwd + "\" ";
            }
        }

        private void btn_OpenBackDir_Click(object sender, EventArgs e) {
            Utilitys.Shell.ExeShell("explorer.exe", _publishCurrDir);
        }

        private void FrmBackCmp_Load(object sender, EventArgs e) {
            txt_ShowBackDir.Text = _publishCurrDir;
            txt_ShowBackDir.SelectionStart = txt_ShowBackDir.TextLength;
            this.Height = 0;
            if (File.Exists(_upDir + "\\Web.config")) {
                p_WebCfgHand.Visible = true;
                btn_OK.Enabled = false;
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_OK_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_OpenBackDirInFTP_Click(object sender, EventArgs e) {

            Utilitys.Shell.ExeShell(_flashFXPPath, _siteName + " " + _arPwd + "-local=\"" + _publishCurrDir + "\" -remotepath=\"" + _ftpPath + "\"");
        }

        private void btn_HandUpWebCfg_Click(object sender, EventArgs e) {

            string backWebCfg = _publishCurrDir + @"\Web.config";
            try {
                if (File.Exists(backWebCfg)) File.Delete(backWebCfg);
            } catch { }

            Utilitys.Shell.ExeShell(_flashFXPPath, _arPwd + @"-local=""" + _publishCurrDir + @""" -download " + _siteName + @" -remotepath=""" + _ftpPath + @"/Web.config"" -localpath=""" + backWebCfg + @""" ");

            btn_OK.Enabled = true;
            lab_WebCfgHandContinue.Visible = true;
        }
    }
}
