using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generate an HTML textarea element.
	/// </summary>
	public class TextArea : FormElement<TextArea>
	{
		protected string format;
		protected object rawValue;

		/// <summary>
		/// Generate an HTML textarea element.
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		public TextArea(string name) : base(HtmlTag.TextArea, name) { }

		/// <summary>
		/// Generate an HTML textarea element.
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		/// <param name="forMember">Expression indicating the view model member assocaited with the element</param>
		/// <param name="behaviors">Behaviors to apply to the element</param>
		public TextArea(string name, MemberExpression forMember, IEnumerable<IMemberBehavior> behaviors)
			: base(HtmlTag.TextArea, name, forMember, behaviors) { }

		/// <summary>
		/// Set the inner text.
		/// </summary>
		/// <param name="value">The value of the inner text.</param>
		public virtual TextArea Value(object value)
		{
			rawValue = value;
			return this;
		}

		/// <summary>
		/// Set the 'rows' attribute.
		/// </summary>
		/// <param name="value">The value of the rows attribute<./param>
		public virtual TextArea Rows(int value)
		{
			Attr(HtmlAttribute.Rows, value);
			return this;
		}

		/// <summary>
		/// Set the 'columns' attribute.
		/// </summary>
		/// <param name="value">The value of the columns attribute.</param>
		public virtual TextArea Columns(int value)
		{
			Attr(HtmlAttribute.Cols, value);
			return this;
		}

		/// <summary>
		/// Specify a format string to be applied to the value.  The format string can be either a
		/// specification (e.g., '$#,##0.00') or a placeholder (e.g., '{0:$#,##0.00}').
		/// </summary>
		/// <param name="value">A format string.</param>
		public virtual TextArea Format(string value)
		{
			format = value;
			return this;
		}

		protected override void PreRender()
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

		public override TagRenderMode TagRenderMode
		{
			get { return TagRenderMode.Normal; }
		}
	}
}
