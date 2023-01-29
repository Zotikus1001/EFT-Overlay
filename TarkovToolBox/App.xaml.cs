using System;
using System.Windows;
using TarkovToolBox.Utils;
using System.Timers;
using System.Diagnostics;

namespace TarkovToolBox
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public MainWindow ToolBoxOverlay { get; set; }
        public LowLevelKeyboardListener keyboardListener { get; set; }

        public Timer orphanedTimer { get; set; }
        Timer key_cooldown_timer = new Timer(100);

        void App_Startup(object sender, StartupEventArgs e)
        {
            keyboardListener = new LowLevelKeyboardListener();
            keyboardListener.HookKeyboard();
            keyboardListener.OnKeyPressed += KeyboardListener_OnKeyPressed;

            ToolBoxOverlay = (MainWindow)this.MainWindow;

            orphanedTimer = new Timer();
            orphanedTimer.Elapsed += OrphanedTimer_Elapsed;
            orphanedTimer.Enabled = true;
            orphanedTimer.Interval = 250;
            orphanedTimer.Start();
        }

        private void ToolBoxOverlay_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Debug.WriteLine("WINDOW DED APP");
        }

        private void OrphanedTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ToolBoxOverlay.Dispatcher.Invoke((Action)(() =>
            {
                if (ToolBoxOverlay.WindowState == WindowState.Maximized)
                {
                    if (!TarkovIsFocused() && !ProcessWatcher.LastActiveWindowWasTarkov())
                        ToolBoxOverlay.WindowState = WindowState.Normal;
                }
            }));
        }

        private void KeyboardListener_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            if (e.KeyPressed == System.Windows.Input.Key.M)
            {
                if (!key_cooldown_timer.Enabled)
                {
                    key_cooldown_timer.AutoReset = false;
                    key_cooldown_timer.Start();
                    ToggleOverlay(e, TarkovIsFocused());
                }
            }
        }

        private void ToggleOverlay(KeyPressedArgs e, bool tIsFocused)
        {
            if (ToolBoxOverlay.WindowState == WindowState.Normal && tIsFocused)
                PerformToggle(WindowState.Maximized);
            else if (ToolBoxOverlay.WindowState == WindowState.Maximized && tIsFocused)
                PerformToggle(WindowState.Normal);
            else if(ToolBoxOverlay.WindowState == WindowState.Maximized && !tIsFocused && ProcessWatcher.LastActiveWindowWasTarkov())
                PerformToggle(WindowState.Normal);

            void PerformToggle(WindowState windowState)
            {
                ToolBoxOverlay.WindowState = windowState;

                switch (windowState)
                {
                    case WindowState.Maximized:
                        ToolBoxOverlay.Focus();
                        ToolBoxOverlay.Activate();
                        break;
                    default:
                        TarkovActivator.ActivateTarkov();
                        break;
                }
            }
        }

        private bool TarkovIsFocused()
        {
            return TarkovStateChecker.IsTarkovActive();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //Console.WriteLine("STEP3");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //Console.WriteLine("STEP1");
            CleanUp();
            base.OnExit(e);
        }

        private void CleanUp()
        {
            Debug.WriteLine("Cleaing Keyboard Listener...");
            keyboardListener.UnHookKeyboard();
            Debug.WriteLine("Cleaing Process watcher...");
            Debug.WriteLine("Cleaing Orphan Timer...");
            orphanedTimer.Stop();
            orphanedTimer.Dispose();
        }

        private void Application_SessionEnding_1(object sender, SessionEndingCancelEventArgs e)
        {
            ////Doesn't fire for some reason:
            //Console.WriteLine("STEP2");
        }
    }
}
