using Formula.Core.Configuration;
using Formula.Core.Encryption;
using Formula.Core.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formula.Cache.Plugins.FileSystem
{
	public class FileSystemCache : ICache
	{
		private string _rootPath { get; set; }
		private string _cacheFileNameExtension { get; set; }

		public FileSystemCache()
		{
			_rootPath = Settings.Get<string>("CacheFileSystemRootPath");
			_cacheFileNameExtension = Settings.Get<string>("CacheFileNameExtension");
		}

		private string BuildKeyHash(string key)
		{
			return Md5Hasher.Hash(key);		
		}
		private string GetKeyHashPrefix(string keyHash)
		{
			return keyHash.Substring(0, 3);
		}




		public void Add(Guid key, object value)
		{
			throw new NotImplementedException();
		}

		public void Add(string key, object value)
		{

			string keyHash = BuildKeyHash(key);

			byte[] bytes = BinarySerializer.ToByteArray(value);

			// Get FileSystem Root
			string rootPath = _rootPath;
			string prefixFolderName = GetKeyHashPrefix(keyHash);
			string path = Path.Combine(rootPath, prefixFolderName);
			string fullFileName = Path.Combine(path, keyHash) + "." + _cacheFileNameExtension;


			//System.IO.FileInfo file = new System.IO.FileInfo(path);
			//file.Directory.Create();
			DirectoryInfo di = Directory.CreateDirectory(path);
			File.WriteAllBytes(fullFileName, bytes);
		}

		public void Add<T>(Guid key, T value)
		{
			throw new NotImplementedException();
		}

		public object Get(Guid key)
		{
			throw new NotImplementedException();
		}

		public object Get(string key)
		{
			string keyHash = BuildKeyHash(key);
			string rootPath = _rootPath;
			string prefixFolderName = GetKeyHashPrefix(keyHash);
			string path = Path.Combine(rootPath, prefixFolderName);
			string fullFileName = Path.Combine(path, keyHash) + "." + _cacheFileNameExtension;

			byte[] bytes =  File.ReadAllBytes(fullFileName);

			return BinarySerializer.ToObject(bytes);
		}



	
		public T Get<T>(Guid key)
		{
			throw new NotImplementedException();
		}

		public T Get<T>(string key)
		{
			throw new NotImplementedException();
		}

		public void Remove(Guid key)
		{
			throw new NotImplementedException();
		}

		public void Remove(string key)
		{
			throw new NotImplementedException();
		}

		public void CleanupExpired()
		{
			throw new NotImplementedException();
		}
	}
}
