using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace YukiBox.Desktop.Tasks
{
    public abstract class BackgroundTaskBase : IBackgroundTask
    {
        private readonly Timer _timer;
        private Int32 _alreadyRunCount;

        public TimeSpan Interval { get; init; }

        public Int32 RunCount { get; init; }

        public Boolean IsRunning { get; private set; }

        public BackgroundTaskBase(TimeSpan interval, Int32 runCount = 0)
        {
            Interval = interval;
            RunCount = runCount;

            this._timer = new();
            this._timer.Interval = Interval.TotalMilliseconds;
            this._timer.Elapsed += async (s, o) => await TickAction();
            this._timer.Enabled = false;
        }

        public virtual void Init()
        {
        }

        public void Run()
        {
            this._timer.Enabled = true;
            this._timer.Start();
            IsRunning = true;
        }

        public void Stop()
        {
            this._timer.Stop();
            this._timer.Enabled = false;
            IsRunning = false;
        }

        private async Task TickAction()
        {
            if (RunCount <= 0 || this._alreadyRunCount++ < RunCount)
            {
                await Action();
            }
            else
            {
                Stop();
            }
        }

        protected virtual async Task Action()
        {
            await Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            this._timer.Stop();
            this._timer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}