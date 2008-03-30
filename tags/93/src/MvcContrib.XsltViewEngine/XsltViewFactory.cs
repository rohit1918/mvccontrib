using System;
using System.Web.Mvc;
using MvcContrib.XsltViewEngine;

namespace MvcContrib.ViewFactories
{
	public class XsltViewFactory : IViewEngine	{
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

	    public void RenderView(ViewContext viewContext)
	    {
			//First check if the data is valid then start working.
			if (!(viewContext.ViewData is XsltViewData))
				throw new ArgumentException("the view data object should be of type XsltViewData");


			string controllerName = (string)viewContext.RouteData.Values["controller"];

			XsltTemplate viewTemplate = new XsltTemplate(_viewSourceLoader, controllerName, viewContext.ViewName);

			var view = new XsltView(viewTemplate, viewContext.ViewData as XsltViewData, string.Empty, viewContext.HttpContext);
	    	view.RenderView(viewContext);
	    }
	}
}
