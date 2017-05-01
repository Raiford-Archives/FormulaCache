using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formula.Core.UnitTesting;


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
