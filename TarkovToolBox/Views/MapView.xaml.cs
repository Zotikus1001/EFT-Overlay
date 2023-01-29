using CefSharp;
using CefSharp.Wpf;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using TarkovToolBox.Extensions;

namespace TarkovToolBox.Views
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : BaseView
    {
        public MapView()
        {
            InitializeComponent();
        }

        ChromiumWebBrowser MarketBrowser { get; set; }

        private void InitMarketBrowser(string url)
        {
            BrowserSettings settings = new BrowserSettings
            {
                WindowlessFrameRate = 60,
                WebGl = CefState.Enabled
            };
            ChromiumWebBrowser MarketBrowser = new ChromiumWebBrowser(url)
            {
                BrowserSettings = settings
            };
            Window visual = Application.Current.Windows[Application.Current.Windows.Count - 1];
            HwndSource parentWindowHwndSource = (HwndSource)HwndSource.FromVisual(visual);
            MarketBrowser.CreateBrowser(parentWindowHwndSource, new Size(100, 100));
            MarketBrowser.Name = $"browser_Market";
            MarketBrowser.RequestHandler = new MyBasicRequestHandler();
            BrowserContainerBorder.Child = MarketBrowser;
        }

        private void BaseView_Loaded(object sender, RoutedEventArgs e)
        {
            if (MarketBrowser == null)
                InitMarketBrowser("https://www.gamemaps.co.uk/game/tarkov"); //https://eftmkg.com/
        }
    }
}