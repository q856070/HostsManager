using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebPublisher {
    public class MCfgItem {
        /// <summary>
        /// 原始目录
        /// </summary>
        public string SourceDir { get; set; }
        /// <summary>
        /// 忽略目录列表
        /// </summary>
        public string IgnoreDirs { get; set; }
        /// <summary>
        /// 上次发布时间
        /// </summary>
        public DateTime? LastReleaseTime { get; set; }
    }
}
