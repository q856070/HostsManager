using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Utilitys;
using System.Collections.Specialized;
using System.Net.Sockets;
using System.Net;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace TcWorkClient {
    public partial class FrmMain : Form {

        TcpListener listener = new TcpListener(IPAddress.Any, 7782);


        public FrmMain() {
            InitializeComponent();
            SetAutoStart();

            listener.Start();
            StartGetLink();
        }

        private void FrmMain_Load(object sender, EventArgs e) {
        }

        private void StartGetLink() {
            listener.BeginAcceptTcpClient(new AsyncCallback(GetLinkCBack), listener);
        }
        private void GetLinkCBack(IAsyncResult ar) {
            TcpListener listener = (TcpListener)ar.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(ar);
            StartGetLink();
            NetworkStream netStream = client.GetStream();
            if (netStream.CanRead) {
                //处理得到接收de数据
                string reMsg = ProcMsg(PublishHelper.ReadStringInStream(client.Client));
                //返回处理后的数据
                PublishHelper.WriteStringInStream(netStream, reMsg);
                netStream.Flush();
            }
            client.Close();
            netStream.Close();
        }
        private string ProcMsg(string msg) {
            var msgData = PublishHelper.GetNameValue(msg);
            NameValueCollection reData = new NameValueCollection();
            reData["state"] = "200";
            switch (msgData["event"]) {
                case "AskSVNFullSubmit":
                    try {
                        reData["re"] = AskSVNFullSubmit(msgData["projectName"]) ? "1" : "0";
                    } catch (Exception ex) {
                        reData["errMsg"] = "客户端发生错误:" + ex.Message;
                    }
                    break;
            }
            return PublishHelper.GetNameValue(reData);
        }
        private bool AskSVNFullSubmit(string projectName) {
            var allProject = PublishHelper.GetCurrProjectCfg(Application.StartupPath);
            if (allProject == null) {
                throw new Exception(projectName + "的SVN目录未配置,请他运行ProjectsSvnCfg.exe进行配置!");
            }
            var project = allProject.FirstOrDefault(p => p.Name == projectName);
            if (project == null) {
                throw new Exception(projectName + "的SVN目录未配置,请他运行ProjectsSvnCfg.exe进行配置!");
            }

            string svnOutStr = "";
            using (Process process = new Process()) {
                process.StartInfo.FileName = "svn.exe";
                process.StartInfo.Verb = "hide";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.Arguments = @"status """ + project.SvnDir + @"""";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                process.Start();
                //ShowWindow(process.Handle, 0);
                //svn.exe status "D:\ZUM\SVN\Code\SoftwarePublicized\DNFData"
                do {
                    svnOutStr = process.StandardOutput.ReadToEnd().Replace("\r", "");
                    string[] svnDirRows = svnOutStr.Split('\n');
                    foreach (var rowStr in svnDirRows) {
                        if (rowStr.StartsWith("M", true, null)
                            || rowStr.StartsWith("C", true, null)
                            || rowStr.StartsWith("A", true, null)
                            || rowStr.StartsWith("K", true, null)) {
                            return false;
                        }
                    }
                } while (!process.HasExited);
            }

            return true;
        }

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);



        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e) {
            try {
                listener.Stop();
            } catch { }
        }

        private void SetAutoStart() {
            const string KEY = "TcWorkClient";
            //获取启动程序的路径和名称
            string path = Application.ExecutablePath;

            RegistryKey HKLM = Registry.LocalMachine;
            try {
                RegistryKey Run = HKLM.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (path != Run.GetValue(KEY, null)) {
                    Run.SetValue(KEY, path);
                }
            } catch { } finally {
                HKLM.Close();
            }
        }

    }
}
