using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formula.Core.UnitTesting.Data;
using Formula.Cache.Plugins.FileSystem;
using Formula.Core.UnitTesting;
using KellermanSoftware.CompareNetObjects;

namespace Formula.Cache.UnitTests.FileSystem
{
	[TestClass]
	public class FileSystemCacheTests
	{
		[TestMethod]
		[TestCategory(TestCategories.UnitTest)]
		public void AddGetRemove_Success()
		{
			CompareLogic compare = new CompareLogic();

			// Arrange
			Customer customer = TestData.CreateCustomer();
			ICache cache = new FileSystemCache();

			// Add, Get and Assert
			cache.Add(customer.Id.ToString(), customer);
			Customer newCustomer = cache.Get<Customer>(customer.Id.ToString());
			
			// Assert
			Assert.IsNotNull(newCustomer);
			Assert.IsInstanceOfType(newCustomer, typeof(Customer));
			Assert.IsTrue(compare.Compare(customer, newCustomer).AreEqual);

			// Remove and Assert
			cache.Remove(customer.Id.ToString());
			newCustomer = cache.Get<Customer>(customer.Id.ToString());
			Assert.IsNull(newCustomer);
			
		}

	}
}
