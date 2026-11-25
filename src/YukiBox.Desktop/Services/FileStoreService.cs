using System;
using System.IO;
using System.Threading.Tasks;

using YukiBox.Desktop.Contracts.Services;

namespace YukiBox.Desktop.Services
{
    public class FileStoreService : IFileStoreService
    {
        public byte[] Read(string folderPath, string fileName)
        {
            var path = Path.Combine(folderPath, fileName);
            if (File.Exists(path))
            {
                using FileStream fs = new(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var bytes = new byte[fs.Length];
                fs.ReadExactly(bytes);
                return bytes;
            }

            return null;
        }

        public async Task<byte[]> ReadAsync(string folderPath, string fileName)
        {
            var path = Path.Combine(folderPath, fileName);
            if (File.Exists(path))
            {
                using FileStream fs = new(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var bytes = new byte[fs.Length];
                await fs.ReadExactlyAsync(bytes);
                return bytes;
            }

            return null;
        }

        public void Save(string folderPath, string fileName, byte[] content)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var path = Path.Combine(folderPath, fileName);
            using FileStream fs = new(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            fs.Write(content);
        }

        public async Task SaveAsync(string folderPath, string fileName, byte[] content)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var path = Path.Combine(folderPath, fileName);
            using FileStream fs = new(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            await fs.WriteAsync(content);
        }

        public void DeleteFile(string folderPath, string fileName)
        {
            var path = Path.Combine(folderPath, fileName);
            if (File.Exists(path))
            {
                File.Delete(Path.Combine(path));
            }
        }

        public void DeleteFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath);
            }
        }
    }
}