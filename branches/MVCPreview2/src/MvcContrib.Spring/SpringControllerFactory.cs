using System;
using System.Web.Mvc;
using System.Web.Routing;
using Spring.Context;
using Spring.Objects.Factory;

namespace MvcContrib.Spring
{
	/// <summary>
	/// Controller Factory implementation for Spring.net
	/// </summary>
	public class SpringControllerFactory : IControllerFactory
	{
		private static IObjectFactory _objectFactory = null;

		public IController CreateController(RequestContext context, Type controllerType)
		{
			if(controllerType == null)
			{
				throw new ArgumentException("controllerType parameter cannot be null.");
			}

			if(_objectFactory != null)
			{
				try
				{
					return (IController)_objectFactory.GetObject(controllerType.Name);
				}
				catch(Exception e)
				{
					throw new ArgumentException("Failed creating instance of: " +
					                            controllerType.Name + " using spring.net object factory", e);
				}
			}
			else
			{
				throw new ArgumentException("CreateController has been called before Configure.");
			}
		}

		/// <summary>
		/// Configures the controller factory to use the 
		/// given spring.net IObjectFactory for controller lookup.
		/// If you call Configure multiple times, the last call will prevail.
		/// </summary>
		/// <param name="objectFactory">IObjectFactory instance to use for lookups.</param>
		public static void Configure(IObjectFactory objectFactory)
		{
			_objectFactory = objectFactory;
		}

		/// <summary>
		/// Configures the controller factory to use the
		/// given spring.net IApplicationContext for controller
		/// lookup.  If you call Configure multiple times, the 
		/// last call will prevail.
		/// </summary>
		/// <param name="ctx"></param>
		public static void Configure(IApplicationContext ctx)
		{
			_objectFactory = ctx;
		}

	    public IController CreateController(RequestContext context, string controllerName)
	    {
            if(_objectFactory==null)
                throw new ArgumentException("ObjectFactory");
	    	controllerName = controllerName + "Controller";
	    	Type controllerType = _objectFactory.GetType(controllerName);
            if(controllerType==null)
                throw new ArgumentException("controllerName");
	    	return CreateController(context, controllerType);
	    }

	    public void DisposeController(IController controller)
	    {
	        throw new NotImplementedException();
	    }
	}
}
