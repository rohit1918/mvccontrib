using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;

namespace MvcContrib.Castle
{
	/// <summary>
	/// Controller Factory class for instantiating controllers using the Windsor IoC container.
	/// </summary>
	public class WindsorControllerFactory : IControllerFactory
	{
		private IWindsorContainer _container;

		/// <summary>
		/// Creates a new instance of the <see cref="WindsorControllerFactory"/> class.
		/// </summary>
		/// <param name="container">The Windsor container instance to use when creating controllers.</param>
		public WindsorControllerFactory(IWindsorContainer container)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			_container = container;
		}

		public virtual IController CreateController(RequestContext context, string controllerName)
		{
			controllerName = controllerName.ToLower() + "controller";
			return (IController)_container.Resolve(controllerName);
		}

		public virtual void DisposeController(IController controller)
		{
			var disposable = controller as IDisposable;

			if (disposable != null)
			{
				disposable.Dispose();
			}

			_container.Release(controller);
		}
	}
}
