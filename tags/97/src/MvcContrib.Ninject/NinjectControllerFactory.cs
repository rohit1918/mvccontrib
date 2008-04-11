using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Ninject;
using Ninject.Core.Parameters;

namespace MvcContrib.ControllerFactories
{
    public class NinjectControllerFactory : IControllerFactory
    {
        public IController CreateController(RequestContext context, string controllerName)
        {  
            return NinjectKernel.Kernel.Get<IController>(
                With.Parameters.ContextVariable("controllerName", controllerName));
        }
 
        public void DisposeController(IController controller)
        {
            IDisposable disposable = controller as IDisposable;

            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
