using Formula.Core.Configuration;
using Formula.Core.Encryption;
using Formula.Core.Security;
using Formula.Core.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formula.Cache.Plugins.FileSystem
{
	/// <summary>
	/// Uses the FileSystem as a local cache by creating a Hash of the key and writing to the file system. This format is 
	/// similar to what Git does by hashing the file and creating a sub folder containing the first few characters of the hash
	/// for a fast lookup and so you dont reach the file system limits on file counts.
	/// </summary>
	public class FileSystemCache : ICache
	{
		#region Fields
		private string _rootPath { get; set; }
		private string _cacheFileNameExtension { get; set; }
		#endregion

		#region Constructor
		public FileSystemCache()
		{
			_rootPath = Settings.Get<string>("CacheFileSystemRootPath");
			_cacheFileNameExtension = Settings.GetWithDefault<string>("CacheFileNameExtension", "cachebin");
		}
		#endregion

		#region Private Helpers
		private string BuildKeyHash(string key)
		{
			return Md5Hasher.Hash(key);		
		}
		private string GetKeyHashPrefix(string keyHash)
		{
			return keyHash.Substring(0, 3);
		}
			
		private string GetCacheFileName(string key)
		{
			string keyHash = BuildKeyHash(key);
			string rootPath = _rootPath;
			string prefixFolderName = GetKeyHashPrefix(keyHash);
			string path = Path.Combine(rootPath, prefixFolderName);
			string fullFileName = Path.Combine(path, keyHash) + "." + _cacheFileNameExtension;
			return fullFileName;
		}
		#endregion

		#region Public Methods
		public void Add<T>(string key, T value)
		{
			byte[] bytes = BinarySerializer.ToByteArray(value);
			string fullFileName = GetCacheFileName(key);
			DirectoryInfo di = Directory.CreateDirectory(Path.GetDirectoryName(fullFileName));
			File.WriteAllBytes(fullFileName, bytes);
		}
		public T Get<T>(string key)
		{
			string fileName = GetCacheFileName(key);

			if (File.Exists(fileName))
			{
				byte[] bytes = File.ReadAllBytes(fileName);
				object obj = BinarySerializer.ToObject(bytes);
				T result = (T)obj;
				return result;
			}
			return default(T);
		}
	
		public void Remove(string key)
		{
			string fullFileName = GetCacheFileName(key);
			File.Delete(fullFileName);
		}

		public void CleanupExpired()
		{
			throw new NotImplementedException();
		}
		#endregion

	}
}
