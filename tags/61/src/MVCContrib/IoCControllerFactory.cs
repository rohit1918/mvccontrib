using System;
using System.Web.Mvc;
using MvcContrib.Interfaces;
using MvcContrib.Services;

namespace MvcContrib.ControllerFactories
{
    public class IoCControllerFactory : IControllerFactory
    {
        private IDependencyResolver resolver = null;

        public IoCControllerFactory(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public IoCControllerFactory()
        {
            
        }

        public IController CreateController(RequestContext context, Type controllerType)
        {
            if(controllerType != null)
            {
                if(resolver != null)
                {
                    return resolver.GetImplementationOf<IController>(controllerType);
                }
                else
                {
                    return DependencyResolver.Resolver.GetImplementationOf<IController>(controllerType);
                }                    
            }
            else
                throw new ArgumentNullException("controllerType");
        }
    }
}