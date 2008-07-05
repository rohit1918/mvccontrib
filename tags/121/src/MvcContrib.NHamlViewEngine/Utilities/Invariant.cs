using System;
using MvcContrib.NHamlViewEngine.Properties;

namespace MvcContrib.NHamlViewEngine.Utilities
{
	internal static class Invariant
	{
		public static void ArgumentNotNull(object argument, string argumentName)
		{
			if(argument == null)
			{
				throw new ArgumentNullException(argumentName);
			}
		}

		public static void IsNotNull(object obj)
		{
			if(obj == null)
			{
				throw new InvalidOperationException(Resources.ObjectNull);
			}
		}
	}
}