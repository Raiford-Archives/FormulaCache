using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formula.Cache
{
	public interface IChainedCache
	{
		void LinkFirst(ICache linkedCache, bool addFirst);


	}
}
