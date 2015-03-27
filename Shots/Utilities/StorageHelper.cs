using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Shots.Utilities
{
    // based on http://codepaste.net/gtu5mq
    public static class StorageHelper
    {
        #region Private Methods

        private static StorageFolder GetFolderFromStrategy(StorageStrategy location)
        {
            switch (location)
            {
                case StorageStrategy.Roaming:
                    return ApplicationData.Current.RoamingFolder;
                case StorageStrategy.Temporary:
                    return ApplicationData.Current.TemporaryFolder;
                case StorageStrategy.Installation:
                    return Windows.ApplicationModel.Package.Current.InstalledLocation;

                default:
                    return ApplicationData.Current.LocalFolder;
            }
        }

        public static async Task<StorageFile> GetIfFileExistsAsync(string path, StorageStrategy strategy = StorageStrategy.Local)
        {
            return await GetIfFileExistsAsync(path, GetFolderFromStrategy(strategy)).ConfigureAwait(false);
        }
        public static async Task<StorageFile> GetIfFileExistsAsync(string path, StorageFolder folder)
        {
            var parts = path.Split('/');

            var fileName = parts.Last();

            if (parts.Length > 1)
            {
                folder =
                    await GetFolderAsync(path.Substring(0, path.Length - fileName.Length), folder).ConfigureAwait(false);
            }

            if (folder == null)
            {
                return null;
            }
            return await folder.TryGetItemAsync(fileName).ConfigureAwait(false) as StorageFile;
        }

        private static async Task<StorageFolder> _EnsureFolderExistsAsync(string name, StorageFolder parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            return
                await
                    parent.CreateFolderAsync(name, CreationCollisionOption.OpenIfExists).AsTask().ConfigureAwait(false);
        }

        #endregion

        #region Public Methods

        public static async Task<bool> FileExistsAsync(string path, StorageStrategy location = StorageStrategy.Local)
        {
            return await FileExistsAsync(path, GetFolderFromStrategy(location)).ConfigureAwait(false);
        }

        public static async Task<bool> FileExistsAsync(string path, StorageFolder folder)
        {
            return (await GetIfFileExistsAsync(path, folder).ConfigureAwait(false)) != null;
        }

        public static async Task<StorageFolder> EnsureFolderExistsAsync(string path,
            StorageStrategy location = StorageStrategy.Local)
        {
            return await EnsureFolderExistsAsync(path, GetFolderFromStrategy(location)).ConfigureAwait(false);
        }

        public static async Task<StorageFolder> EnsureFolderExistsAsync(string path, StorageFolder parentFolder)
        {
            var parent = parentFolder;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var name in path.Trim('/').Split('/'))
            {
                parent = await _EnsureFolderExistsAsync(name, parent).ConfigureAwait(false);
            }

            return parent; // now points to innermost folder
        }

        public static async Task<bool> DeleteFileAsync(string path, StorageStrategy location = StorageStrategy.Local)
        {
            return await DeleteFileAsync(path, GetFolderFromStrategy(location));
        }

        public static async Task<bool> DeleteFileAsync(string path, StorageFolder folder)
        {
            var file = await GetIfFileExistsAsync(path, folder).ConfigureAwait(false);

            if (file != null)
                await file.DeleteAsync();

            return !(await FileExistsAsync(path, folder).ConfigureAwait(false));
        }

        public static async Task<StorageFolder> GetFolderAsync(string path,
            StorageStrategy location = StorageStrategy.Local)
        {
            return await GetFolderAsync(path, GetFolderFromStrategy(location)).ConfigureAwait(false);
        }

        public static async Task<StorageFolder> GetFolderAsync(string path, StorageFolder parentFolder)
        {
            var parent = parentFolder;

            foreach (var name in path.Trim('/').Split('/'))
            {
                parent = await _GetFolderAsync(name, parent).ConfigureAwait(false);

                if (parent == null) return null;
            }

            return parent; // now points to innermost folder
        }

        private static async Task<StorageFolder> _GetFolderAsync(string name, StorageFolder parent)
        {
            var item = await parent.TryGetItemAsync(name).ConfigureAwait(false);
            if (item is StorageFolder)
            {
                return item as StorageFolder;
            }
            else
            {
                return null;
            }
        }

        public static async Task<BinaryReader> GetReaderForFileAsync(string path,
            StorageStrategy location = StorageStrategy.Local)
        {
            return await GetReaderForFileAsync(path, GetFolderFromStrategy(location)).ConfigureAwait(false);
        }

        public static async Task<BinaryReader> GetReaderForFileAsync(string path, StorageFolder folder)
        {
            var file = await CreateFileAsync(path, folder).ConfigureAwait(false);

            var stream = await file.OpenStreamForReadAsync().ConfigureAwait(false);

            return new BinaryReader(stream);
        }

        public static async Task<BinaryWriter> GetWriterForFileAsync(string path,
            StorageStrategy location = StorageStrategy.Local)
        {
            return await GetWriterForFileAsync(path, GetFolderFromStrategy(location)).ConfigureAwait(false);
        }

        public static async Task<BinaryWriter> GetWriterForFileAsync(string path, StorageFolder folder)
        {
            var file = await CreateFileAsync(path, folder).ConfigureAwait(false);

            var stream = await file.OpenStreamForWriteAsync().ConfigureAwait(false);

            return new BinaryWriter(stream);
        }

        public static async Task<StorageFile> CreateFileAsync(string path,
            StorageStrategy location = StorageStrategy.Local,
            CreationCollisionOption option = CreationCollisionOption.OpenIfExists)
        {
            return await CreateFileAsync(path, GetFolderFromStrategy(location), option);
        }

        public static async Task<StorageFile> CreateFileAsync(string path, StorageFolder folder,
            CreationCollisionOption option = CreationCollisionOption.OpenIfExists)
        {
            if (path.StartsWith("/") || path.StartsWith("\\"))
                path = path.Substring(1);
            var parts = path.Split('/');

            var fileName = parts.Last();

            if (parts.Length > 1)
            {
                folder =
                    await
                        EnsureFolderExistsAsync(path.Substring(0, path.Length - fileName.Length), folder)
                            .ConfigureAwait(false);
            }

            return await folder.CreateFileAsync(fileName, option).AsTask().ConfigureAwait(false);
        }

        public static async Task<StorageFile> GetFileAsync(string path,
            StorageStrategy location = StorageStrategy.Local)
        {
            return await CreateFileAsync(path, GetFolderFromStrategy(location));
        }

        public static async Task<StorageFile> GetFileAsync(string path,
            StorageFolder folder)
        {
            return await CreateFileAsync(path, folder);
        }

        #endregion

        #region Nested types

        public enum StorageStrategy
        {
            /// <summary>Local, isolated folder</summary>
            Local,

            /// <summary>Cloud, isolated folder. 100k cumulative limit.</summary>
            Roaming,

            /// <summary>Local, temporary folder (not for settings)</summary>
            Temporary,

            /// <summary>Local, app install folder (read-only)</summary>
            Installation
        }

        #endregion
    }
}