using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public class Input<T> : FormElement<T> where T : Input<T>, IElement
	{
		protected object elementValue;

		public Input(string type, string name) : base(HtmlTag.Input, name)
		{
			builder.MergeAttribute(HtmlAttribute.Type, type, true);
		}

		public Input(string type, string name, MemberExpression forMember, IEnumerable<IMemberBehavior> behaviors)
			: base(HtmlTag.Input, name, forMember, behaviors)
		{
			builder.MergeAttribute(HtmlAttribute.Type, type, true);
		}

		public virtual T Value(object value)
		{
			elementValue = value;
			return (T)this;
		}

		public virtual T Size(int value)
		{
			Attr(HtmlAttribute.Size, value);
			return (T)this;
		}

		public override string ToString()
		{
			Attr(HtmlAttribute.Value, elementValue);
			return base.ToString();
		}
	}
}
