using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.FluentHtml;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.UnitTests.FluentHtml.Fakes
{
	public class FakeViewModelContainer<T> : IViewModelContainer<T> where T : class
	{
		private ViewDataDictionary viewData = new ViewDataDictionary();
		private string htmlNamePrefix;

		public FakeViewModelContainer() { }

		public FakeViewModelContainer(string htmlNamePrefix)
		{
			this.htmlNamePrefix = htmlNamePrefix;
		}

		public T ViewModel
		{
			get { return viewData.Model as T; }
			set { viewData.Model = value; }
		}

		public IEnumerable<IBehaviorMarker> Behaviors
		{
			get { return new List<IBehaviorMarker>(); }
		}

		public string HtmlNamePrefix
		{
			get { return htmlNamePrefix; }
			set { htmlNamePrefix = value; }
		}

		public ViewDataDictionary ViewData
		{
			get { return viewData; }
			set { viewData = value; }
		}
	}
}
