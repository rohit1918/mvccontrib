using Ninject.Core;

namespace NinjectControllerFactory.Domain
{
    public class Samurai
    {
        private readonly IWeapon _weapon;

        [Inject]
        public Samurai(IWeapon weapon)
        {
            _weapon = weapon;
        }

        public string Attack(string target)
        {
            return _weapon.Hit(target);
        }
    }
}
