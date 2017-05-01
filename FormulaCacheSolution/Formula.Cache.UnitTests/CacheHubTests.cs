using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formula.Cache.CachePlugins.MemoryObject;
using System.Diagnostics;

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

			CacheConfig config = CacheConfig.CreatedFromFile();
			CacheHubConfig hubConfig = CacheHubConfig.CreateFromFile();

			config.CacheList.AddFirst(memoryCache);
			
			//options.CacheHub = new InMemoryHub();

			SuperCache cache = new SuperCache(config);

			CacheHub hub = new CacheHub(hubConfig);


			hub.MessageReceived += ((source, args) => Debug.WriteLine("Message Received"));

			CacheMessage message = new CacheMessage() { Command = CacheMessageCommands.Invalidate, Data="CacheKey that changed"};

			// Simulate some data changing
			hub.PublishMessage(message);
				
		}
	}
}
