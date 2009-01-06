using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.FluentHtml;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.UnitTests.FluentHtml.Fakes
{
	public class FakeViewModelContainer : IViewModelContainer<FakeModel>
	{
		private ViewDataDictionary viewData = new ViewDataDictionary();

		public FakeModel ViewModel
		{
			get { return viewData.Model as FakeModel; }
			set { viewData.Model = value; }
		}

		public IEnumerable<IMemberBehavior> MemberBehaviors
		{
			get { return new List<IMemberBehavior>(); }
		}

		public ViewDataDictionary ViewData
		{
			get { return viewData; }
			set { viewData = value; }
		}
	}
}
