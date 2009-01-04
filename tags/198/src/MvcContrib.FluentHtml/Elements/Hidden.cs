using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public class Hidden : TextInput<Hidden>
	{
		public Hidden(string name) : base(HtmlInputType.Hidden, name) { }

		public Hidden(string name, MemberExpression forMember, IEnumerable<IMemberBehavior> behaviors)
			: base(HtmlInputType.Hidden, name, forMember, behaviors) { }
	}
}
