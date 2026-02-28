using System;

namespace YukiBox.Desktop.Tasks
{
    public interface IBackgroundTask : IDisposable
    {
        string Name => this.GetType().FullName;

        TimeSpan Interval { get; }

        int RunCount { get; }

        void Init();

        void Run();

        void Stop();
    }
}