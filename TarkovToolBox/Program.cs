using System;

namespace TarkovToolBox
{
    public partial class Program : System.Windows.Application
    {

        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            TarkovToolBox.App app = new TarkovToolBox.App();
            app.InitializeComponent();
            app.MainWindow = new MainWindow();
            app.Run(app.MainWindow);
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            
        }
    }
}
