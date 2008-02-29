using System.Web.Mvc;

namespace MvcContrib.UI.Html
{
	public static class FormHelperExtensions
	{
		public static IFormHelper Form(this HtmlHelper helper)
		{
			return FormHelper.GetInstance(helper.ViewContext);
		}
	}
}