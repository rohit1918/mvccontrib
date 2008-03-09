using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Interfaces;
using MvcContrib.Services;

namespace MvcContrib.ControllerFactories
{
	public class IoCControllerFactory : DefaultControllerFactory
	{
		private IDependencyResolver resolver = null;

		public IoCControllerFactory(IDependencyResolver resolver)
		{
			if(resolver == null)
				throw new ArgumentNullException("resolver");
			this.resolver = resolver;
		}

		public IoCControllerFactory()
		{
		}

		protected override IController CreateController(RequestContext context, string controllerName)
		{
			if(controllerName == null)
			{
				throw new ArgumentNullException("controllerName");
			}

			Type controllerType = GetControllerType(controllerName);

			if(controllerType != null)
			{
				if(resolver != null)
				{
					return resolver.GetImplementationOf<IController>(controllerType);
				}
				else
				{
					return DependencyResolver.GetImplementationOf<IController>(controllerType);
				}
			}
			else
				throw new Exception(string.Format("Could not find a type for the controller name '{0}'", controllerName));
		}
	}
}