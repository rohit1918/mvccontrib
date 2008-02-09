using System;
using MvcContrib.Interfaces;
using MvcContrib.ObjectBuilder;

namespace MVCContrib.UnitTests.IoC
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
            return (Interface)_container.Get(typeof(Interface));
        }

        public Interface GetImplementationOf<Interface>(Type type)
        {
            return (Interface)_container.Get(type);
        }
    }
}