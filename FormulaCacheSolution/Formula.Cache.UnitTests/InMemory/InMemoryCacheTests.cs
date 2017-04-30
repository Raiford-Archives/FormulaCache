using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formula.Core.UnitTesting.Data;
using Formula.Core.UnitTesting;
using Formula.Cache.CachePlugins.InMemory;
using Formula.Cache.CachePlugins.FileSystem;
using Formula.Cache.NotificationPlugins;

namespace Formula.Cache.UnitTests.InMemory
{
	[TestClass]
	public class InMemoryCacheTests
	{
		[TestMethod]
		[TestCategory(TestCategories.UnitTest)]
		public void AddGetRemove_Success()
		{
			// Arrange
			Customer customer = TestData.CreateCustomer();
			ICache cache = new InMemoryCache();

			// Add, Get and Assert
			cache.Add(customer.Id.ToString(), customer);
			Customer customerObject = cache.Get<Customer>(customer.Id.ToString());

			// Assert
			Assert.IsNotNull(customerObject);
			Assert.IsInstanceOfType(customerObject, typeof(Customer));


			// Remove and Assert
			cache.Remove(customer.Id.ToString());
			customerObject = cache.Get<Customer>(customer.Id.ToString());
			Assert.IsNull(customerObject);
		

		}



		[TestMethod]
		[TestCategory(TestCategories.UnitTest)]
		public void ChainCacheGet_Success()
		{
			Customer customer = TestData.CreateCustomer();
			// Create the 2 Caches
			ICache localCache = new InMemoryCache();
			ICache remoteCache = new FileSystemCache();

			// Add item to remoteCache
			remoteCache.Add<Customer>(customer.Id.ToString(), customer);


			// IChainedCache
			// Hookup the cache Chain
			CacheOptions options = new CacheOptions();
			options.CacheList.AddFirst(remoteCache);
			options.CacheList.AddFirst(localCache);
			
			SuperCache cache = new SuperCache();
			
			// This should have 1 miss (debug it to ensure)
			Customer cachedCustomer = (Customer) cache.Get<Customer>(customer.Id.ToString());

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
			ICache remoteCache = new FileSystemCache();


			CacheOptions options = new CacheOptions();
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
			ICache localCache = new InMemoryCache();
			ICache remoteCache = new FileSystemCache();

			CacheOptions options = new CacheOptions();
			options.CacheList.AddFirst(remoteCache);
			options.CacheList.AddFirst(localCache);

			// Link the cache Chain
			SuperCache chainedCache = new SuperCache();
			
			// Register Caches with notification hub
			ICacheHub notifications = new InMemoryHub();
			notifications.RegisterCache(remoteCache);
			notifications.RegisterCache(localCache);



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
			ICache localCache = new InMemoryCache();
			ICache remoteCache = new FileSystemCache();

			CacheOptions options = new CacheOptions();
			options.CacheList.AddFirst(remoteCache);
			options.CacheList.AddFirst(localCache);
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
	}
}
