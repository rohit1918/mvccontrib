using System.Web.Mvc;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Interface for elements.
	/// </summary>
	public interface IElement
	{
		/// <summary>
		/// TagBuilder object used to generate HTML.
		/// </summary>
		TagBuilder Builder { get; }

		/// <summary>
		/// Set the value of the specified attribute.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="value">The value of the attribute.</param>
		void SetAttr(string name, object value);
	}
}
