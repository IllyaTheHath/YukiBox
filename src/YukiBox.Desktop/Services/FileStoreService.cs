using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YukiBox.Desktop.Contracts.Services;

namespace YukiBox.Desktop.Services
{
    public class FileStoreService : IFileStoreService
    {
        public Byte[] Read(String folderPath, String fileName)
        {
            var path = Path.Combine(folderPath, fileName);
            if (File.Exists(path))
            {
                using FileStream fs = new(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var bytes = new Byte[fs.Length];
                fs.Read(bytes);
                return bytes;
            }

            return null;
        }

        public async Task<Byte[]> ReadAsync(String folderPath, String fileName)
        {
            var path = Path.Combine(folderPath, fileName);
            if (File.Exists(path))
            {
                using FileStream fs = new(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var bytes = new Byte[fs.Length];
                await fs.ReadAsync(bytes);
                return bytes;
            }

            return null;
        }

        public void Save(String folderPath, String fileName, Byte[] content)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var path = Path.Combine(folderPath, fileName);
            using FileStream fs = new(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            fs.Write(content);
        }

        public async Task SaveAsync(String folderPath, String fileName, Byte[] content)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var path = Path.Combine(folderPath, fileName);
            using FileStream fs = new(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            await fs.WriteAsync(content);
        }

        public void DeleteFile(String folderPath, String fileName)
        {
            var path = Path.Combine(folderPath, fileName);
            if (File.Exists(path))
            {
                File.Delete(Path.Combine(path));
            }
        }

        public void DeleteFolder(String folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath);
            }
        }
    }
}