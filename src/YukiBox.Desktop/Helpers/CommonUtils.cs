using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YukiBox.Desktop.Helpers
{
    public static class CommonUtils
    {
        public static Uri GetAbsoluteUri(String path)
        {
            return new Uri($"pack://application:,,,/YukiBox;component/{path}");
        }
    }
}
