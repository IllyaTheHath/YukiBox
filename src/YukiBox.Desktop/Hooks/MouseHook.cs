using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using Windows.Win32.UI.WindowsAndMessaging;

namespace YukiBox.Desktop.Hooks
{
    internal class MouseHook : IDisposable
    {
        private Boolean _disposed;

        private UnhookWindowsHookExSafeHandle _handle = new UnhookWindowsHookExSafeHandle(IntPtr.Zero);
        private HOOKPROC _hookProc;

        public delegate void MouseHookCallback(MOUSEINPUT input);

        public delegate void MouseHookWheelCallback(MOUSEINPUT input, Int16 delta);

        #region Events

        public event MouseHookCallback LeftButtonDown;

        public event MouseHookCallback LeftButtonUp;

        public event MouseHookCallback RightButtonDown;

        public event MouseHookCallback RightButtonUp;

        public event MouseHookCallback MiddleButtonDown;

        public event MouseHookCallback MiddleButtonUp;

        public event MouseHookCallback MouseMove;

        public event MouseHookWheelCallback MouseWheel;

        #endregion Events

        public void Install()
        {
            this._hookProc = MouseProc;
            this._handle = SetHook(this._hookProc);
        }

        public void Uninstall()
        {
            if (!this._handle.IsClosed && !this._handle.IsInvalid)
            {
                this._handle.Dispose();
            }
        }

        private UnhookWindowsHookExSafeHandle SetHook(HOOKPROC proc)
        {
            using var module = Process.GetCurrentProcess().MainModule;
            var hMod = PInvoke.GetModuleHandle(module.ModuleName);
            return PInvoke.SetWindowsHookEx(WINDOWS_HOOK_ID.WH_MOUSE_LL, proc, hMod, 0);
        }

        private LRESULT MouseProc(Int32 nCode, WPARAM wParam, LPARAM lParam)
        {
            if (nCode < 0)
            {
                return PInvoke.CallNextHookEx(null, nCode, wParam, lParam);
            }

            if (nCode == PInvoke.HC_ACTION)
            {
                var mouseEvent = (UInt32)wParam;
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
                        var delta = (Int16)(mouseInput.mouseData >> 16);
                        MouseWheel?.Invoke(mouseInput, delta);
                        break;

                    default:
                        break;
                }
            }

            return PInvoke.CallNextHookEx(null, nCode, wParam, lParam);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(Boolean disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    Uninstall();
                }

                this._disposed = true;
            }
        }

        ~MouseHook()
        {
            Dispose(false);
        }
    }
}