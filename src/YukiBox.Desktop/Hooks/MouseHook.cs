using System.Diagnostics;
using System.Runtime.InteropServices;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using Windows.Win32.UI.WindowsAndMessaging;

namespace YukiBox.Desktop.Hooks;

internal class MouseHook : IDisposable
{
    public event Action<MOUSEINPUT> LeftButtonDown;

    public event Action<MOUSEINPUT> LeftButtonUp;

    public event Action<MOUSEINPUT> RightButtonDown;

    public event Action<MOUSEINPUT> RightButtonUp;

    public event Action<MOUSEINPUT> MiddleButtonDown;

    public event Action<MOUSEINPUT> MiddleButtonUp;

    public event Action<MOUSEINPUT> MouseMove;

    public event Action<MOUSEINPUT, short> MouseWheel;

    private UnhookWindowsHookExSafeHandle _handle = new(IntPtr.Zero);
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
            var mouseInput = (MOUSEINPUT)Marshal.PtrToStructure(lParam, typeof(MOUSEINPUT));
            switch (mouseEvent)
            {
                case PInvoke.WM_LBUTTONDOWN:
                    LeftButtonDown?.Invoke(mouseInput);
                    break;

                case PInvoke.WM_LBUTTONUP:
                    LeftButtonUp?.Invoke(mouseInput);
                    break;

                case PInvoke.WM_RBUTTONDOWN:
                    RightButtonDown?.Invoke(mouseInput);
                    break;

                case PInvoke.WM_RBUTTONUP:
                    RightButtonUp?.Invoke(mouseInput);
                    break;

                case PInvoke.WM_MBUTTONDOWN:
                    MiddleButtonDown?.Invoke(mouseInput);
                    break;

                case PInvoke.WM_MBUTTONUP:
                    MiddleButtonUp?.Invoke(mouseInput);
                    break;

                case PInvoke.WM_MOUSEMOVE:
                    MouseMove?.Invoke(mouseInput);
                    break;

                case PInvoke.WM_MOUSEWHEEL:
                    short delta = (short)(mouseInput.mouseData >> 16);
                    MouseWheel?.Invoke(mouseInput, delta);
                    break;

                default:
                    break;
            }
        }

        LRESULT result = PInvoke.CallNextHookEx(null, nCode, wParam, lParam);
        return result;
    }
}
