using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formula.Cache
{
	public class CacheNotificationHub : ICacheNotifications
	{
		private List<ICache> _caches = new List<ICache>();

		public void BroadcastInvalidation(string key)
		{
			// Will send a message to a network queue that serves as the hub... perhaps a message queue or PubSub topic
		}

		public void ReceiveInvalidation(string key)
		{
			foreach (var cache in _caches)
			{
				cache.Remove(key);
			}
		}

		public void RegisterCache(ICache cache)
		{
			_caches.Add(cache);
		}

		public void UnregisterCache(ICache cache)
		{
			// Need a way to search for a specific cache.. .perhaps a cache guid... not sure yet.
		}
	}
}
