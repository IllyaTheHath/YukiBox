using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;

namespace YukiBox.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const String AppName = "YukiBox.Desktop";
        public const String AppDisplayName = "YukiBox";
        public const String AppUuid = "B19A8370-3BD2-452F-851D-7A0058EC35AC";

        private static readonly Mutex mutex = new(true, AppUuid);

        public static String AppVersion
        {
            get => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public App()
        {
            if (!mutex.WaitOne(TimeSpan.Zero, true))
            {
                App.Current.Exit();
            }

            InitializeComponent();

            AppStartup.Instance.Initialize();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            AppStartup.Instance.OnStartup();
        }

        public static new void Exit()
        {
            AppStartup.Instance.OnExit();

            mutex.ReleaseMutex();

            App.Current.Exit();
        }
    }
}