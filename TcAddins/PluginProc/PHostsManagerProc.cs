using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EnvDTE;

namespace TcAddins.PluginProc {
    public class PHostsManagerProc : IPluginProc {

        private const string path = @"\plugin\HostsManager.exe";

        #region IPluginProc 成员

        public void Start(EnvDTE80.DTE2 application, EnvDTE.AddIn addInInst, string addinDllPath) {
            string addinPath = addinDllPath;
            Utilitys.Shell.ExeShell(addinPath + path);
        }

        #endregion

        #region IPluginProc 成员

        public void QueryCommandStatus(EnvDTE80.DTE2 application, EnvDTE.AddIn addInInst, string addinDllPath, vsCommandStatusTextWanted neededText, ref vsCommandStatus statusOption, ref object commandText) {
            statusOption = vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
        }

        #endregion
    }
}
