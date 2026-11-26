using System.Diagnostics;
using System.Runtime.InteropServices;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace YukiBox.Desktop.Hooks;

internal class MouseHook : IDisposable
{
    public event Action<MSLLHOOKSTRUCT> LeftButtonDown;

    public event Action<MSLLHOOKSTRUCT> LeftButtonUp;

    public event Action<MSLLHOOKSTRUCT> RightButtonDown;

    public event Action<MSLLHOOKSTRUCT> RightButtonUp;

    public event Action<MSLLHOOKSTRUCT> MiddleButtonDown;

    public event Action<MSLLHOOKSTRUCT> MiddleButtonUp;

    public event Action<MSLLHOOKSTRUCT> MouseMove;

    public event Action<MSLLHOOKSTRUCT, short> MouseWheel;

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
            MSLLHOOKSTRUCT mouseInfo = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
            switch (mouseEvent)
            {
                case PInvoke.WM_LBUTTONDOWN:
                    LeftButtonDown?.Invoke(mouseInfo);
                    break;

                case PInvoke.WM_LBUTTONUP:
                    LeftButtonUp?.Invoke(mouseInfo);
                    break;

                case PInvoke.WM_RBUTTONDOWN:
                    RightButtonDown?.Invoke(mouseInfo);
                    break;

                case PInvoke.WM_RBUTTONUP:
                    RightButtonUp?.Invoke(mouseInfo);
                    break;

                case PInvoke.WM_MBUTTONDOWN:
                    MiddleButtonDown?.Invoke(mouseInfo);
                    break;

                case PInvoke.WM_MBUTTONUP:
                    MiddleButtonUp?.Invoke(mouseInfo);
                    break;

                case PInvoke.WM_MOUSEMOVE:
                    MouseMove?.Invoke(mouseInfo);
                    break;

                case PInvoke.WM_MOUSEWHEEL:
                    short delta = (short)(mouseInfo.mouseData >> 16);
                    MouseWheel?.Invoke(mouseInfo, delta);
                    break;

                default:
                    break;
            }
        }

        LRESULT result = PInvoke.CallNextHookEx(null, nCode, wParam, lParam);
        return result;
    }
}
