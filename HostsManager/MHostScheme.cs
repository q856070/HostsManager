using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace HostsManager {
    /// <summary>
    /// HOST方案
    /// </summary>
    public class MHostScheme {
        public string SchemeName { get; set; }
        public string HSFilePath { get; set; }
        public string HostStr { get; set; }
        public bool HostLoaded { get; set; }

        public bool Changed { get; set; }

        public TabPage ShowTabPage { get; set; }
    }
}
