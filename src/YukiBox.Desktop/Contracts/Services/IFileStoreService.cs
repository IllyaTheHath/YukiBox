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
        Byte[] Read(String folderPath, String fileName);

        Task<Byte[]> ReadAsync(String folderPath, String fileName);

        void Save(String folderPath, String fileName, Byte[] content);

        Task SaveAsync(String folderPath, String fileName, Byte[] content);

        void DeleteFile(String folderPath, String fileName);

        void DeleteFolder(String folderPath);
    }
}