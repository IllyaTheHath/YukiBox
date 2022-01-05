using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppLifecycle;

using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

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

        public static Boolean Exiting { get; private set; }
        public static String AppVersion
        {
            get => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public App()
        {
            var appArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
            var instance = AppInstance.FindOrRegisterForKey(AppUuid);
            if (!instance.IsCurrent)
            {
                instance.RedirectActivationToAsync(appArgs).GetResults();
                Process.GetCurrentProcess().Kill();
                return;
            }

            InitializeComponent();

            AppStartup.Instance.Initialize();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            AppStartup.Instance.OnStartup();
        }

        /// <summary>
        /// Override App Exit
        /// </summary>
        public static new void Exit()
        {
            Exiting = true;

            AppStartup.Instance.OnExit();

            App.Current.Exit();
        }
    }
}