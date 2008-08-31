using System;
using System.Web.Mvc;
using MvcContrib.XsltViewEngine;

namespace MvcContrib.ViewFactories
{
	public class XsltViewFactory : System.Web.Mvc.VirtualPathProviderViewEngine, IViewEngine
    {
		private readonly IViewSourceLoader _viewSourceLoader;
        public XsltViewFactory(IViewSourceLoader loader)
        {
            _viewSourceLoader = loader;
        }
		public XsltViewFactory():this(new FileSystemViewSourceLoader())	
		{
            MasterLocationFormats = new string[0];

            ViewLocationFormats = new[]
            {
                "~/Views/{1}/{0}.xslt",   
                "~/Views/Shared/{0}.xslt"
            };

            PartialViewLocationFormats = ViewLocationFormats;
        }
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return CreateView(controllerContext, partialPath, null);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            
            
            if (!(controllerContext.Controller.ViewData.Model is XsltViewData))
                throw new ArgumentException("the view data object should be of type XsltViewData");

            var viewTemplate = new XsltTemplate(_viewSourceLoader, viewPath);

            var view = new XsltView(viewTemplate, controllerContext.Controller.ViewData.Model as XsltViewData, string.Empty, controllerContext.HttpContext);
            return view;
        }

        //public XsltViewFactory(IViewSourceLoader viewSourceLoader)
        //{
        //    if (viewSourceLoader == null) throw new ArgumentNullException("viewSourceLoader");

        //    _viewSourceLoader = viewSourceLoader;
        //}

        //public void RenderView(ViewContext viewContext)
        //{
        //    //First check if the data is valid then start working.
        //    if (!(viewContext.ViewData.Model is XsltViewData))
        //        throw new ArgumentException("the view data object should be of type XsltViewData");


        //    var controllerName = (string)viewContext.RouteData.Values["controller"];

        //    var viewTemplate = new XsltTemplate(_viewSourceLoader, controllerName, viewContext.ViewName);

        //    var view = new XsltView(viewTemplate, viewContext.ViewData.Model as XsltViewData, string.Empty, viewContext.HttpContext);
        //    view.RenderView(viewContext);
        //}

    }
}
