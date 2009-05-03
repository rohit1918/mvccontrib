using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for HTML elements.
	/// </summary>
	/// <typeparam name="T">The derived class type.</typeparam>
	public abstract class Element<T> : IMemberElement where T : Element<T>, IElement
	{
		protected const string LABEL_FORMAT = "{0}_Label";

		protected readonly TagBuilder builder;

		protected string labelBeforeText;
		protected string labelAfterText;
		protected string labelClass;
		protected MemberExpression forMember;
		protected IEnumerable<IBehaviorMarker> behaviors;

		protected Element(string tag, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors) : this(tag)
		{
			this.forMember = forMember;
			this.behaviors = behaviors;
		}

		protected Element(string tag)
		{
			builder = new TagBuilder(tag);
		}

		/// <summary>
		/// TagBuilder object used to generate HTML for elements.
		/// </summary>
		public virtual TagBuilder Builder
		{
			get { return builder; }
		}

		/// <summary>
		/// Set the 'id' attribute.
		/// </summary>
		/// <param name="value">The value of the 'id' attribute.</param>
		public virtual T Id(string value)
		{
			builder.MergeAttribute(HtmlAttribute.Id, value, true);
			return (T)this;
		}

		/// <summary>
		/// Add a value to the 'class' attribute.
		/// </summary>
		/// <param name="classToAdd">The value of the class to add.</param>
		public virtual T Class(string classToAdd)
		{
			builder.AddCssClass(classToAdd);
			return (T)this;
		}

		/// <summary>
		/// Set the 'title' attribute.
		/// </summary>
		/// <param name="value">The value of the 'title' attribute.</param>
		public virtual T Title(string value)
		{
			builder.MergeAttribute(HtmlAttribute.Title, value, true);
			return (T)this;
		}

		/// <summary>
		/// Set the 'style' attribute.
		/// </summary>
		/// <param name="values">A list of funcs, each epxressing a style name value pair.  Replace dashes with 
		/// underscores in style names. For example 'margin-top:10px;' is expressed as 'margin_top => "10px"'.</param>
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

		/// <summary>
		/// Set the 'onclick' attribute.
		/// </summary>
		/// <param name="value">The value for the attribute.</param>
		/// <returns></returns>
		public virtual T OnClick(string value)
		{
			builder.MergeAttribute(HtmlEventAttribute.OnClick, value, true);
			return (T)this;
		}

		/// <summary>
		/// Set the value of the specified attribute.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="value">The value of the attribute.</param>
		public virtual void SetAttr(string name, object value)
		{
			var valueString = value == null ? null : value.ToString();
			builder.MergeAttribute(name, valueString, true);
		}

		/// <summary>
		/// Get the value of the specified attribute.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		public virtual string GetAttr(string name)
		{
			string result;
			builder.Attributes.TryGetValue(name, out result);
			return result;
		}

		/// <summary>
		/// Set the value of a specified attribute.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="value">The value of the attribute.</param>
		public virtual T Attr(string name, object value)
		{
			SetAttr(name, value);
			return (T)this;
		}

		/// <summary>
		/// Generate a label before the element.
		/// </summary>
		/// <param name="value">The inner text of the label.</param>
		/// <param name="class">The value of the 'class' attribute for the label.</param>
		public virtual T Label(string value, string @class)
		{
			SetLabel(value, @class);
			return (T)this;
		}

		/// <summary>
		/// Generate a label before the element.
		/// </summary>
		/// <param name="value">The inner text of the label.</param>
		public virtual T Label(string value)
		{
			SetLabel(value, null);
			return (T)this;
		}

		/// <summary>
		/// Generate a label before the element.
		/// </summary>
		/// <param name="value">The inner text of the label.</param>
		/// <param name="class">The value of the 'class' attribute for the label.</param>
		public virtual void SetLabel(string value, string @class)
		{
			labelBeforeText = value;
			labelClass = @class;
		}

		/// <summary>
		/// Generate a label after the element.
		/// </summary>
		/// <param name="value">The inner text of the label.</param>
		/// <param name="class">The value of the 'class' attribute for the label.</param>
		public virtual T LabelAfter(string value, string @class)
		{
			SetLabelAfter(value, @class);
			return (T)this;
		}

		/// <summary>
		/// Generate a label after the element.
		/// </summary>
		/// <param name="value">The inner text of the label.</param>
		public virtual T LabelAfter(string value)
		{
			SetLabelAfter(value, null);
			return (T)this;
		}

		/// <summary>
		/// Generate a label after the element.
		/// </summary>
		/// <param name="value">The inner text of the label.</param>
		/// <param name="class">The value of the 'class' attribute for the label.</param>
		public virtual void SetLabelAfter(string value, string @class)
		{
			labelAfterText = value;
			labelClass = @class;
		}

		/// <summary>
		/// Remove an attribute.
		/// </summary>
		/// <param name="name">The name of the attribute to remove.</param>
		public void RemoveAttr(string name)
		{
			builder.Attributes.Remove(name);
		}

		/// <summary>
		/// If no label has been explicitly set, set the label using the element name.
		/// </summary>
		public virtual T AutoLabel()
		{
			SetAutoLabel();
			return (T)this;
		}

		/// <summary>
		/// If no label before has been explicitly set, set the label before using the element name.
		/// </summary>
		public virtual T AutoLabelAfter()
		{
			SetAutoLabelAfter();
			return (T)this;
		}

		public override string ToString()
		{
			ApplyBehaviors();
			PreRender();
			var html = RenderLabel(labelBeforeText);
			html += builder.ToString(TagRenderMode);
			html += RenderLabel(labelAfterText);
			return html;
		}

		/// <summary>
		/// How the tag should be closed.
		/// </summary>
		public virtual TagRenderMode TagRenderMode
		{
			get { return TagRenderMode.Normal; }
		}

		/// <summary>
		/// Expression indicating the view model member assocaited with the element.</param>
		/// </summary>
		public virtual MemberExpression ForMember
		{
			get { return forMember; }
		}

		protected virtual string RenderLabel(string labelText)
		{
			if (labelText == null)
			{
				return null;
			}
			var labelBuilder = GetLabelBuilder();
			labelBuilder.SetInnerText(labelText);
			return labelBuilder.ToString();
		}

		protected TagBuilder GetLabelBuilder()
		{
			var labelBuilder = new TagBuilder(HtmlTag.Label);
			if (builder.Attributes.ContainsKey(HtmlAttribute.Id))
			{
				var id = builder.Attributes[HtmlAttribute.Id];
				labelBuilder.MergeAttribute(HtmlAttribute.For, id);
				labelBuilder.MergeAttribute(HtmlAttribute.Id, string.Format(LABEL_FORMAT, id));
			}
			if (!string.IsNullOrEmpty(labelClass))
			{
				labelBuilder.MergeAttribute(HtmlAttribute.Class, labelClass);
			}
			return labelBuilder;
		}

		protected void ApplyBehaviors()
		{
			if(behaviors == null)
			{
				return;
			}
			foreach(var behavior in behaviors)
			{
				if (behavior is IBehavior)
				{
					((IBehavior)behavior).Execute(this);
				}
				if (behavior is IMemberBehavior && forMember != null)
				{
					((IMemberBehavior)behavior).Execute(this);
				}
			}
		}

		public virtual void SetAutoLabel()
		{
			if (labelBeforeText == null)
			{
				var settings = GetAutoLabelSettings();
				SetLabel(GetAutoLabelText(settings), settings == null ? null : settings.LabelCssClass);
			}
		}

		public virtual void SetAutoLabelAfter()
		{
			if (labelAfterText == null)
			{
				var settings = GetAutoLabelSettings();
				SetLabelAfter(GetAutoLabelText(settings), settings == null ? null : settings.LabelCssClass);
			}
		}

		private AutoLabelSettings GetAutoLabelSettings()
		{
			//TODO: should we throw if there is more than one?
			return behaviors == null 
				? new AutoLabelSettings(false, null, null) 
				: behaviors.Where(x => x is AutoLabelSettings).FirstOrDefault() as AutoLabelSettings;
		}

		private string GetAutoLabelText(AutoLabelSettings settings)
		{
			var result = GetAttr(HtmlAttribute.Name);
			if (result == null)
			{
				return result;
			}
			if (settings.UseFullNameForNestedProperties)
			{
				result = result.Replace('.', ' ');
			}
			else
			{
				var lastDot = result.LastIndexOf(".");
				if (lastDot >= 0)
				{
					result = result.Substring(lastDot + 1);
				}
			}
			result = result.PascalCaseToPhrase();
			result = RemoveArrayNotationFromPhrase(result);
			result = settings.LabelFormat != null
				? string.Format(settings.LabelFormat, result)
				: result;
			return result;
		}

		private string RemoveArrayNotationFromPhrase(string phrase) 
		{
			if (phrase.IndexOf("[") >= 0)
			{
				var words = new List<string>(phrase.Split(' '));
				words = words.ConvertAll<string>(RemoveArrayNotation);
				phrase = string.Join(" ", words.ToArray());
			}
			return phrase;
		}

		private string RemoveArrayNotation(string s)
		{
			var index = s.LastIndexOf('[');
			return index >= 0 
				? s.Remove(index) 
				: s;
		}

		protected virtual void PreRender() { }
	}
}