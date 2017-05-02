using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formula.Core.UnitTesting;
using Formula.Cache.Plugins.CachePlugins.MemoryObject;
using System.Runtime.Caching;

namespace Formula.Cache.UnitTests.MemoryObject
{
	[TestClass]
	public class MemoryObjectTests : CachePluginTestsBase
	{
		[TestMethod]
		[TestCategory(TestCategories.UnitTest)]
		public void AddGetRemoveGet_Success()
		{
			AddGetRemoveGet_Success(new MemoryObjectCache());

		}





		[TestMethod]
		[TestCategory(TestCategories.Development)]
		public void CacheEviction_Success()
		{
			MemoryObjectCache cache = new MemoryObjectCache();

			string key = "key";
			string value = "value";

			ObjectCache ocache = MemoryCache.Default;
			CacheItem item = new CacheItem(key)
			{
				
				RegionName = "",
				Value = value
			};
		
			//TODO - work this into the design
			CacheItemPolicy policy = new CacheItemPolicy();
			//policy.






		}

	}
}
