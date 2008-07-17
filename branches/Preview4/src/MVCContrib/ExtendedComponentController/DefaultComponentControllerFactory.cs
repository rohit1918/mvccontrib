using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcContrib.ExtendedComponentController
{
	public class DefaultComponentControllerFactory : IComponentControllerFactory
	{
		public ComponentController CreateComponentController(Type t)
		{
			ComponentController c = Activator.CreateInstance(t) as ComponentController;
			return c;
		}
		public ComponentController CreateComponentController<TController>() where TController : ComponentController
		{
			return CreateComponentController(typeof(TController));
		}

		public void DisposeComponentController(ComponentController controller)
		{

		}
	}
}
