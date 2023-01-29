using CefSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// <summary>
        /// Called on the CEF IO thread before a resource request is loaded. To redirect or change the resource load optionally modify
        /// <paramref name="request"/>. Modification of the request URL will be treated as a redirect.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - can be modified in this callback.</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of url requests.</param>
        /// <returns>
        /// Return <see cref="CefReturnValue.Continue"/> to continue the request immediately. Return
        /// <see cref="CefReturnValue.ContinueAsync"/> and call <see cref="IRequestCallback.Continue"/> or
        /// <see cref="IRequestCallback.Cancel"/> at a later time to continue or the cancel the request asynchronously. Return
        /// <see cref="CefReturnValue.Cancel"/> to cancel the request immediately.
        /// </returns>
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
            if (request.Url.Contains("googleadservices.com") 
                || request.Url.Contains("doubleclick.net")
                || request.Url.Contains("nitropay.com")
                || request.Url.Contains("google-analytics.com"))
            {
                return new MyCustomResourceRequestHandler();
            }
            //Default behaviour, url will be loaded normally.
            return null;
        }
    }
}