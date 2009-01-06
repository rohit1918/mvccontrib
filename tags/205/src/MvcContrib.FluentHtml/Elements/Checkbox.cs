using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generate an HTML input element of type 'checkbox.'
	/// </summary>
	public class CheckBox : Input<CheckBox>
	{
		/// <summary>
		/// Generate an HTML input element of type 'checkbox.'
		/// </summary>
		/// <param name="name">Value used to set the 'name' an 'id' attributes of the element</param>
		public CheckBox(string name) : base(HtmlInputType.Checkbox, name)
		{
			elementValue = "true";
		}

		/// <summary>
		/// Generate an HTML input element of type 'checkbox.'
		/// </summary>
		/// <param name="name">Value of the 'name' attribute of the element.  Also used to derive the 'id' attribute.</param>
		/// <param name="forMember">Expression indicating the view model member assocaited with the element.</param>
		/// <param name="behaviors">Behaviors to apply to the element.</param>
		public CheckBox(string name, MemberExpression forMember, IEnumerable<IMemberBehavior> behaviors)
			: base(HtmlInputType.Checkbox, name, forMember, behaviors)
		{
			elementValue = "true";
		}

		/// <summary>
		/// Set the checked attribute.
		/// </summary>
		/// <param name="value">Whether the checkbox should be checked.</param>
		public virtual CheckBox Checked(bool value)
		{
			if (value)
			{
				Attr(HtmlAttribute.Checked, HtmlAttribute.Checked);
			}
			else
			{
				RemoveAttr(HtmlAttribute.Checked);
			}
			return this;
		}

		public override string ToString()
		{
			var html = base.ToString();

			string hiddenId = "_Hidden";

			if(Builder.Attributes.ContainsKey("id"))
			{
				hiddenId = Builder.Attributes["id"] + hiddenId;
			}

			var hidden = new Hidden(builder.Attributes[HtmlAttribute.Name]).Id(hiddenId).Value("false").ToString();
			return string.Concat(html, hidden);
		}
	}
}
