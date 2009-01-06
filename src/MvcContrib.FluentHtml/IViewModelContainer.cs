using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml
{
	public interface IViewModelContainer<T> : IViewDataContainer where T : class
	{
		T ViewModel { get; }
		IEnumerable<IMemberBehavior> MemberBehaviors { get; }
	}
}
