using System;
using MvcContrib.Interfaces;

namespace MvcContrib.ObjectBuilder
{
	public class ObjectFactoryDependencyResolver : IDependencyResolver
	{
		private readonly IDependencyContainer _container;

		public ObjectFactoryDependencyResolver(IDependencyContainer container)
		{
			_container = container;
		}

		public Interface GetImplementationOf<Interface>()
		{
			return (Interface)GetImplementationOf(typeof(Interface));
		}

		public Interface GetImplementationOf<Interface>(Type type)
		{
			return (Interface)GetImplementationOf(type);
		}

		public object GetImplementationOf(Type type)
		{
			try
			{
				return _container.Get(type);
			}
			catch(ArgumentException)
			{
				return null;
			}
		}

		public void DisposeImplementation(object instance)
		{
		}
	}
}
