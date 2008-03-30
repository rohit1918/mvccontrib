using System;
using System.Globalization;

namespace MvcContrib.NHamlViewEngine.Utilities
{
	internal static class StringUtils
	{
		public static string FormatCurrentCulture(string format, params object[] values)
		{
			return String.Format(CultureInfo.CurrentCulture, format, values);
		}

		public static string FormatInvariant(string format, params object[] values)
		{
			return String.Format(CultureInfo.InvariantCulture, format, values);
		}
	}
}