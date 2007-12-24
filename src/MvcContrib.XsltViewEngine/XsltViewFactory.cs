using System;
using System.Web.Mvc;
using MvcContrib.XsltViewEngine;

namespace MvcContrib.ViewFactories
{
	public class XsltViewFactory : IViewFactory
	{
		private readonly string appPath;

		public XsltViewFactory()
		{
		}

		public XsltViewFactory(string appPath) : this()
		{
			this.appPath = appPath;
		}

		#region IViewFactory Members

		public IView CreateView(ControllerContext controllerContext, string viewName, string masterName, object viewData)
		{
			//First check if the data is valid then start working.
			if(!(viewData is XsltViewData))
				throw new ArgumentException("the view data object should be of type XsltViewData");


			string controllerName = (string)controllerContext.RouteData.Values["controller"];

			XsltTemplate viewTemplate;
			if(!string.IsNullOrEmpty(appPath))
				viewTemplate = new XsltTemplate(appPath, controllerName, viewName);
			else
				viewTemplate = new XsltTemplate(controllerContext, viewName);

			return new XsltView(viewTemplate, viewData as XsltViewData, string.Empty, controllerContext.HttpContext);
		}

		#endregion
	}
}