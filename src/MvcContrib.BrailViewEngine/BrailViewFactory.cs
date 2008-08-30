using System;
using System.Web.Mvc;
using MvcContrib.BrailViewEngine;

namespace MvcContrib.ViewFactories
{
	public class BrailViewFactory : IViewEngine
	{
		private readonly BooViewEngine _viewEngine;

		public BrailViewFactory()
			: this(DefaultViewEngine)
		{
		}

		public BrailViewFactory(BooViewEngine viewEngine)
		{
			if (viewEngine == null) throw new ArgumentNullException("viewEngine");

			_viewEngine = viewEngine;
		}

		public BooViewEngine ViewEngine
		{
			get { return _viewEngine; }
		}

		private static BooViewEngine _defaultViewEngine;
		private static BooViewEngine DefaultViewEngine
		{
			get
			{
				if( _defaultViewEngine == null )
				{
					_defaultViewEngine = new BooViewEngine();
					_defaultViewEngine.Initialize();
				}

				return _defaultViewEngine;
			}
		}

        //public void RenderView(ViewContext viewContext)
        //{
        //    var controller = viewContext.RouteData.Values["controller"] as string;

        //    string viewName = string.Concat(controller, "/", viewContext.ViewName);

        //    BrailBase view = _viewEngine.Process(viewContext.HttpContext.Response.Output, viewName, viewContext.MasterName);
        //    view.RenderView(viewContext);
        //}

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
