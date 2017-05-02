using System;
using System.Threading.Tasks;

namespace Formula.Cache
{

	/// <summary>
	/// ICache defines a Cache Plugin that can be used with SuperCache. ICache represent a wrapper around either a custom Cache or 
	/// any third party Cache such as Azure, Redis, MongoDb or any number of other implementations. There are also many open source
	/// implementations that can be designed and developed using the Super Cache Container Tool.
	/// </summary>
	public interface ICache
	{
		Guid InstanceId { get; } 
		void Add<T>(string key, T value, CacheItemBehavior behavior);
		Task AddAsync<T>(string key, T value, CacheItemBehavior behavior);
		void Add<T>(string key, T value, int expirationMilliseconds=3600, CacheExpirationType expirationType=CacheExpirationType.Absolute);
		Task AddAsync<T>(string key, T value, int expirationMilliseconds=3600, CacheExpirationType expirationType=CacheExpirationType.Absolute);
		T Get<T>(string key);
		Task<T> GetAsync<T>(string key);
		void Remove(string key);
		Task RemoveAsync(string key);
		Task CleanupExpired();



		//bool RequiresSerialization { get; set; }
		//int Count { get; set; }

		//string HostingModel { InProcess, LocalMachine, Remote}

	}
}
