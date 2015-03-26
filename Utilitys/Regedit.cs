using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace Utilitys {
    public class Regedit {
        private string _softName;
        public Regedit(string softName) {
            _softName = softName;
        }

        private RegistryKey GetOrCreate(RegistryKey parent, string name, bool writable) {
            RegistryKey aimdir = parent.OpenSubKey(name, writable);
            if (aimdir == null && writable) aimdir = parent.CreateSubKey(name);
            return aimdir;
        }

        public string GetRegistPath() {
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = GetOrCreate(hkml, "SOFTWARE", false);
            if (software == null) return string.Empty;
            RegistryKey aimdir = GetOrCreate(software, _softName, false);
            if (aimdir == null) return string.Empty;
            return aimdir.ToString();
        }

        /// <summary>
        /// 读取指定名称的注册表的值
        /// </summary>
        /// <param name="name">注册表值</param>
        /// <returns></returns>
        public object GetRegistData(string name) {
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = GetOrCreate(hkml, "SOFTWARE", false);
            if (software == null) return string.Empty;
            RegistryKey aimdir = GetOrCreate(software, _softName, false);
            if (aimdir == null) return string.Empty;
            return aimdir.GetValue(name);
        }

        /// <summary>
        /// 注册表中写数据 
        /// </summary>
        /// <param name="name">注册表</param>
        /// <param name="tovalue">值</param>
        public void WTRegedit(string name, object value) {
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = GetOrCreate(hkml, "SOFTWARE", true);
            RegistryKey aimdir = GetOrCreate(software, _softName, true);
            aimdir.SetValue(name, value);
        }

        /// <summary>
        /// .删除注册表中指定的注册表项
        /// </summary>
        /// <param name="name">注册表</param>
        public void DeleteRegist(string name) {
            string[] aimnames;
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = GetOrCreate(hkml, "SOFTWARE", true);
            RegistryKey aimdir = GetOrCreate(software, _softName, true);
            aimnames = aimdir.GetSubKeyNames();
            foreach (string aimKey in aimnames) {
                if (aimKey == name)
                    aimdir.DeleteSubKeyTree(name);
            }
        }

        /// <summary>
        /// 判断指定注册表项是否存在
        /// </summary>
        /// <param name="name">注册表</param>
        /// <returns></returns>
        public bool IsRegeditExit(string name) {
            string[] subkeyNames;
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = GetOrCreate(hkml, "SOFTWARE", false);
            if (software == null) return false;
            RegistryKey aimdir = GetOrCreate(software, _softName, false);
            if (aimdir == null) return false;
            subkeyNames = aimdir.GetSubKeyNames();
            foreach (string keyName in subkeyNames) {
                if (keyName == name) {
                    return true;
                }
            }
            return false;
        }


    }
}