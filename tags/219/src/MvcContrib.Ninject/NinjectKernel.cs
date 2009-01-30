using Ninject.Core;

namespace MvcContrib.Ninject
{
    public class NinjectKernel
    {
        private static IKernel _kernel;

        public static void Initialize(params IModule[] modules)
        {
            _kernel = new StandardKernel(modules);
        }

        public static IKernel Kernel
        {
            get { return _kernel; }
        }
    }
}
