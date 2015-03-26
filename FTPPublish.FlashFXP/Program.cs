using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace FTPPublish.FlashFXP {
    static class Program {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] command) {
            string sourceDir = "";
            string publishKey = "";
            string publishCurrDir = "";
            string uploadDir = "";
            string backDir = "";
            if (command != null) {
                string inCmd = "";
                foreach (var cmd in command) {
                    if (cmd[0] == '-') {
                        inCmd = cmd;
                        continue;
                    }
                    switch (inCmd) {
                        case "-sourceDir":
                            sourceDir = cmd.Trim();
                            break;
                        case "-publishKey":
                            publishKey = cmd.Trim();
                            break;
                        case "-publishCurrDir":
                            publishCurrDir = cmd.Trim();
                            break;
                        case "-uploadDir":
                            uploadDir = cmd.Trim();
                            break;
                        case "-backDir":
                            backDir = cmd.Trim();
                            break;
                    }
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain(sourceDir, publishKey, publishCurrDir, uploadDir, backDir));
        }
    }
}
