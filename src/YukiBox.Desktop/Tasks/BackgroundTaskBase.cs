using Timer = System.Timers.Timer;

namespace YukiBox.Desktop.Tasks;

public abstract class BackgroundTaskBase : IBackgroundTask
{
    private readonly Timer _timer;
    private int _alreadyRunCount;

    public TimeSpan Interval { get; init; }

    public int RunCount { get; init; }

    public bool IsRunning { get; private set; }

    public BackgroundTaskBase(TimeSpan interval, int runCount = 0)
    {
        Interval = interval;
        RunCount = runCount;

        _timer = new();
        _timer.Interval = Interval.TotalMilliseconds;
        _timer.Elapsed += async (s, o) => await TickActionAsync();
        _timer.Enabled = false;
    }

    public virtual void Init()
    {
    }

    public void Run()
    {
        _timer.Enabled = true;
        _timer.Start();
        IsRunning = true;
    }

    public void Stop()
    {
        _timer.Stop();
        _timer.Enabled = false;
        IsRunning = false;
    }

    private async Task TickActionAsync()
    {
        if (RunCount <= 0 || _alreadyRunCount++ < RunCount)
        {
            await ActionAsync();
        }
        else
        {
            Stop();
        }
    }

    protected virtual async Task ActionAsync()
    {
        await Task.CompletedTask;
    }

    public virtual void Dispose()
    {
        _timer.Stop();
        _timer.Dispose();
        GC.SuppressFinalize(this);
    }
}
