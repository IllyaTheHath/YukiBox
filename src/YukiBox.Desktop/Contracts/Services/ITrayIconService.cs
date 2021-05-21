using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YukiBox.Desktop.Contracts.Services
{
    public interface ITrayIconService : IDisposable
    {
        void Initialize();
    }
}