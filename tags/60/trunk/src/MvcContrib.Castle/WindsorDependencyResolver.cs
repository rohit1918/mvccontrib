using System;
using Castle.Windsor;
using MvcContrib.Interfaces;

namespace MvcContrib.Castle
{
    public class WindsorDependencyResolver:IDependencyResolver
    {
        private IWindsorContainer _container;

        public WindsorDependencyResolver(IWindsorContainer container)
        {
            _container = container;
        }
        public WindsorDependencyResolver()
        {
            _container = new WindsorContainer(); 
        }

        public IWindsorContainer Container
        {
            get { return _container; }
        }

        public Interface GetImplementationOf<Interface>()
        {
            return _container.GetService<Interface>();
        }

        public Interface GetImplementationOf<Interface>(Type type)
        {
            return (Interface)_container.GetService(type);
        }
    }
}