using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using Microsoft.VisualStudio.CommandBars;

namespace TcAddins {
    public class TCommandItem {
        public string CommandName { get; set; }
        public string CommandText { get; set; }
        public string CommandTips { get; set; }
        public int IconID { get; set; }

        public Command CurrVSCommand { get; set; }

        public IPluginProc PluginProc { get; set; }
    }
    public class TCommandBarItem {
        public string CommandName { get; set; }
        public CommandBarButton CurrVSCommandBarBtn { get; set; }
    }
}
