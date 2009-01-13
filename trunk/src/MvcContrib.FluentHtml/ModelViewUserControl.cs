using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml
{
	public class ModelViewUserControl<T> : ViewUserControl<T>, IViewModelContainer<T> where T : class
	{
		protected readonly List<IMemberBehavior> memberBehaviors = new List<IMemberBehavior>();
		protected string htmlNamePrefix;

		public ModelViewUserControl()
		{
			memberBehaviors.Add(new ValidationMemberBehavior(() => ViewData.ModelState));
		}

		public ModelViewUserControl(params IMemberBehavior[] memberBehaviors) : this(null, memberBehaviors) { }

		public ModelViewUserControl(string htmlNamePrefix, params IMemberBehavior[] memberBehaviors) : this()
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
			get { return ViewData.Model; }
		}

		public IEnumerable<IMemberBehavior> MemberBehaviors
		{
			get { return memberBehaviors; }
		}
	}
}
