using System.IO;
using System.Threading.Tasks;
using Shots.Core.Helpers;
using Shots.Core.Utilities.Interfaces;

namespace Shots.Core.Utilities.DesignTime
{
    public class DesignStorageUtility : IStorageUtility
    {
        public Task<byte[]> ReadBytesAsync(string path, bool ifExists = false,
            StorageHelper.StorageStrategy location = StorageHelper.StorageStrategy.Local)
        {
            return Task.FromResult<byte[]>(null);
        }

        public Task<string> ReadStringAsync(string path, bool ifExists = false,
            StorageHelper.StorageStrategy location = StorageHelper.StorageStrategy.Local)
        {
            return Task.FromResult<string>(null);
        }

        public Task<Stream> ReadStreamAsync(string path, bool ifExists = false,
            StorageHelper.StorageStrategy location = StorageHelper.StorageStrategy.Local)
        {
            return Task.FromResult<Stream>(null);
        }

        public Task WriteBytesAsync(string path, byte[] bytes,
            StorageHelper.StorageStrategy location = StorageHelper.StorageStrategy.Local)
        {
            return Task.FromResult(0);
        }

        public Task WriteStringAsync(string path, string text,
            StorageHelper.StorageStrategy location = StorageHelper.StorageStrategy.Local)
        {
            return Task.FromResult(0);
        }

        public Task WriteStreamAsync(string path, Stream stream,
            StorageHelper.StorageStrategy location = StorageHelper.StorageStrategy.Local)
        {
            return Task.FromResult(0);
        }

        public Task<bool> DeleteAsync(string path,
            StorageHelper.StorageStrategy location = StorageHelper.StorageStrategy.Local)
        {
            return Task.FromResult(false);
        }

        public Task<bool> ExistsAsync(string path,
            StorageHelper.StorageStrategy location = StorageHelper.StorageStrategy.Local)
        {
            return Task.FromResult(false);
        }
    }
}