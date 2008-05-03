using System;
using MvcContrib.Interfaces;
using Ninject.Core;

namespace MvcContrib.Ninject
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public Interface GetImplementationOf<Interface>()
        {
            return GetImplementationOf<Interface>(typeof(Interface));
        }

        public Interface GetImplementationOf<Interface>(Type type)
        {
            return (Interface)GetImplementationOf(type);
        }

        public object GetImplementationOf(Type type)
        {
            try
            {
                return _kernel.Get(type);
            }
            catch(ActivationException)
            {
                return null;
            }
        }

    	public void DisposeImplementation(object instance)
    	{
    	}
    }
}
