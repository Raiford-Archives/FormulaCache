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

					
		public T Get<T>(string key)
		{
			LinkedList<ICache> missedCaches = new LinkedList<ICache>();

			T value = default(T);


			foreach (var c in _list)
			{
				value = c.Get<T>(key);
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
			return default(T);
		}
		
	
		public void Add<T>(string key, T value, bool pushToLinked = false)
		{
			ICache cache = _list.First<ICache>();

			if(pushToLinked)
			{
				foreach(var c in _list)
				{
					c.Add<T>(key, value);
				}
			}
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
				
		public void Remove(string key)
		{
			foreach (var c in _list)
			{
				c.Remove(key);
			}
		}

	
	}
}
