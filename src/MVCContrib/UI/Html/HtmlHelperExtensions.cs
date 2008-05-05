using System.Web.Mvc;

namespace MvcContrib.UI.Html
{
	public static class HtmlHelperExtensions
	{
		public static IFormHelper Form(this HtmlHelper helper)
		{
			return FormHelper.GetInstance(helper.ViewContext);
		}

		public static IValidationHelper Validation(this HtmlHelper helper)
		{
			return ValidationHelper.GetInstance(helper.ViewContext);
		}
	}
}