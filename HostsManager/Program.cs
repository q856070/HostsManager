using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HostsManager {
    static class Program {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] commands) {

            bool createNew;
            try {
                using (System.Threading.Mutex m = new System.Threading.Mutex(true, "Global\\" + Application.ProductName, out createNew)) {
                    if (createNew) {
                        try {
                            Application.EnableVisualStyles();
                            Application.SetCompatibleTextRenderingDefault(false);
                            Application.Run(new FrmMain(commands));
                        } catch {}
                    } else {
                        ShowCurrForm(commands);
                    }
                }
            } catch {
                ShowCurrForm(commands);
            }

        }

        static void ShowCurrForm(string[] commands) {
            foreach (var cmd in commands) {
                WinMessageUtil.SendMessage(Application.ProductName, 0, cmd);
            }
        }
    }
}
