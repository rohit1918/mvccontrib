using Castle.Core;
using MvcContrib.UnitTests.Castle;
using NUnit.Framework;
using Castle.Windsor;
using MvcContrib.Castle;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.IoC
{
	[TestFixture]
	public class WindsorExtensionTester
	{

		public class WindsorExtensionTestBase
		{
			protected IWindsorContainer _container;

			[SetUp]
			public virtual void Setup()
			{
				_container = new WindsorContainer();
			}			
		}

		[TestFixture]
		public class When_RegisterController_generic_is_invoked : WindsorExtensionTestBase
		{
			public override void Setup()
			{
				base.Setup();
				_container.RegisterController<WindsorControllerFactoryTester.WindsorSimpleController>();
			}

			[Test]
			public void Then_the_type_should_be_registered()
			{
				Assert.That(_container.Kernel.HasComponent("windsorsimplecontroller"));
				Assert.That(_container.Kernel.GetHandler("windsorsimplecontroller").ComponentModel.Implementation, Is.EqualTo(typeof(WindsorControllerFactoryTester.WindsorSimpleController)));
			}

			[Test]
			public void Then_the_lifestyle_should_be_transient()
			{
				Assert.That(_container.Kernel.GetHandler("windsorsimplecontroller").ComponentModel.LifestyleType, Is.EqualTo(LifestyleType.Transient));
			}
		}

		[TestFixture]
		public class When_RegisterControllers_is_invoked_with_types : WindsorExtensionTestBase
		{
			public override void Setup()
			{
				base.Setup();
				_container.RegisterControllers(typeof(WindsorControllerFactoryTester.WindsorSimpleController));
			}

			[Test]
			public void Then_the_type_should_be_registered()
			{
				Assert.That(_container.Kernel.HasComponent("windsorsimplecontroller"));
				Assert.That(_container.Kernel.GetHandler("windsorsimplecontroller").ComponentModel.Implementation, Is.EqualTo(typeof(WindsorControllerFactoryTester.WindsorSimpleController)));
			}

			[Test]
			public void Then_the_lifestyle_should_be_transient()
			{
				Assert.That(_container.Kernel.GetHandler("windsorsimplecontroller").ComponentModel.LifestyleType, Is.EqualTo(LifestyleType.Transient));
			}
		}

		[TestFixture]
		public class When_RegisterControllers_is_invoked_with_assemblies : WindsorExtensionTestBase
		{
			public override void Setup()
			{
				base.Setup();
				_container.RegisterControllers(typeof(WindsorExtensionTester).Assembly);
			}

			[Test]
			public void Then_all_controllers_in_the_assembly_should_be_registered()
			{
				Assert.That(_container.Kernel.HasComponent("ioctestcontroller"));
				Assert.That(_container.Kernel.GetHandler("ioctestcontroller").ComponentModel.Implementation, Is.EqualTo(typeof(ControllerFactories.IoCControllerFactoryTester.IocTestController)));

				Assert.That(_container.Kernel.HasComponent("windsorsimplecontroller"));
				Assert.That(_container.Kernel.GetHandler("windsorsimplecontroller").ComponentModel.Implementation, Is.EqualTo(typeof(WindsorControllerFactoryTester.WindsorSimpleController)));
				//etc
			}

			[Test]
			public void Then_lifestyles_should_be_set_to_transient()
			{
				Assert.That(_container.Kernel.GetHandler("windsorsimplecontroller").ComponentModel.LifestyleType, Is.EqualTo(LifestyleType.Transient));
				Assert.That(_container.Kernel.GetHandler("ioctestcontroller").ComponentModel.LifestyleType, Is.EqualTo(LifestyleType.Transient));
			}
		}
	}
}