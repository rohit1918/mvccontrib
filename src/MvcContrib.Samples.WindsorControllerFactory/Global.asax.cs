using System;
using System.Web;
using System.Web.Mvc;
using Castle.Core;
using Castle.Windsor;
using MVCContrib.ControllerFactories;
using MvcContrib.Samples.WindsorControllerFactory.Controllers;

namespace MvcContrib.Samples.WindsorControllerFactory
{
	public class Global : HttpApplication, IContainerAccessor
	{
		private static WindsorContainer _container;
		
		public static IWindsorContainer Container
		{
			get { return _container; }
		}

		IWindsorContainer IContainerAccessor.Container
		{
			get { return Container; }
		}

		protected void Application_Start(object sender, EventArgs e)
		{
			InitializeWindsor();
			AddRoutes();
		}

		/// <summary>
		/// Instantiate the container and add all Controllers that derive from 
		/// WindsorController to the container.  Also associate the Controller 
		/// with the WindsorContainer ControllerFactory.
		/// </summary>
		protected virtual void InitializeWindsor()
		{
			if (_container == null)
			{
				_container = new WindsorContainer();
				Type[] assemblyTypes = typeof(WindsorController).Assembly.GetTypes();
				foreach (Type type in assemblyTypes)
				{
					if(typeof(IController).IsAssignableFrom(type) )
					{
						_container.AddComponentWithLifestyle(type.Name, type, LifestyleType.Transient);
						ControllerBuilder.Current.SetControllerFactory(type, typeof(MvcContrib.Castle.WindsorControllerFactory));
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
