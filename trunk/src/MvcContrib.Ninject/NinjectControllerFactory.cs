using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject.Core.Parameters;

namespace MvcContrib.Ninject
{
    public class NinjectControllerFactory : IControllerFactory
    {
        public IController CreateController(RequestContext context, string controllerName)
        {  
            return NinjectKernel.Kernel.Get<IController>(
                With.Parameters.ContextVariable("controllerName", controllerName));
        }

        public void ReleaseController(IController controller)
        {
			var disposable = controller as IDisposable;

			if (disposable != null) 
			{
				disposable.Dispose();
			}
        }
    }
}
