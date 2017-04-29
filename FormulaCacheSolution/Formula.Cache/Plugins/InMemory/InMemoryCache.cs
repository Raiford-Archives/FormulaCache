using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formula.Cache.Plugins.InMemory
{
	/// <summary>
	/// In Memory Cache used for testing or very simple in process uses. The data in the cache is not persistent and only available in 
	/// the single process.
	/// </summary>
	public class InMemoryCache : ICache
	{
		private Dictionary<string, object> _cache = new Dictionary<string, object>();
		

		public void Add(Guid key, object value)
		{
			Add(key.ToString(), value);
		}

		public void Add(string key, object value)
		{
			_cache.Add(key, value);
		}

		public void Add<T>(Guid key, T value)
		{
			Add(key.ToString(), value);
		}

		public void CleanupExpired()
		{
			throw new NotImplementedException();
		}

		public object Get(Guid key)
		{
			return Get(key.ToString());
		}

		public object Get(string key)
		{
			object value = null;

			if(!_cache.TryGetValue(key, out value))
			{
				return null;
			}

			return value;
		}

		public T Get<T>(string key)
		{
			return (T) Get(key);
		}

		public T Get<T>(Guid key)
		{
			return (T)Get(key);
		}

		public void Remove(Guid key)
		{
			_cache.Remove(key.ToString());
		}

		public void Remove(string key)
		{
			_cache.Remove(key);
		}
	}
}
