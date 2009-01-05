using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for elements that are disablable.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class DisableableElement<T> : Element<T> where T : DisableableElement<T>, IElement
	{
		protected DisableableElement(string tag) : base(tag) { }

		/// <summary>
		/// Set the disabled attribute.
		/// </summary>
		/// <param name="value">Whether the element should be disabled.</param>
		public T Disabled(bool value)
		{
			if (value)
			{
				Attr(HtmlAttribute.Disabled, HtmlAttribute.Disabled);
			}
			else
			{
				RemoveAttr(HtmlAttribute.Disabled);
			}
			return (T)this;
		}
	}
}
