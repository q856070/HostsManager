using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;

namespace WebPublisher {
    static class Program {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] command) {
            List<string> projectDirs = new List<string>();
            string solutionName = "";

            if (command != null) {
                string inCmd = "";
                foreach (var cmd in command) {
                    if (cmd[0] == '-') {
                        inCmd = cmd;
                        continue;
                    }
                    switch (inCmd) {
                        case "-solution":
                            solutionName = cmd.Trim();
                            break;
                        case "-projectlist":
                            projectDirs.Add(cmd.Trim());
                            break;
                    }
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain(solutionName, projectDirs));
        }
    }
}
