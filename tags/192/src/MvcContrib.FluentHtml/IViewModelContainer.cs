using System.Collections.Generic;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml
{
	public interface IViewModelContainer<T> where T : class
	{
		T ViewModel { get; }
		IEnumerable<IMemberBehavior> MemberBehaviors { get; }
	}
}
