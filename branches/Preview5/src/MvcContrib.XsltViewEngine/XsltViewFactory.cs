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
			if (!(viewContext.ViewData.Model is XsltViewData))
				throw new ArgumentException("the view data object should be of type XsltViewData");


			var controllerName = (string)viewContext.RouteData.Values["controller"];

			var viewTemplate = new XsltTemplate(_viewSourceLoader, controllerName, viewContext.ViewName);

			var view = new XsltView(viewTemplate, viewContext.ViewData.Model as XsltViewData, string.Empty, viewContext.HttpContext);
	    	view.RenderView(viewContext);
	    }

        #region IViewEngine Members

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName)
        {
            throw new NotImplementedException();
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
