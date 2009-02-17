using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml
{
	public class ModelViewMasterPage<T> : ViewMasterPage<T>, IViewModelContainer<T> where T : class
	{
		protected readonly List<IMemberBehavior> memberBehaviors = new List<IMemberBehavior>();
		protected string htmlNamePrefix;

		public ModelViewMasterPage()
		{
			memberBehaviors.Add(new ValidationMemberBehavior(() => ViewData.ModelState));
		}

		public ModelViewMasterPage(params IMemberBehavior[] memberBehaviors) : this(null, memberBehaviors) { }

		public ModelViewMasterPage(string htmlNamePrefix, params IMemberBehavior[] memberBehaviors) : this()
		{
			this.htmlNamePrefix = htmlNamePrefix;
			if (memberBehaviors != null)
			{
				this.memberBehaviors.AddRange(memberBehaviors);
			}
		}

		public string HtmlNamePrefix
		{
			get { return htmlNamePrefix; }
			set { htmlNamePrefix = value; }
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
