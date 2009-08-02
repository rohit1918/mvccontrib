using System;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MvcContrib.UI.InputBuilder
{
    public class InputBuilder
    {
        public static Action<System.Web.Hosting.VirtualPathProvider> RegisterPathProvider = HostingEnvironment.RegisterVirtualPathProvider;

        public static void BootStrap()
        {
            VirtualPathProvider pathProvider = new AssemblyResourceProvider();
            
            RegisterPathProvider(pathProvider);            

            ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new InputBuilderViewEngine(new string[] { "{1}", "Shared" }));        		
        }
    }
}