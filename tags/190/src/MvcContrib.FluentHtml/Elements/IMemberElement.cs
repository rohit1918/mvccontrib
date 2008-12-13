using System.Linq.Expressions;

namespace MvcContrib.FluentHtml.Elements
{
	public interface IMemberElement : IElement
	{
		MemberExpression ForMember { get; }
	}
}
