namespace NinjectControllerFactory.Domain
{
    public class Sword : IWeapon
    {
        public string Hit(string target)
        {
            return string.Format("Sliced {0} clean in half!", target);
        }
    }
}
