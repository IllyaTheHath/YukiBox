namespace YukiBox.Desktop.Hooks;

internal class HooksHelper
{
    private static readonly Lazy<HooksHelper> s_lazy = new(() => new HooksHelper());

    public static HooksHelper Instance => s_lazy.Value;

    public MouseHook MouseHook { get; private set; }

    public WheelVolumeHookHelper WheelVolumeHookHelper { get; private set; }

    private HooksHelper()
    {
    }

    public void Initialize()
    {
        MouseHook = new MouseHook();
        MouseHook.Install();

        WheelVolumeHookHelper = new WheelVolumeHookHelper();
        WheelVolumeHookHelper.Initialize();
    }

    public void Dispose()
    {
        WheelVolumeHookHelper.IsEnabled = false;
        MouseHook.Dispose();
    }
}
