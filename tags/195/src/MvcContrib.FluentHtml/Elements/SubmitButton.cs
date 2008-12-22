using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public class SubmitButton : Input<SubmitButton>
	{
		public SubmitButton(string text) : base(HtmlInputType.Submit, text == null ? null : text.Replace(' ', '_'))
		{
			elementValue = text;
		}
	}
}
