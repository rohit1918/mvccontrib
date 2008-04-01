using System;
using System.Reflection;
using System.Collections;
using System.Security;
using System.Web.Mvc;

namespace MvcContrib.ControllerFactories
{
	public class DefaultControllerFactory : System.Web.Mvc.DefaultControllerFactory
	{
		private static Hashtable _typeCache;
		private static readonly object _lock = new object();

		public static bool IsController(Type type)
		{
			return
				type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
				&& type != typeof(IController)
				&& type != typeof(Controller)
				&& typeof(IController).IsAssignableFrom(type);
		}

		protected override Type GetControllerType(string controllerName)
		{
			EnsureCacheInitialised();
			return (Type)_typeCache[controllerName + "Controller"];
		}

		protected void EnsureCacheInitialised()
		{
			if (_typeCache == null)
			{
				lock (_lock)
				{
					if (_typeCache == null)
					{
						Hashtable cache = new Hashtable(StringComparer.OrdinalIgnoreCase);

						Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

						foreach (Assembly assembly in assemblies)
						{
							foreach (Type type in assembly.GetTypes())
							{
								if (IsController(type))
								{
									if (cache.ContainsKey(type.Name))
									{
										throw new Exception(string.Format("Multiple controllers found with the name '{0}'", type.Name));
									}

									cache[type.Name] = type;
								}
							}
						}
						_typeCache = cache;
					}
				}
			}
		}
	}
}