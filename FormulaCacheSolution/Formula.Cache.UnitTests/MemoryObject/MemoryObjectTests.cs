using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formula.Core.UnitTesting;
using Formula.Core.UnitTesting.Data;
using Formula.Cache.CachePlugins.MemoryObject;

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

	}
}
