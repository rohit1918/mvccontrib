using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcContrib.ExtendedComponentController
{
	public interface IComponentControllerFactory
	{
		ComponentController CreateComponentController(Type type);
		ComponentController CreateComponentController<TController>() where TController : ComponentController;
		void DisposeComponentController(ComponentController controller);
	}
}
