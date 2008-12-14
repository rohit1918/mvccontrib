using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml.Tests.Fakes
{
	public class FakeViewModelContainer : IViewModelContainer<FakeModel>
	{
		private readonly ViewDataDictionary viewData = new ViewDataDictionary();

		public FakeModel ViewModel
		{
			get { return viewData.Model as FakeModel; }
			set { viewData.Model = value; }
		}

		public IEnumerable<IMemberBehavior> MemberBehaviors
		{
			get { return new List<IMemberBehavior>(); }
		}
	}
}
