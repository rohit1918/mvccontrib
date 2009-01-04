using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public class Password : TextInput<Password>
	{
		public Password(string name) : base(HtmlInputType.Password, name) { }

		public Password(string name, MemberExpression forMember, IEnumerable<IMemberBehavior> behaviors)
			: base(HtmlInputType.Password, name, forMember, behaviors) { }
	}
}
