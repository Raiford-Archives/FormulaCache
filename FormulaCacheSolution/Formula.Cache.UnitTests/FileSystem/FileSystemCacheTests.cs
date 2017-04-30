using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formula.Core.UnitTesting.Data;
using Formula.Core.UnitTesting;
using KellermanSoftware.CompareNetObjects;
using Formula.Cache.CachePlugins.FileSystem;

namespace Formula.Cache.UnitTests.FileSystem
{
	[TestClass]
	public class FileSystemCacheTests : CachePluginTestsBase
	{
		[TestMethod]
		[TestCategory(TestCategories.UnitTest)]
		public void AddGetRemoveGet_Success()
		{
			AddGetRemoveGet_Success(new FileSystemCache());
			
		}

	}
}
