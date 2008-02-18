using System.Web.Mvc;

namespace MvcContrib.UI.Html
{
	public static class FormHelperExtensions
	{
		public static IFormHelper Form(this HtmlHelper helper)
		{
			//TODO: Replace with per-web request IoC.
			return new FormHelper(helper.ViewContext);
		}
	}
}