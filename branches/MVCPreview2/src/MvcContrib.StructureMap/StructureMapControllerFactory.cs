using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace MvcContrib.ControllerFactories
{
	public class StructureMapControllerFactory : IControllerFactory
	{
		IController CreateController(RequestContext context, Type controllerType)
		{
			return (IController)ObjectFactory.GetInstance(controllerType);
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
