using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web;

namespace Utilitys {
    /// <summary>
    /// DES-CBC加解密
    /// </summary>
    public class DESCBCEncry {

        //private static Encoding tencoding = System.Text.Encoding.GetEncoding("utf-8");
        private Encoding tencoding = System.Text.Encoding.UTF8;
        // System.Text.ASCIIEncoding.ASCII; //System.Text.Encoding.GetEncoding("gb2312");

        public DESCBCEncry(Encoding encode) {
            tencoding = encode;
        }

        /// <summary>
        /// 取Key
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private byte[] GetKey(string key) {
            /*
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(tencoding.GetBytes(key));
            byte[] ret = new byte[24];
            Array.Copy(t, ret, 24);
             * */
            return tencoding.GetBytes(key.MD5().Substring(0, 8).ToLower());
        }



        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public string GetEncrypt(string toEncry, string key) {
            DESCryptoServiceProvider sa = new DESCryptoServiceProvider();
            sa.IV = sa.Key = GetKey(key);
            sa.Mode = CipherMode.CBC;

            //SymmetricAlgorithm sa = DES.Create();
            //sa.IV = sa.Key = GetKey(key);
            //sa.Mode = CipherMode.CBC;

            sa.Padding = PaddingMode.Zeros;

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, sa.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] byt = tencoding.GetBytes(toEncry);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
            //return ms.ToArray().ByteArrayToHexString();
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="EnctyptStr"></param>
        /// <returns></returns>
        public string GetDecrypt(string EncryptStr, string key) {
            byte[] byt = Convert.FromBase64String(EncryptStr);
            //byte[] byt = EncryptStr.HexStringToByteArray();
            DESCryptoServiceProvider sa = new DESCryptoServiceProvider();
            sa.IV = sa.Key = GetKey(key);
            sa.Mode = CipherMode.CBC;
            sa.Padding = PaddingMode.Zeros;
            MemoryStream ms = new MemoryStream(byt);
            CryptoStream cs = new CryptoStream(ms, sa.CreateDecryptor(), CryptoStreamMode.Read);
            MemoryStream msoutput = new MemoryStream();
            int tmp = cs.ReadByte();
            while (tmp > 0) {
                msoutput.WriteByte((byte)tmp);
                tmp = cs.ReadByte();
            }
            string str = tencoding.GetString(msoutput.ToArray());
            cs.Close();
            ms.Close();
            msoutput.Close();
            return str;
        }

    } 
}
