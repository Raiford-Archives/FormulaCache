using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formula.Core.UnitTesting.Data;
using Formula.Core.UnitTesting;
using Formula.Cache.CachePlugins.InMemory;
using Formula.Cache.CachePlugins.FileSystem;
using Formula.Cache.NotificationPlugins;

namespace Formula.Cache.UnitTests.InMemory
{
	[TestClass]
	public class InMemoryCacheTests : CachePluginTestsBase
	{
		[TestMethod]
		[TestCategory(TestCategories.UnitTest)]
		public void AddGetRemoveGet_Success()
		{
			AddGetRemoveGet_Success(new InMemoryCache());

		}
	}
}
