using System.Collections;

namespace MvcContrib.FluentHtml
{
	public static class EnumerableExtensions
	{
		public static bool Contains(this IEnumerable list, object value)
		{
			if (list == null)
			{
				return false;
			}
			var valueString = value == null ? string.Empty : value.ToString();
			var enumerator = list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				var selectedValueString = enumerator.Current == null
					? string.Empty
					: enumerator.Current.GetType().IsEnum
						? ((int)enumerator.Current).ToString()
						: enumerator.Current.ToString();
				if (valueString == selectedValueString)
				{
					return true;
				}
			}
			return false;  
		}
	}
}