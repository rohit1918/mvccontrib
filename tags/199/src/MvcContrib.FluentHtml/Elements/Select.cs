using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml.Elements
{
	public class Select : SelectBase<Select>
	{
		public Select(string name) : base(name) { }

		public Select(string name, MemberExpression forMember, IEnumerable<IMemberBehavior> behaviors)
			: base(name, forMember, behaviors) { }


		public virtual Select Selected(object selectedValue)
		{
			_selectedValues = new List<object> { selectedValue };
			return this;
		}
	}
}
