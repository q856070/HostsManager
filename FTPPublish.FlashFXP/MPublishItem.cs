using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTPPublish.FlashFXP {
    /// <summary>
    /// 每个发布项
    /// </summary>
    public class MPublishItem {
        /// <summary>
        /// 要上传的文件路径
        /// </summary>
        public string SourceFilePath { get; set; }
        /// <summary>
        /// 备份的文件路径
        /// </summary>
        public string BackFilePath { get; set; }
        /// <summary>
        /// 在ftp上的路径
        /// </summary>
        public string FtpFilePath { get; set; }
        /// <summary>
        /// 以"\"开头的相对路径(相对base路径)
        /// </summary>
        public string FileRePath { get; set; }
    }
}
