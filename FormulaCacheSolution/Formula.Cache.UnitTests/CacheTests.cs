using Formula.Cache.CachePlugins.FileSystem;
using Formula.Cache.CachePlugins.InMemory;
using Formula.Cache.CachePlugins.MemoryObject;
using Formula.Cache.NotificationPlugins;
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
			CacheOptions options = new CacheOptions
			{
				CacheHub = new InMemoryHub(),
				EnableStatistics = true,
				EnableTracing = true,
				
			};
			options.CacheList.AddFirst(new MemoryObjectCache());
			options.CacheList.AddFirst(new FileSystemCache());
			options.CacheList.AddFirst(new InMemoryCache());
			

			// Create the Cache
			SuperCache cache = new SuperCache(options);


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
			CacheOptions options = new CacheOptions();
			options.CacheHub = new InMemoryHub();
			options.CacheList.AddFirst(memoryCache);
			options.CacheList.AddFirst(remoteCache);
			options.CacheList.AddFirst(localCache);

			SuperCache cache = new SuperCache(options);

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


			CacheOptions options = new CacheOptions();
			options.CacheList.AddFirst(memoryCache);
			options.CacheList.AddFirst(remoteCache);
			options.CacheList.AddFirst(localCache);

			// Link the cache Chain
			SuperCache chainedCache = new SuperCache(options);


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

			CacheOptions options = new CacheOptions();
			options.CacheList.AddFirst(memoryCache);
			options.CacheList.AddFirst(remoteCache);
			options.CacheList.AddFirst(localCache);
			options.CacheHub = new InMemoryHub();

			// Link the cache Chain
			SuperCache chainedCache = new SuperCache(options);

			// Register Caches with notification hub
			ICacheHub notifications = new InMemoryHub();
			notifications.RegisterCache(remoteCache);
			notifications.RegisterCache(localCache);
			notifications.RegisterCache(memoryCache);



			// Now we will add it to the caches local to the remotes
			bool pushToOuter = true;
			chainedCache.Add<Customer>(customer.Id.ToString(), customer, pushToOuter);

			Customer cachedCustomer = (Customer)chainedCache.Get<Customer>(customer.Id.ToString());
			Assert.IsNotNull(cachedCustomer);

			// Now remove it and should remove from local to the remotes
			//chainedCache.Remove(customer.Id.ToString());

			// Send notification (This will delete the item)
			notifications.BroadcastInvalidation(customer.Id.ToString());

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

			CacheOptions options = new CacheOptions();
			options.CacheList.AddFirst(remoteCache);
			options.CacheList.AddFirst(localCache);
			options.CacheList.AddFirst(memoryCache);
			options.CacheHub = new InMemoryHub();

			SuperCache cache = new SuperCache();


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
			int iterations = 1000;

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
