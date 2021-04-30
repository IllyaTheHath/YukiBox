using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.Contracts.Services
{
    public interface IConfigService
    {
        Config CurrentConfig { get; }

        Task Clear();
    }
}