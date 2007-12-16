using System;
using System.Web.Mvc;
using StructureMap;

namespace MvcContrib.ControllerFactories
{
	public class StructureMapControllerFactory : IControllerFactory
	{
		IController IControllerFactory.CreateController(RequestContext context,
		                                                Type controllerType)
		{
			IController controller = GetController<IController>(controllerType);
			return controller;
		}

		private static T GetController<T>(Type controllerType)
			where T : IController
		{
			return (T)ObjectFactory.GetInstance(controllerType);
		}
	}
}