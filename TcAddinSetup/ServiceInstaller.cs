using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Collections;
using System.Configuration.Install;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TcAddinSetup {
    public class ServiceInstaller:IDisposable {


        #region DLLImport
        [DllImport("advapi32.dll")]
        public static extern IntPtr OpenSCManager(string lpMachineName, string lpSCDB, int scParameter);
        [DllImport("Advapi32.dll")]
        public static extern IntPtr CreateService(IntPtr SC_HANDLE, string lpSvcName, string lpDisplayName,
         int dwDesiredAccess, int dwServiceType, int dwStartType, int dwErrorControl, string lpPathName,
         string lpLoadOrderGroup, int lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword);
        [DllImport("advapi32.dll")]
        public static extern void CloseServiceHandle(IntPtr SCHANDLE);
        [DllImport("advapi32.dll")]
        public static extern int StartService(IntPtr SVHANDLE, int dwNumServiceArgs, string lpServiceArgVectors);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern IntPtr OpenService(IntPtr SCHANDLE, string lpSvcName, int dwNumServiceArgs);
        [DllImport("advapi32.dll")]
        public static extern int DeleteService(IntPtr SVHANDLE);
        [DllImport("kernel32.dll")]
        public static extern int GetLastError();
        #endregion DLLImport

        private string _serviceName;
        private string _filepath;
        private System.ServiceProcess.ServiceController _service;

        public ServiceInstaller(string serviceName,string filePath) {
            _serviceName = serviceName;
            _filepath = filePath;
            _service = new System.ServiceProcess.ServiceController(_serviceName);
        }

        public bool Exist {
            get {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController s in services) {
                    if (s.ServiceName.ToLower() == _serviceName.ToLower()) {
                        return true;
                    }
                }
                return false;
            }
        }
        public bool Running {
            get {
                return (_service.Status == ServiceControllerStatus.Running);
            }
        }
        public EServiceStartType StartType {
            get {
                try {
                    RegistryKey regist = Registry.LocalMachine;
                    RegistryKey sysReg = regist.OpenSubKey("SYSTEM");
                    RegistryKey currentControlSet = sysReg.OpenSubKey("CurrentControlSet");
                    RegistryKey services = currentControlSet.OpenSubKey("Services");
                    RegistryKey servicesName = services.OpenSubKey(_serviceName, true);
                    return (EServiceStartType)servicesName.GetValue("Start");
                } catch (Exception ex) {
                    throw ex;
                }
            }
            set {
                try {
                    RegistryKey regist = Registry.LocalMachine;
                    RegistryKey sysReg = regist.OpenSubKey("SYSTEM");
                    RegistryKey currentControlSet = sysReg.OpenSubKey("CurrentControlSet");
                    RegistryKey services = currentControlSet.OpenSubKey("Services");
                    RegistryKey servicesName = services.OpenSubKey(_serviceName, true);
                    servicesName.SetValue("Start", (int)value);
                } catch (Exception ex) {
                    throw ex;
                }
            }
        }

        public void Start() {
            if (_service.Status != ServiceControllerStatus.Running) {
                _service.Start();
                for (int i = 0; i < 200; i++) {
                    _service.Refresh();
                    System.Threading.Thread.Sleep(100);
                    if (_service.Status == System.ServiceProcess.ServiceControllerStatus.Running) {
                        return;
                    }
                }
                throw new Exception("无法启动服务:" + _serviceName);
            }
        }
        public void Stop() {
            if (_service.Status != ServiceControllerStatus.Stopped) {
                _service.Stop();
                for (int i = 0; i < 200; i++) {
                    _service.Refresh();
                    System.Threading.Thread.Sleep(100);
                    if (_service.Status == System.ServiceProcess.ServiceControllerStatus.Stopped) {
                        return;
                    }
                }
                throw new Exception("无法停止服务:" + _serviceName);
            }
        }

        public void InstallService() {
            FileInfo insertFile = new FileInfo(_filepath);  
            IDictionary savedState = new Hashtable();
            using (AssemblyInstaller aInstaller = new AssemblyInstaller(_filepath, new string[] { "/LogFile=" + insertFile.DirectoryName + "\\" + insertFile.Name.Substring(0, insertFile.Name.Length - insertFile.Extension.Length) + ".log" })) {
                aInstaller.UseNewContext = true;
                aInstaller.Path = _filepath;
                aInstaller.Install(null);
                aInstaller.Commit(null);
                aInstaller.Dispose();
            }
            //ProcessStartInfo ps = new ProcessStartInfo("InstallUtil.exe", "\"" + _filepath + "\"");
            //ps.UseShellExecute = false;
            //ps.CreateNoWindow = true;//加这一句 
            //ps.RedirectStandardOutput = true;
            //Process p = Process.Start(ps);
            //p.WaitForExit();
            //string output = p.StandardOutput.ReadToEnd();
            //MessageBox.Show(output);
        }
        public void UnInstallService() {
            IDictionary savedState = new Hashtable();
            try {
                Assembly sevAsb = Assembly.Load(File.ReadAllBytes(_filepath));
                using (AssemblyInstaller aInstaller = new AssemblyInstaller()) {
                    aInstaller.UseNewContext = true;
                    aInstaller.Assembly = sevAsb;
                    aInstaller.CommandLine = new string[] { "/LogFile=" + Path.GetDirectoryName(_filepath) + "\\" + Path.GetFileNameWithoutExtension(_filepath) + ".log" };
                    aInstaller.Uninstall(null);
                    aInstaller.Dispose();
                }
            } catch {
                try {
                    int GENERIC_WRITE = 0x40000000;
                    IntPtr sc_hndl = OpenSCManager(null, null, GENERIC_WRITE);
                    if (sc_hndl.ToInt32() != 0) {
                        int DELETE = 0x10000;
                        IntPtr svc_hndl = OpenService(sc_hndl, _serviceName, DELETE);
                        if (svc_hndl.ToInt32() != 0) {
                            int i = DeleteService(svc_hndl);
                            if (i != 0) {
                                CloseServiceHandle(sc_hndl);
                                //return true;
                            } else {
                                CloseServiceHandle(sc_hndl);
                            }
                        }
                    }
                } catch { }
            }

            //ProcessStartInfo ps = new ProcessStartInfo("InstallUtil.exe", "/u \"" + _filepath + "\"");
            //ps.UseShellExecute = false;
            //ps.CreateNoWindow = true;//加这一句 
            //ps.RedirectStandardOutput = true;
            //Process p = Process.Start(ps);
            //p.WaitForExit();
            //string output = p.StandardOutput.ReadToEnd();
            //MessageBox.Show(output);
        }

        #region IDisposable 成员

        public void Dispose() {
            _service.Dispose();
        }

        #endregion
    }

    public enum EServiceStartType {
        Auto=2,
        Hand=3,
        Disabled = 4
    }




    class ServiceInstaller2 {
        #region DLLImport
        [DllImport("advapi32.dll")]
        public static extern IntPtr OpenSCManager(string lpMachineName, string lpSCDB, int scParameter);
        [DllImport("Advapi32.dll")]
        public static extern IntPtr CreateService(IntPtr SC_HANDLE, string lpSvcName, string lpDisplayName,
         int dwDesiredAccess, int dwServiceType, int dwStartType, int dwErrorControl, string lpPathName,
         string lpLoadOrderGroup, int lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword);
        [DllImport("advapi32.dll")]
        public static extern void CloseServiceHandle(IntPtr SCHANDLE);
        [DllImport("advapi32.dll")]
        public static extern int StartService(IntPtr SVHANDLE, int dwNumServiceArgs, string lpServiceArgVectors);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern IntPtr OpenService(IntPtr SCHANDLE, string lpSvcName, int dwNumServiceArgs);
        [DllImport("advapi32.dll")]
        public static extern int DeleteService(IntPtr SVHANDLE);
        [DllImport("kernel32.dll")]
        public static extern int GetLastError();
        #endregion DLLImport

        ///  
        /// 安装和运行 
        ///  
        /// 程序路径. 
        /// 服务名 
        /// 服务显示名称. 
        /// 服务安装是否成功. 
        public bool InstallService(string svcPath, string svcName, string svcDispName) {
            #region Constants declaration.
            int SC_MANAGER_CREATE_SERVICE = 0x0002;
            int SERVICE_WIN32_OWN_PROCESS = 0x00000010;
            //int SERVICE_DEMAND_START = 0x00000003; 
            int SERVICE_ERROR_NORMAL = 0x00000001;
            int STANDARD_RIGHTS_REQUIRED = 0xF0000;
            int SERVICE_QUERY_CONFIG = 0x0001;
            int SERVICE_CHANGE_CONFIG = 0x0002;
            int SERVICE_QUERY_STATUS = 0x0004;
            int SERVICE_ENUMERATE_DEPENDENTS = 0x0008;
            int SERVICE_START = 0x0010;
            int SERVICE_STOP = 0x0020;
            int SERVICE_PAUSE_CONTINUE = 0x0040;
            int SERVICE_INTERROGATE = 0x0080;
            int SERVICE_USER_DEFINED_CONTROL = 0x0100;
            int SERVICE_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED |
             SERVICE_QUERY_CONFIG |
             SERVICE_CHANGE_CONFIG |
             SERVICE_QUERY_STATUS |
             SERVICE_ENUMERATE_DEPENDENTS |
             SERVICE_START |
             SERVICE_STOP |
             SERVICE_PAUSE_CONTINUE |
             SERVICE_INTERROGATE |
             SERVICE_USER_DEFINED_CONTROL);
            int SERVICE_AUTO_START = 0x00000002;
            #endregion Constants declaration.
            try {
                IntPtr sc_handle = OpenSCManager(null, null, SC_MANAGER_CREATE_SERVICE);
                if (sc_handle.ToInt32() != 0) {
                    IntPtr sv_handle = CreateService(sc_handle, svcName, svcDispName, SERVICE_ALL_ACCESS, SERVICE_WIN32_OWN_PROCESS, SERVICE_AUTO_START, SERVICE_ERROR_NORMAL, svcPath, null, 0, null, null, null);
                    if (sv_handle.ToInt32() == 0) {
                        CloseServiceHandle(sc_handle);
                        return false;
                    } else {
                        //试尝启动服务 
                        int i = StartService(sv_handle, 0, null);
                        if (i == 0) {

                            return false;
                        }

                        CloseServiceHandle(sc_handle);
                        return true;
                    }
                } else

                    return false;
            } catch (Exception e) {
                throw e;
            }
        }
        ///  
        /// 反安装服务. 
        ///  
        /// 服务名. 
        public bool UnInstallService(string svcName) {
            int GENERIC_WRITE = 0x40000000;
            IntPtr sc_hndl = OpenSCManager(null, null, GENERIC_WRITE);
            if (sc_hndl.ToInt32() != 0) {
                int DELETE = 0x10000;
                IntPtr svc_hndl = OpenService(sc_hndl, svcName, DELETE);
                if (svc_hndl.ToInt32() != 0) {
                    int i = DeleteService(svc_hndl);
                    if (i != 0) {
                        CloseServiceHandle(sc_hndl);
                        return true;
                    } else {
                        CloseServiceHandle(sc_hndl);
                        return false;
                    }
                } else
                    return false;
            } else
                return false;
        }

    } 


}
