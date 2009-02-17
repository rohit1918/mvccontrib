using System.Text.RegularExpressions;

namespace MvcContrib.FluentHtml
{
	public static class StringExtensions
	{
		public static string FormatAsHtmlId(this string name)
		{
			//Replace charactes not valid for ID attribute with underscores.
			//Replace dots with underscores to distinguish from name attribute.
			return string.IsNullOrEmpty(name) 
				? string.Empty 
				: Regex.Replace(name, "[^a-zA-Z0-9-:]", "_");
		}
	}
}