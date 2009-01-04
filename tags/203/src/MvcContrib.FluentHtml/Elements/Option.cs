using System.Web.Mvc;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public class Option
	{
		protected readonly TagBuilder builder;

		public Option()
		{
			builder = new TagBuilder(HtmlTag.Option);
		}

		public Option Value(string value) 
		{
			builder.MergeAttribute(HtmlAttribute.Value, value, true);
			return this;
		}

		public virtual Option Text(string value)
		{
			builder.SetInnerText(value);
			return this;
		}

		public virtual Option Selected(bool value)
		{
			if (value)
			{
				builder.MergeAttribute(HtmlAttribute.Selected, HtmlAttribute.Selected, true);
			}
			else
			{
				builder.Attributes.Remove(HtmlAttribute.Selected);
			}
			return this;
		}

		public virtual Option Disabled(bool value)
		{
			if (value)
			{
				builder.MergeAttribute(HtmlAttribute.Disabled, HtmlAttribute.Disabled, true);
			}
			else
			{
				builder.Attributes.Remove(HtmlAttribute.Disabled);
			}
			return this;
		}

		public override string ToString()
		{
			return builder.ToString();
		}
	}
}
