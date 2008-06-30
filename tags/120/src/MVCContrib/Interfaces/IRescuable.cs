using System;

namespace MvcContrib.Interfaces
{
	public interface IRescuable
	{
		void OnPreRescue(Exception thrownException);
	}
}