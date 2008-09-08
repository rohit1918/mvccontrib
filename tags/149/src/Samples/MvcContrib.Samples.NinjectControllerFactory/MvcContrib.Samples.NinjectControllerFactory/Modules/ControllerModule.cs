using System.Web.Mvc;
using MvcContrib.Samples.NinjectControllerFactory.Controllers;
using Ninject.Conditions;
using Ninject.Core;

namespace MvcContrib.Samples.NinjectControllerFactory.Modules
{
    public class ControllerModule : StandardModule
    {
        public override void Load()
        {
            Bind<IController>().To<HomeController>().Only(When.Context.Variable("controllerName").EqualTo("Home"));
        }
    }
}
