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
		
		public void Add<T>(string key, T value)
		{
			_cache.Add(key, value);
		}
			
	
		public T Get<T>(string key)
		{
			object value = null;

			if (!_cache.TryGetValue(key, out value))
			{
				return default(T);
			}

			return (T)value;
		}
		
		public void Remove(string key)
		{
			_cache.Remove(key);
		}


		public Task CleanupExpired()
		{
			throw new NotImplementedException();
		}

		public Task AddAsync<T>(string key, T value)
		{
			return Task.Run(() => Add(key, value));
		}

		public Task<T> GetAsync<T>(string key)
		{			
			return Task.FromResult<T>(Get<T>(key));
		}

		public Task RemoveAsync(string key)
		{
			return Task.Run( () => Remove(key));
		}
	}
}
