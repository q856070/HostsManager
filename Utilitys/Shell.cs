using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Utilitys {
    public static class Shell {

        public enum ShowCommands : int {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }

        [DllImport("shell32.dll")]
        private static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            ShowCommands nShowCmd);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern unsafe bool ReadConsoleOutput(IntPtr hConsoleOutput, string pBuffer, int bufferSize, int bufferCoord, ref string readRegion);

        public struct COORD {
            public short X;
            public short Y;
            public short Right;
            public short Bottom;
        }

        [DllImport("kernel32.dll")]
        static extern bool ReadConsoleOutputCharacter(IntPtr hConsoleOutput,
          [Out] StringBuilder lpCharacter, uint nLength, COORD dwReadCoord,
          out uint lpNumberOfCharsRead);



        public static IntPtr ExeShell(string filePath) {
            return ShellExecute(IntPtr.Zero, "open", filePath, "", "", ShowCommands.SW_SHOW);
        }
        public static IntPtr ExeShell(string filePath, string arg) {
            return ShellExecute(IntPtr.Zero, "open", filePath, arg, "", ShowCommands.SW_SHOW);
        }
        public static IntPtr ExeShell(string filePath, string arg, ShowCommands showCmd) {
            return ShellExecute(IntPtr.Zero, "open", filePath, arg, "", showCmd);
        }
        public static IntPtr ExeShell(IntPtr hwnd, string filePath, string arg, ShowCommands showCmd) {
            return ShellExecute(hwnd, "open", filePath, arg, "", showCmd);
        }

    }
}
