using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Utilitys {
    public static class StringExtend {

        /// <summary>   
        /// DES加密方法，用UTF8编码
        /// </summary>   
        /// <param name="inBytes">待加密数组</param>   
        /// <param name="keyBytes">key数组</param>   
        /// <param name="ivBytes">iv数组</param>   
        /// <returns>加密完的数组</returns>   
        public static string DESEncryptWithCBCZeros(this string inp, string key) {
            return DESEncryptWithCBCZeros(inp, key, Encoding.UTF8);
        }
        /// <summary>   
        /// DES加密方法   
        /// </summary>   
        /// <param name="inBytes">待加密数组</param>   
        /// <param name="keyBytes">key数组</param>   
        /// <param name="ivBytes">iv数组</param>   
        /// <returns>加密完的数组</returns>   
        public static string DESEncryptWithCBCZeros(this string inp, string key, Encoding encode) {
            DESCBCEncry enr = new DESCBCEncry(encode);
            return enr.GetEncrypt(inp, key);
        }

        /// <summary>   
        /// DES解密方法，用UTF8编码
        /// </summary>   
        /// <param name="inBytes">待解密数组</param>   
        /// <param name="keyBytes">key数组</param>   
        /// <param name="ivBytes">iv数组</param>   
        /// <returns>解密完的数组</returns>   
        public static string DESDecryptWithCBCZeros(this string inp, string key) {
            return DESDecryptWithCBCZeros(inp, key, Encoding.UTF8);
        }
        /// <summary>   
        /// DES解密方法
        /// </summary>   
        /// <param name="inBytes">待解密数组</param>   
        /// <param name="keyBytes">key数组</param>   
        /// <param name="ivBytes">iv数组</param>   
        /// <returns>解密完的数组</returns>   
        public static string DESDecryptWithCBCZeros(this string inp, string key, Encoding encode) {
            DESCBCEncry enr = new DESCBCEncry(encode);
            return enr.GetDecrypt(inp, key);
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5(this string input) {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(input, "MD5");
        }




        /// <summary>
        /// 是否为空,包括null,空字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string input) {
            return string.IsNullOrEmpty(input);
        }
        /// <summary>
        /// 是否不为空,包括null,空字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNoEmpty(this string input) {
            return !string.IsNullOrEmpty(input);
        }
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UrlEncode(this string input, Encoding encode) {
            return HttpUtility.UrlEncode(input, encode);
        }
        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UrlDecode(this string input, Encoding encode) {
            return HttpUtility.UrlDecode(input, encode);
        }
    }
}
