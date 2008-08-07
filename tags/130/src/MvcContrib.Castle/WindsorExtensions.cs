using Castle.Core;
using Castle.Windsor;
using System.Web.Mvc;
using System;
using System.Reflection;
namespace MvcContrib.Castle
{
	public static class WindsorExtensions
	{
		public static IWindsorContainer RegisterController<T>(this IWindsorContainer container) where T : IController
		{
			container.RegisterControllers(typeof(T));
			return container;
		}
		
		public static IWindsorContainer RegisterControllers(this IWindsorContainer container, params Type[] controllerTypes)
		{
			foreach(var type in controllerTypes)
			{
				if(ControllerFactories.DefaultControllerFactory.IsController(type))
				{
					container.AddComponentWithLifestyle(type.Name.ToLower(), type, LifestyleType.Transient);
				}
			}

			return container;
		}

		public static IWindsorContainer RegisterControllers(this IWindsorContainer container, params Assembly[] assemblies)
		{
			foreach(var assembly in assemblies)
			{
				container.RegisterControllers(assembly.GetExportedTypes());
			}
			return container;
		}
	}
}
