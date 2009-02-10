using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for a list of checkboxes.
	/// </summary>
	public class CheckBoxListBase<T> : OptionsElementBase<T> where T : CheckBoxListBase<T>
	{
		private string _itemFormat;

		public CheckBoxListBase(string tag, string name, MemberExpression forMember, IEnumerable<IMemberBehavior> behaviors)
			: base(tag, name, forMember, behaviors) { }

		public CheckBoxListBase(string tag, string name) : base(tag, name) { }

		/// <summary>
		/// Set the selected values.
		/// </summary>
		/// <param name="selectedValues">Values matching the values of options to be selected.</param>
		public virtual T Selected(IEnumerable selectedValues)
		{
			_selectedValues = selectedValues;
			return (T)this;
		}

		/// <summary>
		/// Specify a format string for the HTML of each checkbox button and label.
		/// </summary>
		/// <param name="value">A format string.</param>
		public virtual T ItemFormat(string value)
		{
			_itemFormat = value;
			return (T)this;
		}

		protected override void PreRender()
		{
			builder.InnerHtml = RenderBody();
		}

		public override TagRenderMode TagRenderMode
		{
			get { return TagRenderMode.Normal; }
		}

		private string RenderBody()
		{
			if (_options == null)
			{
				return null;
			}

			var name = builder.Attributes[HtmlAttribute.Name];
			builder.Attributes.Remove(HtmlAttribute.Name);
			var sb = new StringBuilder();
			var i = 0;
			foreach (var option in _options)
			{
				var value = _valueFieldSelector(option);
				var html = new CheckBox(name)
					.Id(string.Format("{0}_{1}", name.FormatAsHtmlId(), i))
					.Value(value)
					.LabelAfter(_textFieldSelector(option).ToString())
					.Checked(IsSelectedValue(value))
					.ToCheckBoxOnlyHtml();
				sb.Append(_itemFormat == null 
					? html 
					: string.Format(_itemFormat, html));
				i++;
			}
			return sb.ToString();
		}
	}
}