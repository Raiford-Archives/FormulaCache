using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KellermanSoftware.CompareNetObjects;
using Formula.Core.UnitTesting.Data;
using System.Collections;
using System.Collections.Generic;

namespace Formula.Cache.UnitTests
{
	public class CachePluginTestsBase
	{
		protected void AddGetRemoveGet_Success(ICache cachePlugin)
		{
			CompareLogic compare = new CompareLogic();

			// Arrange
			Customer customer = TestData.CreateCustomer();
			ICache cache = cachePlugin;

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


		protected void AddGetRemoveGet_Performance(ICache cachePlugin, int iterations)
		{
			IList<Customer> customers = TestData.GetCustomerList(iterations);


			foreach (var c in customers)
			{
				cachePlugin.Add(c.Id.ToString(), c);
			}

			foreach (var c in customers)
			{
				Customer customer = cachePlugin.Get<Customer>(c.Id.ToString());
			}

			foreach (var c in customers)
			{
				cachePlugin.Remove(c.Id.ToString());
			}
		}
	}
}