using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for HTML input element of type 'submit.'
	/// </summary>
	public abstract class SubmitButtonBase<T> : Input<T> where T : SubmitButtonBase<T>
	{
		protected SubmitButtonBase(string text) : base(HtmlInputType.Submit, text == null ? null : text.Replace(' ', '_'))
		{
			elementValue = text;
		}
	}
}