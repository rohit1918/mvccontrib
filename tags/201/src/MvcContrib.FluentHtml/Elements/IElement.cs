using System.Web.Mvc;

namespace MvcContrib.FluentHtml.Elements
{
	public interface IElement
	{
		TagBuilder Builder { get; }
		void SetAttr(string name, object value);
	}
}
