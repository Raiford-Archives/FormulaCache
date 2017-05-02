using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formula.Core.UnitTesting;
using Formula.Cache.Plugins.CachePlugins.FileSystem;

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
