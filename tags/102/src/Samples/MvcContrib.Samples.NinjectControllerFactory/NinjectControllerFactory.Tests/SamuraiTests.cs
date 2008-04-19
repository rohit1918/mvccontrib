using Ninject.Core;
using NinjectControllerFactory.Domain;
using NUnit.Framework;

namespace NinjectControllerFactory.Tests
{
    [TestFixture]
    public class SamuraiTests
    {
        [Test]
        public void ShouldChopTargetWithSword()
        {
            Samurai samurai = new Samurai(new Sword());
            string attackResult = samurai.Attack("the evildoers");
            Assert.AreEqual("Sliced the evildoers clean in half!", attackResult);
        }

        [Test]
        public void ShouldChopTargetWithSwordUsingNinject()
        {
            IKernel kernel = new StandardKernel(new TestModule());
            Samurai warrior = kernel.Get<Samurai>();
            string attackResult = warrior.Attack("the evildoers");
            Assert.AreEqual("Sliced the evildoers clean in half!", attackResult);
        }

        private class TestModule : StandardModule
        {
            public override void Load()
            {
                Bind<IWeapon>().To<Sword>();
                Bind<Samurai>().ToSelf();
            }
        }
    }
}
