using System.Diagnostics;
using System.Runtime.InteropServices;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace YukiBox.Desktop.Hooks;

internal class MouseHook : IDisposable
{
    public event EventHandler<MouseHookEventArgs> LeftButtonDown;

    public event EventHandler<MouseHookEventArgs> LeftButtonUp;

    public event EventHandler<MouseHookEventArgs> RightButtonDown;

    public event EventHandler<MouseHookEventArgs> RightButtonUp;

    public event EventHandler<MouseHookEventArgs> MiddleButtonDown;

    public event EventHandler<MouseHookEventArgs> MiddleButtonUp;

    public event EventHandler<MouseHookEventArgs> MouseMove;

    public event EventHandler<MouseHookEventArgs> MouseWheel;

    private UnhookWindowsHookExSafeHandle _handle = new(nint.Zero);
    private HOOKPROC _hookProc;

    private bool _disposed;

    ~MouseHook()
    {
        Dispose(false);
    }

    public void Install()
    {
        _hookProc = MouseProc;
        _handle = SetHook(_hookProc);
    }

    public void Uninstall()
    {
        if (!_handle.IsClosed && !_handle.IsInvalid)
        {
            _handle.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Uninstall();
            }

            _disposed = true;
        }
    }

    private UnhookWindowsHookExSafeHandle SetHook(HOOKPROC proc)
    {
        using ProcessModule module = Process.GetCurrentProcess().MainModule;
        FreeLibrarySafeHandle hMod = PInvoke.GetModuleHandle(module.ModuleName);
        UnhookWindowsHookExSafeHandle handle = PInvoke.SetWindowsHookEx(WINDOWS_HOOK_ID.WH_MOUSE_LL, proc, hMod, 0);
        return handle;
    }

    private LRESULT MouseProc(int nCode, WPARAM wParam, LPARAM lParam)
    {
        if (nCode < 0)
        {
            LRESULT skipped = PInvoke.CallNextHookEx(null, nCode, wParam, lParam);
            return skipped;
        }

        if (nCode == PInvoke.HC_ACTION)
        {
            uint mouseEvent = (uint)wParam;
            EventHandler<MouseHookEventArgs> handler = mouseEvent switch
            {
                PInvoke.WM_LBUTTONDOWN => LeftButtonDown,
                PInvoke.WM_LBUTTONUP => LeftButtonUp,
                PInvoke.WM_RBUTTONDOWN => RightButtonDown,
                PInvoke.WM_RBUTTONUP => RightButtonUp,
                PInvoke.WM_MBUTTONDOWN => MiddleButtonDown,
                PInvoke.WM_MBUTTONUP => MiddleButtonUp,
                PInvoke.WM_MOUSEMOVE => MouseMove,
                PInvoke.WM_MOUSEWHEEL => MouseWheel,
                _ => null,
            };

            if (handler is not null)
            {
                MSLLHOOKSTRUCT mouseInfo = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
                short wheelDelta = mouseEvent == PInvoke.WM_MOUSEWHEEL ? (short)(mouseInfo.mouseData >> 16) : (short)0;
                MouseHookEventArgs args = new(mouseInfo, wheelDelta);
                handler(this, args);

                if (args.Handled)
                {
                    // A nonzero return value stops the message from reaching the rest of the hook chain and the target window
                    LRESULT handled = new(1);
                    return handled;
                }
            }
        }

        LRESULT result = PInvoke.CallNextHookEx(null, nCode, wParam, lParam);
        return result;
    }
}
