using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generate a literal (text inside a span element).
	/// </summary>
	public class Literal : Element<Literal>, IElement
	{
		protected string format;
		protected object rawValue;

		/// <summary>
		/// Generate a literal (text inside a span element).
		/// </summary>
		public Literal() : base(HtmlTag.Span) { }

		/// <summary>
		/// Set the inner text of the span element.
		/// </summary>
		/// <param name="value">The value of the inner text.</param>
		public virtual Literal Value(object value)
		{
			rawValue = value;
			return this;
		}

		/// <summary>
		/// Specify a format string to be applied to the value.  The format string can be either a
		/// specification (e.g., '$#,##0.00') or a placeholder (e.g., '{0:$#,##0.00}').
		/// </summary>
		/// <param name="value">A format string.</param>
		public virtual Literal Format(string value)
		{
			format = value;
			return this;
		}

		public override string ToString()
		{
			builder.SetInnerText(FormatValue(rawValue));
			return base.ToString();
		}

		protected virtual string FormatValue(object value)
		{
			return string.IsNullOrEmpty(format)
				? value == null
					? null
					: value.ToString()
				: (format.StartsWith("{0") && format.EndsWith("}"))
					? string.Format(format, value)
					: string.Format("{0:" + format + "}", value);
		}
	}
}
