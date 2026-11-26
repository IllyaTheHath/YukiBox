using System.Runtime.InteropServices;

using NAudio.CoreAudioApi;

using Windows.Win32;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace YukiBox.Desktop.Helpers;

public static class AudioDeviceHelper
{
    public static int GetSystemVolume()
    {
        int volume = -1;
        using var enumerator = new MMDeviceEnumerator();

        var speakDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
        if (speakDevices.Count > 0)
        {
            var device = speakDevices.FirstOrDefault();
            if (device != null)
            {
                volume = (int)(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
            }
        }
        return volume;
    }

    public static void StepSystemVolume(bool up, bool useWin32Input = true)
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

    private static void StepSystemVolumeNAudio(bool up)
    {
        using var enumerator = new MMDeviceEnumerator();

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

    private static void StepSystemVolumeWin32Input(bool up)
    {
        var kInputs = new INPUT[2];

        var kDown = new INPUT();
        kDown.type = INPUT_TYPE.INPUT_KEYBOARD;
        kDown.Anonymous.ki = new KEYBDINPUT()
        {
            wVk = up ? VIRTUAL_KEY.VK_VOLUME_UP : VIRTUAL_KEY.VK_VOLUME_DOWN
        };

        var kUp = new INPUT();
        kUp.type = INPUT_TYPE.INPUT_KEYBOARD;
        kUp.Anonymous.ki = new KEYBDINPUT()
        {
            wVk = up ? VIRTUAL_KEY.VK_VOLUME_UP : VIRTUAL_KEY.VK_VOLUME_DOWN,
            dwFlags = KEYBD_EVENT_FLAGS.KEYEVENTF_KEYUP
        };

        kInputs[0] = kDown;
        kInputs[1] = kUp;
        Span<INPUT> kInputSpan = kInputs;

        PInvoke.SendInput(kInputSpan, Marshal.SizeOf(typeof(INPUT)));
    }

    public static void Mute(bool useWin32Input = true)
    {
        if (useWin32Input)
        {
            MuteWin32();
        }
        else
        {
            MuteNAudio();
        }
    }

    private static void MuteNAudio()
    {
        using var enumerator = new MMDeviceEnumerator();

        var speakDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
        if (speakDevices.Count > 0)
        {
            var device = speakDevices.FirstOrDefault();
            if (device != null)
            {
                device.AudioEndpointVolume.Mute = true;
            }
        }
    }

    private static void MuteWin32()
    {
        var kInputs = new INPUT[2];

        var kDown = new INPUT();
        kDown.type = INPUT_TYPE.INPUT_KEYBOARD;
        kDown.Anonymous.ki = new KEYBDINPUT()
        {
            wVk = VIRTUAL_KEY.VK_VOLUME_MUTE
        };

        var kUp = new INPUT();
        kUp.type = INPUT_TYPE.INPUT_KEYBOARD;
        kUp.Anonymous.ki = new KEYBDINPUT()
        {
            wVk = VIRTUAL_KEY.VK_VOLUME_MUTE,
            dwFlags = KEYBD_EVENT_FLAGS.KEYEVENTF_KEYUP
        };

        kInputs[0] = kDown;
        kInputs[1] = kUp;
        Span<INPUT> kInputSpan = kInputs;

        PInvoke.SendInput(kInputSpan, Marshal.SizeOf(typeof(INPUT)));
    }
}
