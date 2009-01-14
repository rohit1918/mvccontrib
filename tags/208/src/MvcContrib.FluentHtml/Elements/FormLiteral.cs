using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generates a literal element (span) accompanied by a hidden input element having the same value.  Use this 
	/// if you want to display a value and also have that same value be included in the form post.
	/// </summary>
	public class FormLiteral : Literal, IElement
	{
		protected string hiddenElementName;

		/// <summary>
		/// Generates a literal element (span) accompanied by a hidden input element having the same value.  Use 
		/// this if you want to display a value and also have that same value be included in the form post.
		/// </summary>
		/// <param name="name">Value used to set the 'name' an 'id' attributes of the element.</param>
		public FormLiteral(string name)
		{
			hiddenElementName = name;
		}

		/// <summary>
		/// Set the inner text of the literal (span) and also the value of the 'value' attribute of the 
		/// hidden input element.
		/// </summary>
		/// <param name="value">The value of element.</param>
		/// <returns></returns>
		public override Literal Value(object value)
		{
			base.Value(value);
			Attr(HtmlAttribute.Value, value);
			return this;
		}

		public override string ToString()
		{
			var html = base.ToString();
			if (hiddenElementName != null)
			{
				html += new Hidden(hiddenElementName)
					.Value(builder.Attributes[HtmlAttribute.Value]).ToString();
			}
			return html;
		}
	}
}
