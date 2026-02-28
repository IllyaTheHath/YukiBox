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