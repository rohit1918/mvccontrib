using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml
{
	public class ModelViewMasterPage<T> : ViewMasterPage<T>, IViewModelContainer<T> where T : class
	{
		private readonly IEnumerable<IMemberBehavior> memberBehaviors = new List<IMemberBehavior>();

		public ModelViewMasterPage() { }

		public ModelViewMasterPage(params IMemberBehavior[] memberBehaviors)
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
