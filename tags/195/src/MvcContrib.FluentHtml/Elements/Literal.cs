using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public class Literal : Element<Literal>, IElement
	{
		protected string format;
		protected object rawValue;

		public Literal() : base(HtmlTag.Span) { }

		public virtual Literal Value(object value)
		{
			rawValue = value;
			return this;
		}

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

		protected string FormatValue(object value)
		{
			return string.IsNullOrEmpty(format) 
				? value == null 
					? null 
					: value.ToString()
				: string.Format("{0:" + format + "}", value);
		}
	}
}
