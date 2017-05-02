using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formula.Core.Serialization;
using System.Diagnostics;
using Formula.Cache.Configuration;
using Formula.Cache.Plugins.CachePlugins.MemoryObject;
using Formula.Cache.Plugins.CachePlugins.FileSystem;
using Formula.Cache.Plugins.CachePlugins.InMemory;

namespace Formula.Cache.UnitTests
{
	[TestClass]
	public class CacheConfigTests
	{
		[TestMethod]
		public void CacheConfigBuildSerialize_Success()
		{

			CacheConfigDefinition config = new CacheConfigDefinition();

			config.Version = "1.0j.0.0.";
			config.Cache.EnableStatistics = true;
			config.Cache.Plugins.CachePlugins.Add(new CachePluginDefinition { Name = "MemoryObjectCache", Class = "MemoryObjectCache", ClassData = "logging=true;internalflags=3;argument1=1;argument2=2"});
			config.Cache.Plugins.CachePlugins.Add(new CachePluginDefinition { Name = "FileSystemCache", Class = "FileSystemCache", ClassData = "" });
			config.Cache.Plugins.CachePlugins.Add(new CachePluginDefinition { Name = "InMemoryCache", Class = "InMemoryCache", ClassData = "" });
			
			string json = JsonSerializer.SerializeObject<CacheConfigDefinition>(config);

			Debug.WriteLine(json);


		}

		[TestMethod]
		public void CacheConfigBuildConfig_Success()
		{

			// Configure Cache
			CacheConfig config = new CacheConfig();
			config.CachePlugins.Add(new MemoryObjectCache());
			config.CachePlugins.Add(new InMemoryCache());
			config.CachePlugins.Add(new FileSystemCache());
			
			// Create Multi Cache based on Configuration
			MultiCache cache = new MultiCache(config);


			//Configure Hub
			CacheHubConfig hubConfig = new CacheHubConfig();

			// Create the Cache Hub
			CacheHub hub = new CacheHub(hubConfig);
			hub.MultiCache = cache;
			hub.MessageReceived += ((source, args) => Debug.WriteLine($"Message Received {args.Message.Command} | {args.Message.Data}"));


			// Add Cache Item and confirm its there
			cache.Add<string>("key", "value");
			string v = cache.Get<string>("key");
			Assert.IsTrue(v == "value");


			// Now have your data repository invalidate the key
			hub.PublishInvalidate("key");

			// Confirm it has been removed
			v = cache.Get<string>("key");
			Assert.IsTrue(string.IsNullOrWhiteSpace(v));
			
		}

		private void Hub_MessageReceived(object source, CacheMessageReceivedArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
