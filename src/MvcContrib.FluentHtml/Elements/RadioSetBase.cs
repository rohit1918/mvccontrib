using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for a set of radio buttons.
	/// </summary>
	public class RadioSetBase<T> : OptionsElementBase<T> where T : RadioSetBase<T>
	{
		private string _format;

		public RadioSetBase(string tag, string name, MemberExpression forMember, IEnumerable<IMemberBehavior> behaviors)
			: base(tag, name, forMember, behaviors) { }

		public RadioSetBase(string tag, string name) : base(tag, name) { }

		/// <summary>
		/// Set the selected option.
		/// </summary>
		/// <param name="selectedValue">A value matching the option to be selected.</param>
		/// <returns></returns>
		public virtual T Selected(object selectedValue)
		{
			_selectedValues = new List<object> { selectedValue };
			return (T)this;
		}

		/// <summary>
		/// Specify a format string for the HTML of each radio button and label.
		/// </summary>
		/// <param name="format">A format string.</param>
		public virtual T OptionFormat(string format)
		{
			_format = format;
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
			foreach (var option in _options)
			{
				var value = _valueFieldSelector(option);
				sb.Append(new RadioButton(name)
					.Value(value)
					.Format(_format)
					.LabelAfter(_textFieldSelector(option).ToString())
					.Checked(IsSelectedValue(value)));
			}
			return sb.ToString();
		}
	}
}