using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Formula.Cache.Plugins.CachePlugins.MemoryObject;
using Formula.Cache.Configuration;

namespace Formula.Cache.UnitTests
{
	[TestClass]
	public class CacheHubTests
	{
		[TestMethod]
		public void RegsiterBroadcastReceive_Success()
		{
			// Configure Cache
			ICache memoryCache = new MemoryObjectCache();

			CacheConfig config = new CacheConfig();
			CacheHubConfig hubConfig = new CacheHubConfig();

			config.CachePlugins.Add(memoryCache);
			
			//options.CacheHub = new InMemoryHub();

			MultiCache cache = new MultiCache(config);

			CacheHub hub = new CacheHub(hubConfig);


			hub.MessageReceived += ((source, args) => Debug.WriteLine("Message Received"));

			CacheMessage message = new CacheMessage() { Command = CacheMessageCommands.ItemInvalidated, Data="CacheKey that changed"};

			// Simulate some data changing
			hub.PublishMessage(message);
				
		}
	}
}
