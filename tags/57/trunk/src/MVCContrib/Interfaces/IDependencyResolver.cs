using System;
using System.Web.Mvc;

namespace MvcContrib.Interfaces
{
    public interface IDependencyResolver
    {
        Interface GetImplementationOf<Interface>();
        Interface GetImplementationOf<Interface>(Type type);
    }
}