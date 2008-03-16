using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Interfaces;
using MvcContrib.Services;

namespace MvcContrib.ExtendedComponentController
{
	public class IoCComponentControllerFactory : IComponentControllerFactory
	{
		private IDependencyResolver _resolver;

		public IoCComponentControllerFactory(IDependencyResolver resolver)
		{
			if(resolver == null)
				throw new ArgumentNullException("resolver");
			_resolver = resolver;
		}

		public IoCComponentControllerFactory()
		{
		}

		public ComponentController CreateComponentController(Type type)
		{
			return _resolver.GetImplementationOf(type) as ComponentController;
		}

		public ComponentController CreateComponentController<TController>() where TController : ComponentController
		{
			return _resolver.GetImplementationOf<TController>();
		}

		public void DisposeComponentController(ComponentController controller)
		{
			throw new NotImplementedException();
		}
	}
}
