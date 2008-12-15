using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public class CheckBox : Input<CheckBox>
	{
		public CheckBox(string name) : base(HtmlInputType.Checkbox, name)
		{
			elementValue = "true";
		}

		public CheckBox(string name, MemberExpression forMember, IEnumerable<IMemberBehavior> actions)
			: base(HtmlInputType.Checkbox, name, forMember, actions)
		{
			elementValue = "true";
		}

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
			var hidden = new Hidden(builder.Attributes[HtmlAttribute.Name]).Value("false").ToString();
			return string.Concat(html, hidden);
		}
	}
}
