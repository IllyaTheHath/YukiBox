using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YukiBox.Desktop.Helpers
{
    public static class AppLifecycleManager
    {
        private static readonly Mutex mutex = new(true, Program.AppUuid);

        public static void StartApplication(Action action)
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                action();
            }
            else
            {
                //Environment.Exit(0);
                App.Current.Shutdown();
            }
        }

        public static void ReleaseMutex()
        {
            mutex.ReleaseMutex();
        }
    }
}
