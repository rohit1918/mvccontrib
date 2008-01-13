using MvcContrib.Interfaces;

namespace MvcContrib.Services
{
    public class DependencyResolver
    {
        private static IDependencyResolver resolver;

        public static void InitializeWith(IDependencyResolver resolver)
        {
            DependencyResolver.resolver = resolver;
        }

        public static IDependencyResolver Resolver
        {
            get { return resolver; }
        }
    }
}