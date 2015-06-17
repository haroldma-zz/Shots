using System.IO;
using System.Threading.Tasks;
using Shots.Core.Helpers;

namespace Shots.Core.Utilities.Interfaces
{
    public interface IStorageUtility
    {
        Task<byte[]> ReadBytesAsync(string path, bool ifExists = false,
            StorageHelper.StorageStrategy location = StorageHelper.StorageStrategy.Local);

        Task<string> ReadStringAsync(string path, bool ifExists = false,
            StorageHelper.StorageStrategy location = StorageHelper.StorageStrategy.Local);

        Task<Stream> ReadStreamAsync(string path, bool ifExists = false,
            StorageHelper.StorageStrategy location = StorageHelper.StorageStrategy.Local);

        Task WriteBytesAsync(string path, byte[] bytes,
            StorageHelper.StorageStrategy location = StorageHelper.StorageStrategy.Local);

        Task WriteStringAsync(string path, string text,
           StorageHelper.StorageStrategy location = StorageHelper.StorageStrategy.Local);

        Task WriteStreamAsync(string path, Stream stream,
            StorageHelper.StorageStrategy location = StorageHelper.StorageStrategy.Local);

        Task<bool> DeleteAsync(string path,
            StorageHelper.StorageStrategy location = StorageHelper.StorageStrategy.Local);

        Task<bool> ExistsAsync(string path,
            StorageHelper.StorageStrategy location = StorageHelper.StorageStrategy.Local);
    }
}