using System;
using System.Web.Mvc;
using System.Web.Routing;
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
                    return DependencyResolver.GetImplementationOf<IController>(controllerType);
                }                    
            }
            else
                throw new ArgumentNullException("controllerType");
        }

        public IController CreateController(RequestContext context, string controllerName)
        {
            throw new NotImplementedException();
        }

        public void DisposeController(IController controller)
        {
            throw new NotImplementedException();
        }
    }
}
