using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for a set of radio buttons.
	/// </summary>
	public class RadioSetBase<T> : OptionsElementBase<T>, IElement where T : RadioSetBase<T>
	{
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
				var radioButton = new RadioButton(name)
					.Value(value)
					.LabelAfter(_textFieldSelector(option).ToString())
					.Checked(IsSelectedValue(value));
				sb.Append(radioButton.ToString());
			}
			return sb.ToString();
		}
	}
}