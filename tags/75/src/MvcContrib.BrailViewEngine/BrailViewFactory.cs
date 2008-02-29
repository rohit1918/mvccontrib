using System;
using System.Web.Mvc;
using MvcContrib.BrailViewEngine;

namespace MvcContrib.ViewFactories
{
	public class BrailViewFactory : IViewFactory
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

		public IView CreateView(ControllerContext controllerContext, string viewName, string masterName, object viewData)
		{
			string controller = controllerContext.RouteData.Values["controller"] as string;

			viewName = string.Concat(controller, "/", viewName);

			return _viewEngine.Process(controllerContext.HttpContext.Response.Output, viewName, masterName);
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
	}
}