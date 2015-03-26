using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HostsManager {
    /// <summary>
    /// HOST方案管理
    /// </summary>
    public static class HostSchemeMgr {

        private static string hostSchemePath = Application.StartupPath + @"\hostscheme\";
        private const string hsExName = ".hs";
        private static Encoding encode = Encoding.UTF8;

        private static void CreateHSDir() {
            if (!Directory.Exists(hostSchemePath)) Directory.CreateDirectory(hostSchemePath);
        }

        public static bool DeleteAllHostScheme() {
            try {
                CreateHSDir();
                Directory.Delete(hostSchemePath);
            } catch { return false; }
            return true;
        }
        public static List<MHostScheme> GetAllHostScheme(bool loadHostStr) {
            CreateHSDir();
            string[] hsList = Directory.GetFiles(hostSchemePath, "*" + hsExName, SearchOption.AllDirectories);
            List<MHostScheme> list = new List<MHostScheme>();
            if (hsList != null) {
                foreach (var hsPath in hsList) {
                    MHostScheme hs = new MHostScheme {
                        SchemeName = Path.GetFileNameWithoutExtension(hsPath),
                        HostLoaded = loadHostStr,
                        HSFilePath = hsPath,
                        HostStr = string.Empty,
                        Changed = false
                    };
                    if (loadHostStr) {
                        hs.HostStr = File.ReadAllText(hsPath, encode);
                    }
                    list.Add(hs);
                }
            }
            return list;
        }
        public static bool ReLoadHostScheme(MHostScheme hostScheme) {
            hostScheme.HostLoaded = false;
            return LoadHostScheme(hostScheme);
        }
        public static bool LoadHostScheme(MHostScheme hostScheme) {
            try {
                CreateHSDir();
                if (!hostScheme.HostLoaded) {
                    hostScheme.HostStr = File.ReadAllText(hostScheme.HSFilePath, encode);
                    hostScheme.HostLoaded = true;
                }
            } catch { return false; }
            return true;
        }
        public static bool DeleteHostScheme(MHostScheme hostScheme) {
            try {
                CreateHSDir();
            } catch { return true; }
            if (File.Exists(hostScheme.HSFilePath)) {
                try {
                    File.Delete(hostScheme.HSFilePath);
                } catch { return false; }
            }
            return true;
        }
        public static MHostScheme AddHostScheme(string hostSchemeName) {
            try {
                CreateHSDir();
                string hsFileName = hostSchemePath + hostSchemeName + hsExName;
                if (File.Exists(hsFileName)) {
                    return null;
                }
                using (StreamWriter fs = new StreamWriter(hsFileName, false, encode)) {
                    fs.Close();
                }
                MHostScheme hs = new MHostScheme {
                    SchemeName = hostSchemeName,
                    HostLoaded = false,
                    HSFilePath = hsFileName,
                    HostStr = string.Empty,
                    Changed = false
                };
                return hs;
            } catch { return null; }
        }
        public static bool SaveHostScheme(MHostScheme hostScheme) {
            try {
                CreateHSDir();
                using (StreamWriter fs = new StreamWriter(hostScheme.HSFilePath, false, encode)) {
                    fs.Write(hostScheme.HostStr);
                    fs.Close();
                }
            } catch { return false; }
            return true;
        }
        public static bool RenameHostScheme(MHostScheme hostScheme, string newHSName) {
            try {
                CreateHSDir();
                string dir = Path.GetDirectoryName(hostScheme.HSFilePath);
                string exName = Path.GetExtension(hostScheme.HSFilePath);
                string newHSPath = dir + "\\" + newHSName + exName;
                if (File.Exists(newHSPath)) return false;
                File.Move(hostScheme.HSFilePath, newHSPath);
                hostScheme.HSFilePath = newHSPath;
                hostScheme.SchemeName = newHSName;
            } catch { return false; }
            return true;
        }
    }
}
