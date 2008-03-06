using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ObjectBuilder;

namespace MvcContrib.ControllerFactories
{
	public class ObjectBuilderControllerFactory : DefaultControllerFactory
	{
		protected override IController CreateController(RequestContext context, string controllerName)
		{
			//TODO: Don't rely on DefaultControllerFactory's implementation of GetControllerType
			Type controllerType = base.GetControllerType(controllerName);

			IDependencyContainer container = GetContainer(context);

			return (IController)container.Get(controllerType);
		}

		protected virtual IDependencyContainer GetContainer(RequestContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			IDependencyContainerAccessor dependencyContainerAccessor = context.HttpContext.ApplicationInstance as IDependencyContainerAccessor;
			if (dependencyContainerAccessor == null)
			{
				throw new InvalidOperationException(
					"You must extend the HttpApplication in your web project and implement the IContainerAccessor to properly expose your container instance");
			}

			IDependencyContainer container = dependencyContainerAccessor.Container;
			if (container == null)
			{
				throw new InvalidOperationException("The container seems to be unavailable in your HttpApplication subclass");
			}

			return container;
		}
	}
}