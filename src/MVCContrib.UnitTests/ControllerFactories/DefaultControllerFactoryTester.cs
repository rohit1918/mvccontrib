using System.Web;
using System.Web.Mvc;
using MvcContrib.UnitTests.ControllerFactories.IoCControllerFactoryTester;
using NUnit.Framework;
using Rhino.Mocks;
using DefaultControllerFactory=MvcContrib.ControllerFactories.DefaultControllerFactory;
using NUnit.Framework.SyntaxHelpers;
using System.Web.Routing;
namespace MvcContrib.UnitTests.ControllerFactories
{
	[TestFixture]
	public class DefaultControllerFactoryTester
	{
		public class DefaultControllerFactoryBaseTest : SpecBase
		{
			protected IControllerFactory _factory;
			protected RequestContext _context;

			protected override void BeforeEachSpec()
			{
				_factory = new DefaultControllerFactory();
				_context = new RequestContext(MockRepository.GenerateStub<HttpContextBase>(), new RouteData());
			}

			protected override void AfterEachSpec()
			{
			}
		}

		[TestFixture]
		public class When_CreateController_is_called : DefaultControllerFactoryBaseTest
		{
			[Test]
			public void Then_the_controller_should_be_created()
			{
				IController controller = _factory.CreateController(_context, "IocTest");
				Assert.That(controller, Is.Not.Null);
				Assert.That(controller, Is.InstanceOfType(typeof(IocTestController)));
			}

			[Test]
			public void Then_the_controller_should_be_created_case_insensitive()
			{
				IController controller = _factory.CreateController(_context, "ioctest");
				Assert.That(controller, Is.Not.Null);
				Assert.That(controller, Is.InstanceOfType(typeof(IocTestController)));
			}
		}
	}
}
