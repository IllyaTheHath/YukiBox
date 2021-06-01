using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YukiBox.Desktop.Tasks
{
    public interface IBackgroundTask : IDisposable
    {
        String Name => this.GetType().FullName;

        TimeSpan Interval { get; }

        Int32 RunCount { get; }

        void Init();

        void Run();

        void Stop();
    }
}