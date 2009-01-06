using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml
{
	public class ModelViewPage<T> : ViewPage<T>, IViewModelContainer<T> where T : class
	{
		protected readonly IList<IMemberBehavior> memberBehaviors = new List<IMemberBehavior>();

		public ModelViewPage() {}

		public ModelViewPage(params IMemberBehavior[] memberBehaviors)
		{
			this.memberBehaviors = memberBehaviors;
		}

		public T ViewModel
		{
			get { return ViewData.Model; }
		}

		public IEnumerable<IMemberBehavior> MemberBehaviors
		{
			get { return memberBehaviors; }
		}
	}
}
