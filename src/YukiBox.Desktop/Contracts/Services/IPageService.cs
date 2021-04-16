using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ModernWpf.Controls;

using YukiBox.Desktop.ViewModels;

namespace YukiBox.Desktop.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(String key);

        Page GetPage(String key);

        void Configure<VM, V>()
            where VM : ViewModelBase
            where V : Page;
    }
}
