using System.Web.Mvc;

namespace MvcContrib.UiHelpers
{
    public static class TextInputExtensions
    {
        private static string INPUT_FORMAT = "<input type=\"text\" {0}/>";

        public static string TextInput(this HtmlHelper htmlHelper)
        {
            return FormattedHtmlInput(INPUT_FORMAT, string.Empty);
        }

        public static string TextInput(this HtmlHelper htmlHelper, object attributes)
        {
            if(attributes == null)
                return TextInput(htmlHelper);

            string htmlAttributes = attributes.ToAttributeList();
            return FormattedHtmlInput(INPUT_FORMAT, htmlAttributes);
        }

        private static string FormattedHtmlInput(string format, string attributes)
        {
            return string.Format(format, attributes);
        }
    }
}