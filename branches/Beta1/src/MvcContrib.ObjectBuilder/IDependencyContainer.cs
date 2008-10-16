using System;

namespace MvcContrib.ObjectBuilder
{
    public interface IDependencyContainer
    {
        object Get(Type tBuild);
        void RegisterTypeMapping<TRequested, TToBuild>();
    }
}