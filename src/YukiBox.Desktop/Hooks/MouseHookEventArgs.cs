using Windows.Win32.UI.WindowsAndMessaging;

namespace YukiBox.Desktop.Hooks;

internal class MouseHookEventArgs : EventArgs
{
    public MSLLHOOKSTRUCT MouseInfo { get; }

    public short WheelDelta { get; }

    public bool Handled { get; set; }

    public MouseHookEventArgs(
        MSLLHOOKSTRUCT mouseInfo,
        short wheelDelta)
    {
        MouseInfo = mouseInfo;
        WheelDelta = wheelDelta;
    }
}
