using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ModernWpf.Controls;

namespace YukiBox.Desktop.Contracts.Services
{
    public interface INavigationService
    {
        String CurrentPageKey { get; }

        void Initialize(Frame shellFrame);

        void NavigateTo(String pageKey);

        void NavigateTo(String pageKey, Object parameter);
    }
}
