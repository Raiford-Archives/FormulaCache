using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formula.Cache
{
	public interface ICacheNotifications
	{
		//void OnItemInvalidated(object args);
		//void OnItemExpired(object args);

		/// <summary>
		/// Outgoing - Called by the data components when something changes. This will broadcast the change to all the other caches.
		/// Implementation can be plugged into the hub as required.
		/// </summary>
		/// <param name="key"></param>
		void BroadcastInvalidation(string key);

		/// <summary>
		/// Incoming - Called when an external system publishes a invaidation message and is received by the hub.
		/// The hub will generally iterate all the registered caches to invalidate this value.
		/// </summary>
		/// <param name="key"></param>
		void ReceiveInvalidation(string key);



		void RegisterCache(ICache cache);

		void UnregisterCache(ICache cache);




	}
}
