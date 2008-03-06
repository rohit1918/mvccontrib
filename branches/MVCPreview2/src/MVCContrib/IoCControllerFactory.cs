using System;
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
            this.resolver = resolver;
        }

        public IoCControllerFactory()
        {
            
        }

        protected override IController CreateController(RequestContext context, string controllerName)
        {
			//TODO: Don't rely on DefaultControllerFactory's GetControllerType
        	Type controllerType = base.GetControllerType(controllerName);

			if (controllerType != null)
			{
				if (resolver != null)
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

        protected override void DisposeController(IController controller)
        {
            //TODO: Release properly.
        }
    }
}
