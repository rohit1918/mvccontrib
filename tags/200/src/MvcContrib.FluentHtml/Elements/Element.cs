using System;
using System.Text;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public abstract class Element<T> where T : Element<T>, IElement
	{
		protected const string LABEL_FORMAT = "{0}_Label";

		protected readonly TagBuilder builder;

		protected string labelBeforeText;
		protected string labelAfterText;
		protected string labelClass;

		protected Element(string tag)
		{
			builder = new TagBuilder(tag);
		}

		public virtual TagBuilder Builder
		{
			get { return builder; }
		}

		public virtual T Id(string value)
		{
			builder.MergeAttribute(HtmlAttribute.Id, value, true);
			return (T)this;
		}

		public virtual T Class(string classToAdd)
		{
			builder.AddCssClass(classToAdd);
			return (T)this;
		}

		public virtual T Title(string value)
		{
			builder.MergeAttribute(HtmlAttribute.Title, value, true);
			return (T)this;
		}

		public virtual T Styles(params Func<string, string>[] values)
		{
			var sb = new StringBuilder();
			foreach (var func in values)
			{
				sb.AppendFormat("{0}:{1};", func.Method.GetParameters()[0].Name.Replace('_', '-'), func(null));
			}
			builder.MergeAttribute(HtmlAttribute.Style, sb.ToString());
			return (T)this;
		}

		public virtual T OnClick(string value)
		{
			builder.MergeAttribute(HtmlEventAttribute.OnClick, value, true);
			return (T)this;
		}

		public virtual void SetAttr(string name, object value)
		{
			var valueString = value == null ? null : value.ToString();
			builder.MergeAttribute(name, valueString, true);
		}

		public virtual T Attr(string name, object value)
		{
			SetAttr(name, value);
			return (T)this;
		}

		public virtual T Label(string value, string @class)
		{
			labelClass = @class;
			return Label(value);
		}

		public virtual T Label(string value)
		{
			labelBeforeText = value;
			return (T)this;
		}

		public virtual T LabelAfter(string value, string @class)
		{
			labelClass = @class;
			return LabelAfter(value);
		}

		public virtual T LabelAfter(string value)
		{
			labelAfterText = value;
			return (T)this;
		}

		public void RemoveAttr(string name)
		{
			builder.Attributes.Remove(name);
		}

		public override string ToString()
		{
			var html = RenderLabel(labelBeforeText);
			html += builder.ToString(TagRenderMode);
			html += RenderLabel(labelAfterText);
			return html;
		}

		public virtual TagRenderMode TagRenderMode
		{
			get { return TagRenderMode.Normal; }
		}

		protected virtual string RenderLabel(string labelText)
		{
			if (labelText == null)
			{
				return null;
			}
			var labelBuilder = new TagBuilder(HtmlTag.Label);
			if (builder.Attributes.ContainsKey(HtmlAttribute.Id))
			{
				var id = builder.Attributes[HtmlAttribute.Id];
				labelBuilder.MergeAttribute(HtmlAttribute.For, id);
				labelBuilder.MergeAttribute(HtmlAttribute.Id, string.Format(LABEL_FORMAT, id));
				if (!string.IsNullOrEmpty(labelClass))
				{
					labelBuilder.MergeAttribute(HtmlAttribute.Class, labelClass);
				}
			}
			labelBuilder.SetInnerText(labelText);
			return labelBuilder.ToString();
		}
	}
}
