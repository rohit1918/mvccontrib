using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class to generate a literal element (span) accompanied by a hidden input element having the same 
	/// value.  Use this if you want to display a value and also have that same value be included in the form post.
	/// </summary>
	public abstract class FormLiteralBase<T> : LiteralBase<T> where T : FormLiteralBase<T>
	{
		protected string hiddenElementName;

		protected FormLiteralBase(string name, MemberExpression forMember, IEnumerable<IMemberBehavior> behaviors) 
			: base(forMember, behaviors)
		{
			hiddenElementName = name;
		}

		protected FormLiteralBase(string name)
		{
			hiddenElementName = name;
		}

		/// <summary>
		/// Set the inner text of the literal (span) and also the value of the 'value' attribute of the 
		/// hidden input element.
		/// </summary>
		/// <param name="value">The value of element.</param>
		/// <returns></returns>
		public override T Value(object value)
		{
			base.Value(value);
			Attr(HtmlAttribute.Value, value);
			return (T)this;
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