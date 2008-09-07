using Ninject.Core;
using NinjectControllerFactory.Domain;

namespace MvcContrib.Samples.NinjectControllerFactory.Modules
{
    public class DomainModule : StandardModule
    {
        public override void Load()
        {
            Bind<IWeapon>().To<Sword>();
            Bind<Samurai>().ToSelf();
        }
    }
}
