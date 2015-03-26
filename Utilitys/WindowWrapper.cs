using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilitys {
    public class WindowWrapper : System.Windows.Forms.IWin32Window {
        public WindowWrapper(IntPtr handle) {
            _hwnd = handle;
        }
        public WindowWrapper(int handle) {
            _hwnd = new IntPtr(handle);
        }
        public IntPtr Handle {
            get { return _hwnd; }
        }
        private IntPtr _hwnd;
    }
}
