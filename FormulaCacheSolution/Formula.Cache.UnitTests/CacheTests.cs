using Formula.Cache.Configuration;
using Formula.Cache.Plugins.CachePlugins.FileSystem;
using Formula.Cache.Plugins.CachePlugins.InMemory;
using Formula.Cache.Plugins.CachePlugins.MemoryObject;
using Formula.Cache.Plugins.HubPlugins;
using Formula.Core.LoggersDiagnostics;
using Formula.Core.UnitTesting;
using Formula.Core.UnitTesting.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Formula.Cache.UnitTests
{
	[TestClass]
	public class CacheTests : CachePluginTestsBase
	{
		[TestMethod]
		[TestCategory(TestCategories.UnitTest)]
		public void SuperCache_Success()
		{
			// Configure Cache Options
			CacheConfig config = new CacheConfig
			{
			//	CacheHub = new InMemoryHub(),
				EnableStatistics = true,
				EnableTracing = true,
				
			};
			config.CachePlugins.Add(new MemoryObjectCache());
			config.CachePlugins.Add(new FileSystemCache());
			config.CachePlugins.Add(new InMemoryCache());
			

			// Create the Cache
			MultiCache cache = new MultiCache(config);


			// Add some item and ready to GO!
			//IList<Customer> customers = TestData.GetCustomerList(1000);
			Customer customer = TestData.CreateCustomer();
			cache.Add<Customer>(customer.Id.ToString(), customer);


			Customer customers2 = cache.Get<Customer>(customer.Id.ToString());


			CacheStatistics stats = cache.Statistics;
			
		}

		[TestMethod]
		[TestCategory(TestCategories.UnitTest)]
		public void ChainCacheGet_Success()
		{
			Customer customer = TestData.CreateCustomer();
			// Create the 2 Caches
			ICache memoryCache = new MemoryObjectCache();
			ICache localCache = new InMemoryCache();
			ICache remoteCache = new FileSystemCache();

			// Add item to remoteCache
			remoteCache.Add<Customer>(customer.Id.ToString(), customer);


			// IChainedCache
			// Hookup the cache Chain
			CacheConfig config = new CacheConfig();
			//config.CacheHub = new InMemoryHub();
			config.CachePlugins.Add(memoryCache);
			config.CachePlugins.Add(remoteCache);
			config.CachePlugins.Add(localCache);

			MultiCache cache = new MultiCache(config);

			// This should have 1 miss (debug it to ensure)
			Customer cachedCustomer = (Customer)cache.Get<Customer>(customer.Id.ToString());

			// This should have 0 miss (debug it to ensure) since it was added in the last Get()
			cachedCustomer = (Customer)cache.Get<Customer>(customer.Id.ToString());

			// Assert
			Assert.IsNotNull(cachedCustomer);

		}



		[TestMethod]
		[TestCategory(TestCategories.UnitTest)]
		public void ChainCacheGetAddGet_Success()
		{
			Customer customer = TestData.CreateCustomer();
			ICache localCache = new InMemoryCache();
			ICache memoryCache = new MemoryObjectCache();
			ICache remoteCache = new FileSystemCache();


			CacheConfig config = new CacheConfig();
			config.CachePlugins.Add(memoryCache);
			config.CachePlugins.Add(remoteCache);
			config.CachePlugins.Add(localCache);

			// Link the cache Chain
			MultiCache chainedCache = new MultiCache(config);


			// This should have no cached customer
			Customer cachedCustomer = (Customer)chainedCache.Get<Customer>(customer.Id.ToString());
			Assert.IsNull(cachedCustomer);

			// Now we will add it to the caches
			bool pushToOuter = true;
			chainedCache.Add<Customer>(customer.Id.ToString(), customer, pushToOuter);

			// This should have a customer in the nearest cache
			cachedCustomer = (Customer)chainedCache.Get<Customer>(customer.Id.ToString());
			Assert.IsNotNull(cachedCustomer);


		}

		[TestMethod]
		[TestCategory(TestCategories.UnitTest)]
		public void ChainCacheAddGetRemove_Success()
		{
			Customer customer = TestData.CreateCustomer();
			ICache memoryCache = new MemoryObjectCache();
			ICache localCache = new InMemoryCache();
			ICache remoteCache = new FileSystemCache();

			CacheConfig config = new CacheConfig();
			config.CachePlugins.Add(memoryCache);
			config.CachePlugins.Add(remoteCache);
			config.CachePlugins.Add(localCache);
			//config.CacheHub = new InMemoryHub();

			// Link the cache Chain
			MultiCache chainedCache = new MultiCache(config);

			// Register Caches with notification hub
			ICacheHub hub = new InMemoryHub();
			hub.RegisterCache(remoteCache);
			hub.RegisterCache(localCache);
			hub.RegisterCache(memoryCache);



			// Now we will add it to the caches local to the remotes
			bool pushToOuter = true;
			chainedCache.Add<Customer>(customer.Id.ToString(), customer, pushToOuter);

			Customer cachedCustomer = (Customer)chainedCache.Get<Customer>(customer.Id.ToString());
			Assert.IsNotNull(cachedCustomer);

			// Now remove it and should remove from local to the remotes
			//chainedCache.Remove(customer.Id.ToString());

			// Send notification (This will delete the item)
			hub.PublishMessage(new CacheMessage { Command = CacheMessageCommands.Invalidate, Data = customer.Id.ToString() });

			// Confirm its gone.
			cachedCustomer = (Customer)chainedCache.Get<Customer>(customer.Id.ToString());
			Assert.IsNull(cachedCustomer);

		}



		[TestMethod]
		[TestCategory(TestCategories.UnitTest)]
		public void ChainCacheFullFlow_Success()
		{
			Customer customer = TestData.CreateCustomer();

			// Configure Cache
			ICache memoryCache = new MemoryObjectCache();
			ICache localCache = new InMemoryCache();
			ICache remoteCache = new FileSystemCache();

			CacheConfig config = new CacheConfig();
			config.CachePlugins.Add(remoteCache);
			config.CachePlugins.Add(localCache);
			config.CachePlugins.Add(memoryCache);
			//config.CacheHub = new InMemoryHub();

			MultiCache cache = new MultiCache(config);


			// Now we will add it to the caches local to the remotes
			bool pushToOuter = true;
			cache.Add<Customer>(customer.Id.ToString(), customer, pushToOuter);

			Customer cachedCustomer = (Customer)cache.Get<Customer>(customer.Id.ToString());
			Assert.IsNotNull(cachedCustomer);

			cache.Remove(customer.Id.ToString());

			// Confirm its gone.
			cachedCustomer = (Customer)cache.Get<Customer>(customer.Id.ToString());
			Assert.IsNull(cachedCustomer);

		}


		[TestMethod]
		[TestCategory(TestCategories.Performance)]
		public void AddGetRemoveGet_PerformanceAll()
		{
			int iterations = 10000;

			using (PerfLog perf = PerfLog.StartNew(iterations))
			{
				AddGetRemoveGet_Performance(new MemoryObjectCache(), iterations);
			}


			using (PerfLog perf = PerfLog.StartNew(iterations))
			{
				AddGetRemoveGet_Performance(new InMemoryCache(), iterations);
			}

			using (PerfLog perf = PerfLog.StartNew(iterations))
			{
				AddGetRemoveGet_Performance(new FileSystemCache(), iterations);
			}

		}
	}
}
