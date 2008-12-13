using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public abstract class TextInput<T> : Input<T> where T : TextInput<T>
	{
		protected string _format;

		protected TextInput(string type, string name) : base(type, name) { }

		protected TextInput(string type, string name, MemberExpression forMember, IEnumerable<IMemberBehavior> behaviors)
			: base(type, name, forMember, behaviors) { }

		public virtual T MaxLength(int value)
		{
			Attr(HtmlAttribute.MaxLength, value);
			return (T)this;
		}

		public T Format(string format)
		{
			_format = format;
			return (T)this;
		}

		public override string ToString()
		{
			FormatValue();
			return base.ToString();
		}

		protected virtual void FormatValue()
		{
			if (!string.IsNullOrEmpty(_format))
			{
				if (_format.StartsWith("{0") && _format.EndsWith("}"))
				{
					elementValue = string.Format(_format, elementValue);
				}
				else
				{
					elementValue = string.Format("{0:" + _format + "}", elementValue);
				}
			}
		}
	}
}
