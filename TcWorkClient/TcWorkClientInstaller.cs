using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ServiceProcess;
using System.Configuration.Install;

namespace TcWorkClient {
    [RunInstallerAttribute(true)]
    public class TcWorkClientInstaller : Installer {
        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller processInstaller;

        public TcWorkClientInstaller() {
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = "TcWorkClient";
            serviceInstaller.DisplayName = "TC团队开发服务";
            serviceInstaller.Description = "用于支持团队开发的一些必要和便捷功能,乃开发之利器";

            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }
    }

}
