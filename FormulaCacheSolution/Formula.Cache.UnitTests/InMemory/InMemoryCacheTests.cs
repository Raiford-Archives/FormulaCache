using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formula.Core.UnitTesting.Data;
using Formula.Cache.Plugins.InMemory;
using Formula.Core.UnitTesting;

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
			object customerObject = cache.Get(customer.Id.ToString());

			// Assert
			Assert.IsNotNull(customerObject);
			Assert.IsInstanceOfType(customerObject, typeof(Customer));


			// Remove and Assert
			cache.Remove(customer.Id.ToString());
			customerObject = cache.Get(customer.Id.ToString());
			Assert.IsNull(customerObject);
		

		}



		[TestMethod]
		[TestCategory(TestCategories.UnitTest)]
		public void ChainCacheGet_Success()
		{
			Customer customer = TestData.CreateCustomer();
			// Create the 2 Caches
			ICache localCache = new InMemoryCache();
			ICache remoteCache = new InMemoryCache();

			// Add item to remoteCache
			remoteCache.Add<Customer>(customer.Id, customer);


			// IChainedCache
			// Hookup the cache Chain
			ChainedCache chainedCache = new ChainedCache();
			chainedCache.LinkFirst(remoteCache);
			chainedCache.LinkFirst(localCache);
			
			// This should have 1 miss (debug it to ensure)
			Customer cachedCustomer = (Customer) chainedCache.Get(customer.Id);

			// This should have 0 miss (debug it to ensure) since it was added in the last Get()
			cachedCustomer = (Customer)chainedCache.Get(customer.Id);

			// Assert
			Assert.IsNotNull(cachedCustomer);
			
		}
	}
}
