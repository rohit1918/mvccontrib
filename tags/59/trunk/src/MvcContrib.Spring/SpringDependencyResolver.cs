using System;
using MvcContrib.Interfaces;
using Spring.Objects.Factory;

namespace MVCContrib.UnitTests.IoC
{
    public class SpringDependencyResolver : IDependencyResolver
    {
        private readonly IObjectFactory _factory;

        public SpringDependencyResolver(IObjectFactory factory)
        {
            _factory = factory;
        }

        public Interface GetImplementationOf<Interface>()
        {
            return (Interface)_factory.GetObject(typeof(Interface).Name);
        }

        public Interface GetImplementationOf<Interface>(Type type)
        {
            return (Interface)_factory.GetObject(type.Name);
        }
    }
}