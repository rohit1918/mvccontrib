using System;
using System.Web.Mvc;
using StructureMap;

namespace MvcContrib.ControllerFactories
{
	public class StructureMapControllerFactory : IControllerFactory
	{
		IController IControllerFactory.CreateController(RequestContext context, Type controllerType)
		{
			return (IController)ObjectFactory.GetInstance(controllerType);
		}
	}
}