using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public class TextArea : FormElement<TextArea>
	{
		protected string format;
		protected object rawValue;

		public TextArea(string name) : base(HtmlTag.TextArea, name) { }

		public TextArea(string name, MemberExpression forMember, IEnumerable<IMemberBehavior> behaviors)
			: base(HtmlTag.TextArea, name, forMember, behaviors) { }

		public virtual TextArea Value(object value)
		{
			rawValue = value;
			return this;
		}

		public virtual TextArea Rows(int value)
		{
			Attr(HtmlAttribute.Rows, value);
			return this;
		}

		public virtual TextArea Columns(int value)
		{
			Attr(HtmlAttribute.Cols, value);
			return this;
		}

		public virtual TextArea Format(string value)
		{
			format = value;
			return this;
		}

		public override string ToString()
		{
			if (!(rawValue is string) && rawValue is IEnumerable)
			{
				var items = new List<string>();
				foreach (var item in (IEnumerable)rawValue)
				{
					items.Add(FormatValue(item));
				}
				builder.SetInnerText(string.Join(Environment.NewLine, items.ToArray()));
			}
			else
			{
				builder.SetInnerText(FormatValue(rawValue));
			}
			return base.ToString();
		}

		protected virtual string FormatValue(object value)
		{
			return string.IsNullOrEmpty(format)
				? value == null 
					? null 
					: value.ToString()
				: string.Format("{0:" + format + "}", value);
		}

		public override TagRenderMode TagRenderMode
		{
			get { return TagRenderMode.Normal; }
		}
	}
}
