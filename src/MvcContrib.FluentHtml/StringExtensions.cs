namespace MvcContrib.FluentHtml
{
	public static class StringExtensions
	{
		public static string FormatAsHtmlId(this string name)
		{
			return string.IsNullOrEmpty(name) 
				? string.Empty 
				: name.Replace(' ', '_').Replace('.', '_');
		}
	}
}