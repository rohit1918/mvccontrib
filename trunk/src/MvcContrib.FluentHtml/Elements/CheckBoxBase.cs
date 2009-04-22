using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for HTML input element of type 'checkbox.'
	/// </summary>
	public abstract class CheckBoxBase<T> : Input<T>, ICheckbox where T : CheckBoxBase<T>
	{
		protected CheckBoxBase(string name) : base(HtmlInputType.Checkbox, name)
		{
			elementValue = "true";
		}

		protected CheckBoxBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlInputType.Checkbox, name, forMember, behaviors)
		{
			elementValue = "true";
		}

		/// <summary>
		/// Set the checked attribute.
		/// </summary>
		/// <param name="value">Whether the checkbox should be checked.</param>
		public virtual T Checked(bool value)
		{
			if (value)
			{
				Attr(HtmlAttribute.Checked, HtmlAttribute.Checked);
			}
			else
			{
				RemoveAttr(HtmlAttribute.Checked);
			}
			return (T)this;
		}

		void ICheckbox.Checked(bool value)
		{
			Checked(value);
		}

		public override string ToString()
		{
			var html = ToCheckBoxOnlyHtml();
			var hiddenId = "_Hidden";
			if(Builder.Attributes.ContainsKey("id"))
			{
				hiddenId = Builder.Attributes["id"] + hiddenId;
			}
			var hidden = new Hidden(builder.Attributes[HtmlAttribute.Name]).Id(hiddenId).Value("false").ToString();
			return string.Concat(html, hidden);
		}

		public string ToCheckBoxOnlyHtml()
		{
			return base.ToString();
		}
	}
}