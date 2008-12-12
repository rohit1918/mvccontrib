using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public abstract class DisableableElement<T> : Element<T> where T : DisableableElement<T>, IElement
	{
		protected DisableableElement(string tag) : base(tag) { }

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
