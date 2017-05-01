using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formula.Core.Serialization;
using System.Diagnostics;

namespace Formula.Cache.UnitTests
{
	[TestClass]
	public class ConfigFileTests
	{
		[TestMethod]
		public void CacheConfigLoad_Success()
		{



			CacheConfig config = new CacheConfig();

			string json = JsonSerializer.SerializeObject<CacheConfig>(config);

			Debug.WriteLine(json);


		}
	}
}
