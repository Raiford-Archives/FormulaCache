using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formula.Cache
{
	public class ChainedCache : IChainedCache
	{
		private LinkedList<ICache> _list = new LinkedList<ICache>();


	//	private IPowerCache GetNextCache()


		public object Get(Guid key)
		{
			return Get(key.ToString());
		}

		public object Get(string key)
		{
			LinkedList<ICache> missedCaches = new LinkedList<ICache>();

			object value = null;


			foreach (var c in _list)
			{
				value = c.Get(key);
				if (value != null)
				{
					// You got the value now add the value to the missed Caches
					foreach (var m in missedCaches)
					{
						m.Add(key, value);
					}

					return value;
				}
				else
				{
					//Missed
					missedCaches.AddFirst(c);
				}

			}
			return null;
		}

		public T Get<T>(Guid key)
		{
			return (T)Get(key.ToString());
		}

		public T Get<T>(string key)
		{
			return (T)Get(key);
		}


		public void Add(Guid key, object value)
		{
			Add(key.ToString(), value);
		}

		public void Add(string key, object value, bool pushToLinked = false)
		{
			ICache cache = _list.First<ICache>();

			if(pushToLinked)
			{
				foreach(var c in _list)
				{
					c.Add(key, value);
				}
			}
		}

		public void Add<T>(Guid key, T value)
		{
			Add(key.ToString(), value);
		}

		

		public void LinkFirst(ICache linkedCache, bool addFirst = true)
		{
			if(addFirst)
			{
				_list.AddFirst(linkedCache);
			}
			else
			{
				_list.AddLast(linkedCache);
			}
		}

		public void Remove(Guid key)
		{
			Remove(key.ToString());
		}

		public void Remove(string key)
		{
			foreach (var c in _list)
			{
				c.Remove(key);
			}
		}

	
	}
}
