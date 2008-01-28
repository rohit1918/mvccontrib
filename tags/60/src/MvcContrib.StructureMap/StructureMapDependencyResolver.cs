using System;
using MvcContrib.Interfaces;

namespace MvcContrib.UnitTests.ControllerFactories
{
    public class StructureMapDependencyResolver : IDependencyResolver
    {
        public TType GetImplementationOf<TType>()
        {
            return StructureMap.ObjectFactory.GetInstance<TType>();            
        }

        public Interface GetImplementationOf<Interface>(Type type)
        {
            return (Interface)StructureMap.ObjectFactory.GetInstance(type);
        }
    }
}