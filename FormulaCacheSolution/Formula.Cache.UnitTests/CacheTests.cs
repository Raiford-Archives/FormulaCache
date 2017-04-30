using Formula.Cache.CachePlugins.FileSystem;
using Formula.Cache.CachePlugins.InMemory;
using Formula.Cache.NotificationPlugins;
using Formula.Core.UnitTesting;
using Formula.Core.UnitTesting.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Formula.Cache.UnitTests
{
	[TestClass]
	public class CacheTests
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
			options.CacheList.AddFirst(new FileSystemCache());
			options.CacheList.AddFirst(new InMemoryCache());
			

			// Create the Cache
			SuperCache cache = new SuperCache(options);


			// Add some item and ready to GO!
			IList<Customer> customers = TestData.GetCustomerList(1000);
			cache.Add<IList<Customer>>("list", customers);


			IList<Customer> customers2 = cache.Get<IList<Customer>>("list");


			CacheStatistics stats = cache.GetStatistics();





		}
	}
}
