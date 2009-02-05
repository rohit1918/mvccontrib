namespace MvcContrib.FluentHtml
{
    public static class StringExtensions
    {
        public static string FormatAsHtmlId(this string name)
        {
            return name == null ? string.Empty : name.Replace(' ', '_').Replace('.', '_');
        }
    }
}