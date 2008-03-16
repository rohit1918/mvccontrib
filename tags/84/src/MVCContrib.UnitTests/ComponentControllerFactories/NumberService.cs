using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcContrib.UnitTests.ComponentControllerFactories
{
	public interface INumberService
	{
		IEnumerable<int> GetNumbersBetween(int low, int high);
	}
	public class NumberService : INumberService
	{

		#region INumberService Members

		public IEnumerable<int> GetNumbersBetween(int low, int high)
		{
			for (int i =low+1; i < high; i++)
			{
				yield return i;
			}
		}

		#endregion
	}
}
