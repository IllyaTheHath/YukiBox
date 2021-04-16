using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Services;

namespace YukiBox.Desktop
{
    public static class Program
    {
        public const String AppName = "YukiBox.Desktop";
        public const String AppUuid = "B19A8370-3BD2-452F-851D-7A0058EC35AC";

        [STAThread]
        private static void Main(String[] args)
        {
            try
            {
                var thread = new Thread(() =>
                {
                    AppLifecycleManager.StartApplication(() =>
                    {
                        //I18NSource.Instance.Initialize();

                        var app = new App();
                        app.Run();
                    });
                });

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                AppLifecycleManager.ReleaseMutex();
            }
        }

        public static String AppVersion
        {
            get => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
