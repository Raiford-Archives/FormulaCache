using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formula.Core.UnitTesting.Data;
using Formula.Core.UnitTesting;
using Formula.Cache.Plugins.CachePlugins.InMemory;

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
