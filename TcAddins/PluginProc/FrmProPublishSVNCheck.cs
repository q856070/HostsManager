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
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Collections.Specialized;
using Utilitys.Modul;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

namespace TcAddins.PluginProc {
    public partial class FrmProPublishSVNCheck : Form, FormIsClosed {

        private string _softRootPath;
        private string _solutionPath;
        private List<MCoder> allCorder;
        private Thread startThread;
        private bool userExit = false;
        private string _errMsg;
        private DialogResult result = DialogResult.None;

        private List<MProject> allProject = null;
        private MProject currProject = null;

        public string ErrMsg { get { return _errMsg; } }

        public bool IsClosed { get; set; }

        public DialogResult Result { get { return result; } }

        public FrmProPublishSVNCheck(string softRootPath, string solutionPath) {
            _softRootPath = softRootPath;
            _solutionPath = solutionPath;
            InitializeComponent();
            IsClosed = false;
        }

        private void FrmProPublishSVNCheck_Load(object sender, EventArgs e) {
            string msg = CheckCurrProjectCfg();
            if (!string.IsNullOrEmpty(msg)) {
                MessageBox.Show(msg);
                if (!string.IsNullOrEmpty(CheckCurrProjectCfg())) {
                    this.Close();
                    return;
                }
            }
            btn_OK.Enabled = false;
            startThread = new Thread(new ThreadStart(DoStart));
            startThread.Start();
            IsClosed = false;
        }
        private string CheckCurrProjectCfg() {
            allProject = PublishHelper.GetCurrProjectCfg(_softRootPath);
            if (allProject == null) {
                return ("SVN目录未配置,请运行ProjectsSvnCfg.exe进行配置!");
            }
            currProject = allProject.FirstOrDefault(p => p.SvnDir == _solutionPath);
            if (currProject == null) {
                return ("当前解决方案SVN目录未配置,请运行ProjectsSvnCfg.exe进行配置!");
            }
            return string.Empty;
        }
        private void DoStart() {
            string name = PublishHelper.GetCurrCoderName(_softRootPath);
            if (string.IsNullOrEmpty(name)) {
                this.BeginInvoke(new ThreadStart(delegate() {
                    txt_Log.AppendText("当前身份信息未配置,进入配置...\r\n");
                }));
                var coderCfg = new FrmCurrCoderCfg(_softRootPath);
                coderCfg.ShowDialog();
                name = PublishHelper.GetCurrCoderName(_softRootPath);
                if (string.IsNullOrEmpty(name)) {
                    result = DialogResult.Cancel;
                    this.BeginInvoke(new ThreadStart(delegate() {
                        txt_Log.AppendText("未完成配置,点击[确定]退出...\r\n");
                        btn_OK.Enabled = true;
                    }));
                    return;
                }
                this.BeginInvoke(new ThreadStart(delegate() {
                    txt_Log.AppendText("完成配置\r\n");
                }));
            }

            this.BeginInvoke(new ThreadStart(delegate() {
                txt_Log.AppendText("开始连接服务器...\r\n");
            }));
            allCorder = PublishHelper.GetAllCoder(_softRootPath, out _errMsg);
            if (allCorder == null) {
                result = DialogResult.Cancel;
                this.BeginInvoke(new ThreadStart(delegate() {
                    txt_Log.AppendText(_errMsg + "\r\n");
                }));
                return;
            }
            this.BeginInvoke(new ThreadStart(delegate() {
                txt_Log.AppendText("共" + allCorder.Count(c => c.Name != name).ToString() + "人:\r\n");
            }));
            foreach (var coder in allCorder) {
                //自己不查
                if (coder.Name == name) continue;

                this.BeginInvoke(new ThreadStart(delegate() {
                    txt_Log.AppendText(coder.Name + "...");
                }));

                string errMsg;
                bool? svnFull = AskSVNFullSubmit(coder.IP, out errMsg);
                if (!svnFull.HasValue) {
                    //未知就允许跳过
                    result = DialogResult.OK;
                    this.BeginInvoke(new ThreadStart(delegate() {
                        txt_Log.AppendText(errMsg + "\r\n");
                    }));
                } else {
                    if (svnFull.Value) {
                        this.BeginInvoke(new ThreadStart(delegate() {
                            txt_Log.AppendText("已完全提交\r\n");
                        }));
                    } else {
                        result = DialogResult.Cancel;
                        this.BeginInvoke(new ThreadStart(delegate() {
                            txt_Log.AppendText("有未提交的内容!!!\r\n");
                            txt_Log.AppendText("点击[确定]退出\r\n");
                            btn_OK.Enabled = true;
                        }));
                        return;
                    }
                }
            }
            result = DialogResult.OK;
            this.BeginInvoke(new ThreadStart(delegate() {
                txt_Log.AppendText("请点击[确定]进入下一步\r\n");
                btn_OK.Enabled = true;
            }));
        }

        /// <summary>
        /// 询问是否已经完全提交,如果未知返回null,要看errMsg
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool? AskSVNFullSubmit(string ip, out string errMsg) {
            errMsg = "";
            try {
                using (TcpClient client = new TcpClient()) {
                    client.Connect(new System.Net.IPEndPoint(IPAddress.Parse(ip), 7782));
                    if (client.Connected) {
                        var netStream = client.GetStream();
                        if (netStream.CanWrite) {
                            NameValueCollection msg = new NameValueCollection();
                            msg["event"] = "AskSVNFullSubmit";
                            msg["projectName"] = currProject.Name;
                            //先发送数据
                            PublishHelper.WriteStringInStream(netStream, PublishHelper.GetNameValue(msg));
                            netStream.Flush();

                            if (netStream.CanRead) {
                                //接收数据
                                var reData = PublishHelper.GetNameValue(PublishHelper.ReadStringInStream(client.Client));
                                errMsg = reData["errMsg"].GetString("");
                                string re = reData["re"];
                                if (re.IsEmpty()) return null;
                                else return re == "1";
                            }
                        }
                    }
                }
            } catch { errMsg = "连不上该电脑"; return null; }
            return false;
        }



        private void btn_OK_Click(object sender, EventArgs e) {
            userExit = true;
            this.Close();
        }

        private void FrmProPublishSVNCheck_FormClosed(object sender, FormClosedEventArgs e) {
            try { startThread.Abort(); } catch { }
            IsClosed = true;
        }

        private void FrmProPublishSVNCheck_FormClosing(object sender, FormClosingEventArgs e) {
            if (!userExit) {
                result = DialogResult.Cancel;
            }
        }

        private void btn_Skip_Click(object sender, EventArgs e) {
            result = DialogResult.OK;
            userExit = true;
            this.Close();
        }
    }
}
