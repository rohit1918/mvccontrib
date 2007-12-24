using System;
using System.Web.Mvc;
using MvcContrib.XsltViewEngine;

namespace MvcContrib.ViewFactories
{
	public class XsltViewFactory : IViewFactory
	{
		private readonly IViewSourceLoader _viewSourceLoader;

		public XsltViewFactory()
			: this(new FileSystemViewSourceLoader())
		{
		}

		public XsltViewFactory(IViewSourceLoader viewSourceLoader)
		{
			if (viewSourceLoader == null) throw new ArgumentNullException("viewSourceLoader");

			_viewSourceLoader = viewSourceLoader;
		}

		#region IViewFactory Members

		public IView CreateView(ControllerContext controllerContext, string viewName, string masterName, object viewData)
		{
			//First check if the data is valid then start working.
			if(!(viewData is XsltViewData))
				throw new ArgumentException("the view data object should be of type XsltViewData");


			string controllerName = (string)controllerContext.RouteData.Values["controller"];

			XsltTemplate viewTemplate = new XsltTemplate(_viewSourceLoader, controllerName, viewName);

			return new XsltView(viewTemplate, viewData as XsltViewData, string.Empty, controllerContext.HttpContext);
		}

		#endregion
	}
}