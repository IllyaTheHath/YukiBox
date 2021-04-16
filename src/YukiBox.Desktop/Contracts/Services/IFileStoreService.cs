using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YukiBox.Desktop.Contracts.Services
{
    public interface IFileStoreService
    {
        Task<Byte[]> ReadAsync(String folderPath, String fileName);

        Task SaveAsync(String folderPath, String fileName, Byte[] content);

        void DeleteFile(String folderPath, String fileName);

        void DeleteFolder(String folderPath);
    }
}
