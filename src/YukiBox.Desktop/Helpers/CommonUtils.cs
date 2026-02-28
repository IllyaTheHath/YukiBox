using System;

namespace YukiBox.Desktop.Helpers
{
    public static class CommonUtils
    {
        public static WindowsVersion WindowsVersion
        {
            get => Environment.OSVersion.Version.Build >= 22000 ? WindowsVersion.Windows11 : WindowsVersion.Windows10;
        }
    }

    public enum WindowsVersion
    {
        Windows10 = 10,
        Windows11 = 11,
    }
}