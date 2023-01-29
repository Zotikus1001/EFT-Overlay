﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TarkovToolBox.Utils
{
    public static class TarkovStateChecker
    {
        public static bool IsTarkovActive()
        {
            IntPtr intPtr = GetForegroundWindow();
            return TestWindowText(intPtr);

            bool TestWindowText(IntPtr handle)
            {
                const int nChars = 256;
                StringBuilder Buff = new StringBuilder(nChars);
                if (GetWindowText(handle, Buff, nChars) > 0)
                    return Buff.ToString() == "EscapeFromTarkov";
                else
                    return false;
            }
        }

        private static WINDOWPLACEMENT GetPlacement(IntPtr hwnd)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(hwnd, ref placement);
            return placement;
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowPlacement(
            IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public ShowWindowCommands showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        public enum ShowWindowCommands : int
        {
            Hide = 0,
            Normal = 1,
            Minimized = 2,
            Maximized = 3,
        }
    }
}
