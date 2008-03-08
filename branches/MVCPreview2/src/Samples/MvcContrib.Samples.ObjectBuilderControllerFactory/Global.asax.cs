using System;
using System.Web;
using System.Web.Mvc;
using MvcContrib.ObjectBuilder;
using MvcContrib.Samples.ObjectBuilderControllerFactory.Controllers;

namespace MvcContrib.Samples.ObjectBuilderControllerFactory
{
	public class Global : HttpApplication, IDependencyContainerAccessor
	{
		private static DependencyContainer _container;

        public static IDependencyContainer Container
		{
			get { return _container; }
		}

				IDependencyContainer IDependencyContainerAccessor.Container
		{
			get { return Container; }
		}

		protected void Application_Start(object sender, EventArgs e)
		{
			InitializeContainer();
			AddRoutes();
		}

		/// <summary>
		/// Instantiate the container and add all Controllers that derive from 
        /// ObjectBuilderController to the container.  Also associate the Controller 
        /// with the ObjectBuilderContainer ControllerFactory.
		/// </summary>
        protected virtual void InitializeContainer()
		{
			if (_container == null)
			{
				_container = new DependencyContainer();
                Type[] assemblyTypes = typeof(ObjectBuilderController).Assembly.GetTypes();
				foreach (Type type in assemblyTypes)
				{
					if(typeof(IController).IsAssignableFrom(type) )
					{
						_container.RegisterTypeMapping(type, type);
                        ControllerBuilder.Current.SetControllerFactory(type, typeof(ControllerFactories.ObjectBuilderControllerFactory));
					}
				}
			}
		}

		protected virtual void AddRoutes()
		{
			RouteTable.Routes.Add(new Route
			{
				Url = "[controller]/[action]/[id]",
				Defaults = new { action = "Index", id = (string)null },
				RouteHandler = typeof(MvcRouteHandler)
			});

			RouteTable.Routes.Add(new Route
			{
				Url = "Default.aspx",
				Defaults = new { controller = "Home", action = "Index", id = (string)null },
				RouteHandler = typeof(MvcRouteHandler)
			});
		}
	}
}