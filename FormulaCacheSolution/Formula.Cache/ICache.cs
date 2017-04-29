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
		void Add(string key, object value);
		void Add(Guid key, object value);
		void Add<T>(Guid key, T value);
		void Remove(string key);
		void Remove(Guid key);
		object Get(string key);
		object Get(Guid key);
		T Get<T>(string key);
		T Get<T>(Guid key);

		void CleanupExpired();

	}
}
