using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace TarkovToolBox.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : BaseView
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void BaseView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWelcomeBlurb();
        }

        private void LoadWelcomeBlurb()
        {
            var range = new TextRange(WelcomRTB.Document.ContentStart, WelcomRTB.Document.ContentEnd);
            using (MemoryStream memLicStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(Properties.Resources.welcome_blurb)))
            {
                range.Load(memLicStream, DataFormats.Rtf);
            }
            WelcomRTB.IsEnabled = false;
        }
    }
}
