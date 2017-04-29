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
			object customerObject = cache.Get(customer.Id.ToString());
			Customer newCustomer = customerObject as Customer;

			// Assert
			Assert.IsNotNull(customerObject);
			Assert.IsInstanceOfType(customerObject, typeof(Customer));
			Assert.IsTrue(compare.Compare(customer, newCustomer).AreEqual);

			// Remove and Assert
			cache.Remove(customer.Id.ToString());
			customerObject = cache.Get(customer.Id.ToString());
			Assert.IsNull(customerObject);
			
		}

	}
}
