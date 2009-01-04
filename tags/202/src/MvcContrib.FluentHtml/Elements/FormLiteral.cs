using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public class FormLiteral : Literal, IElement
	{
		protected string hiddenElementName;

		public FormLiteral(string name)
		{
			hiddenElementName = name;
		}

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
