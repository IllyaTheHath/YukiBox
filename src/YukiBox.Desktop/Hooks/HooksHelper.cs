using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YukiBox.Desktop.Interop;

namespace YukiBox.Desktop.Hooks
{
    public class HooksHelper
    {
        private static readonly Lazy<HooksHelper> lazy = new(() => new HooksHelper());

        public static HooksHelper Instance => lazy.Value;

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
            MouseHook.Dispose();
        }
    }
}