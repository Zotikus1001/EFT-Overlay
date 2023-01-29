using CefSharp;
using CefSharp.Wpf;
using System.Windows;
using System.Windows.Interop;
using TarkovToolBox.Extensions;

namespace TarkovToolBox.Views
{
    /// <summary>
    /// Interaction logic for WikiView.xaml
    /// </summary>
    public partial class WikiView : BaseView
    {
        public WikiView()
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
                InitMarketBrowser("https://escapefromtarkov.gamepedia.com/Escape_from_Tarkov_Wiki");
        }
    }
}
