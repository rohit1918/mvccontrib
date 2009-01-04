using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public class MultiSelect : SelectBase<MultiSelect>
	{
		public MultiSelect(string name) : base(name)
		{
			builder.MergeAttribute(HtmlAttribute.Multiple, HtmlAttribute.Multiple, true);
		}

		public MultiSelect(string name, MemberExpression forMember, IEnumerable<IMemberBehavior> behaviors)
			: base(name, forMember, behaviors)
		{
			builder.MergeAttribute(HtmlAttribute.Multiple, HtmlAttribute.Multiple, true);
		}

		public virtual MultiSelect Selected(IEnumerable selectedValues)
		{
			_selectedValues = selectedValues;
			return this;
		}
	}
}
