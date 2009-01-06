using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml
{
	public class ModelViewMasterPage<T> : ViewMasterPage<T>, IViewModelContainer<T> where T : class
	{
		protected readonly IList<IMemberBehavior> memberBehaviors = new List<IMemberBehavior>();

		public ModelViewMasterPage() { }

		public ModelViewMasterPage(params IMemberBehavior[] memberBehaviors)
		{
			this.memberBehaviors = memberBehaviors;
		}

		public T ViewModel
		{
			get { return ViewData.Model as T; }
		}

		public IEnumerable<IMemberBehavior> MemberBehaviors
		{
			get { return memberBehaviors; }
		}

		public new ViewDataDictionary ViewData
		{
			get { return base.ViewData; }
			set { throw new NotImplementedException("ViewData from base class ViewMasterPage<T> is read-only."); }
		}
	}
}
