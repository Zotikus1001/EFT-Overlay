using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TarkovToolBox.Utils
{
    public static class TarkovActivator
    {

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_SHOWNORMAL = 1;


        public static void ActivateTarkov()
        {
            Process[] processes = Process.GetProcessesByName("EscapeFromTarkov");
            if (processes.Length > 0)
            {
                var hWnd = processes.First().MainWindowHandle;
                if (hWnd != IntPtr.Zero)
                {
                    ShowWindow(hWnd, SW_SHOWNORMAL);
                    SetForegroundWindow(hWnd);
                }
            }
        }
    }
}
