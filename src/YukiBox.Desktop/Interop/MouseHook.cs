using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using static PInvoke.Kernel32;
using static PInvoke.User32;

namespace YukiBox.Desktop.Interop
{
    public class MouseHook : IDisposable
    {
        private Boolean _disposed;

        private WindowsHookDelegate _hookDelegate;
        private SafeHookHandle _handle = SafeHookHandle.Null;

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
            this._hookDelegate = new WindowsHookDelegate(MouseProc);
            this._handle = SetHook(this._hookDelegate);
        }

        public void Uninstall()
        {
            if (!this._handle.IsClosed && !this._handle.IsInvalid)
            {
                this._handle.Dispose();
            }
        }

        private SafeHookHandle SetHook(WindowsHookDelegate proc)
        {
            using var module = Process.GetCurrentProcess().MainModule;
            var hMod = GetModuleHandle(module.ModuleName);
            return SetWindowsHookEx(WindowsHookType.WH_MOUSE_LL, proc, hMod, 0);
        }

        private Int32 MouseProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var mouseEvent = (WindowMessage)wParam;
                var mouseInput = (MOUSEINPUT)Marshal.PtrToStructure(lParam, typeof(MOUSEINPUT));
                switch (mouseEvent)
                {
                    case WindowMessage.WM_LBUTTONDOWN:
                        LeftButtonDown?.Invoke(mouseInput);
                        break;

                    case WindowMessage.WM_LBUTTONUP:
                        LeftButtonUp?.Invoke(mouseInput);
                        break;

                    case WindowMessage.WM_RBUTTONDOWN:
                        RightButtonDown?.Invoke(mouseInput);
                        break;

                    case WindowMessage.WM_RBUTTONUP:
                        RightButtonUp?.Invoke(mouseInput);
                        break;

                    case WindowMessage.WM_MBUTTONDOWN:
                        MiddleButtonDown?.Invoke(mouseInput);
                        break;

                    case WindowMessage.WM_MBUTTONUP:
                        MiddleButtonUp?.Invoke(mouseInput);
                        break;

                    case WindowMessage.WM_MOUSEMOVE:
                        MouseMove?.Invoke(mouseInput);
                        break;

                    case WindowMessage.WM_MOUSEWHEEL:
                        var delta = (Int16)(mouseInput.mouseData >> 16);
                        MouseWheel?.Invoke(mouseInput, delta);
                        break;

                    default:
                        break;
                }
            }

            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
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