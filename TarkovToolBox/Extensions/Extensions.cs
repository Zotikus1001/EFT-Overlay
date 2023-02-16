using CefSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace TarkovToolBox.Extensions
{
    public static class Extensions
    {
        public static System.Windows.Media.Color WithAlpha(this System.Drawing.Color color, int newA) {
            var aplhColor = System.Drawing.Color.FromArgb(newA,color);
            return System.Windows.Media.Color.FromArgb(aplhColor.A, aplhColor.R, aplhColor.G, aplhColor.B);
        }

        public static int AddCollection(this UIElementCollection collection, IEnumerable<Control> controls)
        {
            if (controls == null)
                return 0;

            if (collection == null)
                throw new InvalidOperationException("An exception occured while trying to add child controls to a null ui object.");

            foreach (var control in controls)
            {
                collection.Add(control);
            }

            return collection.Count;
        }
    }

    public class MyCustomResourceRequestHandler : CefSharp.Handler.ResourceRequestHandler
    {
        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            return CefReturnValue.Cancel;
        }
    }

    public class MyBasicRequestHandler : CefSharp.Handler.RequestHandler
    {
        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            Debug.WriteLine("In GetResourceRequestHandler : " + request.Url);
            //Only intercept specific Url's
            if (request.Url.Contains("googleadservices")
                || request.Url.Contains("doubleclick.net")
                || request.Url.Contains("nitropay")
                || request.Url.Contains("google-analytics")
                || request.Url.Contains("ad.turn.com")
                || request.Url.Contains("ad-delivery.net")
                || request.Url.Contains("amazon-adsystem"))
            {
                return new MyCustomResourceRequestHandler();
            }
            //Default behaviour, url will be loaded normally.
            return null;
        }
    }

    public static class WindowFocuser
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_SHOWNORMAL = 1;

        public static void FocusWindow(string windowName)
        {
            Process[] processes = Process.GetProcessesByName(windowName);
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