using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using Utilitys;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;
using Utilitys.Modul;
using System.Xml.Serialization;
using System.Net;
using System.Runtime.Serialization.Formatters.Soap;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TcWorkClient {
    public class ServiceMain : System.ServiceProcess.ServiceBase {

        TcpListener listener = new TcpListener(IPAddress.Any, 7782);

        public ServiceMain() {
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
        }

        protected override void OnStart(string[] args) {
            listener.Start();
            StartGetLink();
        }

        protected override void OnStop() {
            try {
                listener.Stop();
            } catch { }
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
                throw new Exception(projectName + "SVN目录未配置,请运行ProjectsSvnCfg.exe进行配置!");
            }
            var project = allProject.FirstOrDefault(p => p.Name == projectName);
            if (project == null) {
                throw new Exception(projectName + "SVN目录未配置,请运行ProjectsSvnCfg.exe进行配置!");
            }

            //string exePath = "svn.exe";
            //string argm = "svn.exe";
            //STARTUPINFO sInfo = new STARTUPINFO();
            //sInfo.lpDesktop = "Winsta0\\Default";
            //PROCESS_INFORMATION pInfo = new PROCESS_INFORMATION();
            //if (!CreateProcess(new StringBuilder(exePath), new StringBuilder(argm), null, null, false, 0, null, null, ref sInfo, ref pInfo)) {
            //    throw new Exception("调用SVN命令失败");
            //}
            //uint i = 0;
            //WaitForSingleObject(pInfo.hProcess, int.MaxValue);
            //GetExitCodeProcess(pInfo.hProcess, ref i);
            //CloseHandle(pInfo.hProcess);
            //CloseHandle(pInfo.hThread);


            GetDesktopWindow();
            IntPtr hwinstaSave = GetProcessWindowStation();
            IntPtr dwThreadId = GetCurrentThreadId();
            IntPtr hdeskSave = GetThreadDesktop(dwThreadId);
            IntPtr hwinstaUser = OpenWindowStation("WinSta0", false, 33554432);
            if (hwinstaUser == IntPtr.Zero) {
                RpcRevertToSelf();
                throw new Exception("SVN命令行启动失败!");
            }
            SetProcessWindowStation(hwinstaUser);
            IntPtr hdeskUser = OpenDesktop("Default", 0, false, 33554432);
            RpcRevertToSelf();
            if (hdeskUser == IntPtr.Zero) {
                SetProcessWindowStation(hwinstaSave);
                CloseWindowStation(hwinstaUser);
                throw new Exception("SVN命令行启动失败!");
            }
            SetThreadDesktop(hdeskUser);

            IntPtr dwGuiThreadId = dwThreadId;

            string svnOutStr = "";
            using (Process process = new Process()) {
                process.StartInfo.FileName = "svn.exe";
                process.StartInfo.Verb = "hide";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.Arguments = @"status """ + project.SvnDir + @"""";
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                process.Start();
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


            dwGuiThreadId = IntPtr.Zero;
            SetThreadDesktop(hdeskSave);
            SetProcessWindowStation(hwinstaSave);
            CloseDesktop(hdeskUser);
            CloseWindowStation(hwinstaUser);

            return true;
        }


        [System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
        public class SECURITY_ATTRIBUTES {
            public int nLength;
            public string lpSecurityDescriptor;
            public bool bInheritHandle;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFO {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public int lpTitle;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public int wShowWindow;
            public int cbReserved2;
            public byte lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }
        [DllImport("Kernel32.dll", CharSet = CharSet.Ansi)]
        public static extern bool CreateProcess(
            StringBuilder lpApplicationName, StringBuilder lpCommandLine,
            SECURITY_ATTRIBUTES lpProcessAttributes,
            SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandles,
            int dwCreationFlags,
            StringBuilder lpEnvironment,
            StringBuilder lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            ref PROCESS_INFORMATION lpProcessInformation
            );
        #region Win32 Api : WaitForSingleObject
        //检测一个系统核心对象(线程，事件，信号)的信号状态，当对象执行时间超过dwMilliseconds就返回，否则就一直等待对象返回信号
        [DllImport("Kernel32.dll")]
        public static extern uint WaitForSingleObject(System.IntPtr hHandle, uint dwMilliseconds);

        #endregion

        #region Win32 Api : CloseHandle
        //关闭一个内核对象,释放对象占有的系统资源。其中包括文件、文件映射、进程、线程、安全和同步对象等
        [DllImport("Kernel32.dll")]
        public static extern bool CloseHandle(System.IntPtr hObject);

        #endregion

        #region Win32 Api : GetExitCodeProcess
        //获取一个已中断进程的退出代码,非零表示成功，零表示失败。
        //参数hProcess，想获取退出代码的一个进程的句柄，参数lpExitCode，用于装载进程退出代码的一个长整数变量。
        [DllImport("Kernel32.dll")]
        static extern bool GetExitCodeProcess(System.IntPtr hProcess, ref uint lpExitCode);
        #endregion  



        

        #region WinAPI

        [DllImport("user32.dll")]
        static extern int GetDesktopWindow();

        [DllImport("user32.dll")]
        static extern IntPtr GetProcessWindowStation();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThreadId();

        [DllImport("user32.dll")]
        static extern IntPtr GetThreadDesktop(IntPtr dwThread);

        [DllImport("user32.dll")]
        static extern IntPtr OpenWindowStation(string parm1, bool parm2, int parm3);

        [DllImport("user32.dll")]
        static extern IntPtr OpenDesktop(string lpszDesktop, uint dwFlags, bool fInherit, uint dwDesiredAccess);

        [DllImport("user32.dll")]
        static extern IntPtr CloseDesktop(IntPtr p);

        [DllImport("rpcrt4.dll", SetLastError = true)]
        static extern IntPtr RpcImpersonateClient(int i);

        [DllImport("rpcrt4.dll", SetLastError = true)]
        static extern IntPtr RpcRevertToSelf();

        [DllImport("user32.dll")]
        static extern IntPtr SetThreadDesktop(IntPtr a);

        [DllImport("user32.dll")]
        static extern IntPtr SetProcessWindowStation(IntPtr a);

        [DllImport("user32.dll")]
        static extern IntPtr CloseWindowStation(IntPtr a);

        #endregion
    }

}
