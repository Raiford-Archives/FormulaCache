using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;
using Formula.Core.UnitTesting;

namespace Formula.Cache.UnitTests.Redis
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		[TestCategory(TestCategories.Development)]
		public void Basic_Usage()
		{
			ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("contoso5.redis.cache.windows.net,abortConnect=false,ssl=true,password=...");


			IDatabase cache = connection.GetDatabase();

			cache.StringSet("key1", "value");
			cache.StringSet("key2", 25);

			// Simple get of data types from the cache
			string key1 = cache.StringGet("key1");
			int key2 = (int)cache.StringGet("key2");

		}
	}
}
