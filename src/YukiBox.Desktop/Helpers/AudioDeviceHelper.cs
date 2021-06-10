using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using NAudio.CoreAudioApi;

using PInvoke;

using static PInvoke.User32;

namespace YukiBox.Desktop.Helpers
{
    public static class AudioDeviceHelper
    {
        public static Int32 GetSystemVolume()
        {
            Int32 volume = -1;
            var enumerator = new MMDeviceEnumerator();

            var speakDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            if (speakDevices.Count > 0)
            {
                var device = speakDevices.FirstOrDefault();
                if (device != null)
                {
                    volume = (Int32)(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
                }
            }
            return volume;
        }

        public static void StepSystemVolume(Boolean up, Boolean useWin32Input = true)
        {
            if (useWin32Input)
            {
                StepSystemVolumeWin32Input(up);
            }
            else
            {
                StepSystemVolumeNAudio(up);
            }
        }

        private static void StepSystemVolumeNAudio(Boolean up)
        {
            var enumerator = new MMDeviceEnumerator();

            var speakDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            if (speakDevices.Count > 0)
            {
                var device = speakDevices.FirstOrDefault();
                if (device != null)
                {
                    if (up)
                    {
                        device.AudioEndpointVolume.VolumeStepUp();
                    }
                    else
                    {
                        device.AudioEndpointVolume.VolumeStepDown();
                    }
                }
            }
        }

        private static void StepSystemVolumeWin32Input(Boolean up)
        {
            var kInputs = new INPUT[2];

            var kDown = new INPUT();
            kDown.type = InputType.INPUT_KEYBOARD;
            kDown.Inputs.ki = new KEYBDINPUT()
            {
                wVk = up ? VirtualKey.VK_VOLUME_UP : VirtualKey.VK_VOLUME_DOWN
            };

            var kUp = new INPUT();
            kUp.type = InputType.INPUT_KEYBOARD;
            kUp.Inputs.ki = new KEYBDINPUT()
            {
                wVk = up ? VirtualKey.VK_VOLUME_UP : VirtualKey.VK_VOLUME_DOWN,
                dwFlags = KEYEVENTF.KEYEVENTF_KEYUP
            };

            kInputs[0] = kDown;
            kInputs[1] = kUp;

            SendInput(kInputs.Length, kInputs, Marshal.SizeOf(typeof(INPUT)));
        }
    }
}