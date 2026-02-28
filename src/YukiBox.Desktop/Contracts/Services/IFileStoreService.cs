using System;
using System.Threading.Tasks;

namespace YukiBox.Desktop.Contracts.Services
{
    public interface IFileStoreService
    {
        byte[] Read(string folderPath, string fileName);

        Task<byte[]> ReadAsync(string folderPath, string fileName);

        void Save(string folderPath, string fileName, byte[] content);

        Task SaveAsync(string folderPath, string fileName, byte[] content);

        void DeleteFile(string folderPath, string fileName);

        void DeleteFolder(string folderPath);
    }
}