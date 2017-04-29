using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formula.Core.UnitTesting.Data;

namespace Formula.Cache
{
	public interface ICache
	{
		void Add<T>(string key, T value);
		T Get<T>(string key);
		void Remove(string key);
	
		void CleanupExpired();

	}
}
