using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generate an HTML input element of type 'submit.'
	/// </summary>
	public class SubmitButton : Input<SubmitButton>
	{
		/// <summary>
		/// Generate an HTML input element of type 'submit.'
		/// </summary>
		/// <param name="text">Value of the 'value' and 'name' attributes.  Also used to derive the 'id' attribute.</param>
		public SubmitButton(string text) : base(HtmlInputType.Submit, text == null ? null : text.Replace(' ', '_'))
		{
			elementValue = text;
		}
	}
}
