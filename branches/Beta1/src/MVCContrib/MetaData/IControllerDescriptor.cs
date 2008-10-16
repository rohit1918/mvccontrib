using System;
using System.Web.Mvc;

namespace MvcContrib.MetaData
{
	public interface IControllerDescriptor
	{
		ControllerMetaData GetMetaData(IController controller);
		ControllerMetaData GetMetaData(Type controllerType);
	}
}
