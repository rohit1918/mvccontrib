using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public class TextBox : TextInput<TextBox>
	{
		public TextBox(string name) : base(HtmlInputType.Text, name) { }

		public TextBox(string name, MemberExpression forMember, IEnumerable<IMemberBehavior> behaviors)
			: base(HtmlInputType.Text, name, forMember, behaviors) { }
	}
}
