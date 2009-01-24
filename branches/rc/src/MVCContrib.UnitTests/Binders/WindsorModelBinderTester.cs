using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using MvcContrib.Castle;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.Binders
{
	[TestFixture]
	public class WindsorModelBinderTester
	{
		private ModelBindingContext _context;

		[SetUp]
		public void Setup()
		{
			var httpContext = MvcMockHelpers.DynamicHttpContextBase();
			var controllerContext = new ControllerContext(httpContext, new RouteData(), MockRepository.GenerateStub<ControllerBase>());

		    _context = new ModelBindingContext();
            //controllerContext, MockRepository.GenerateStub<IValueProvider>(), typeof(object), "Test", null, new ModelStateDictionary(), null);
		}

		[Test]
		public void ShouldResolveTheCorrectBinder_WhenBinderExists()
		{
			IWindsorContainer container = new WindsorContainer();

			container.AddComponent<IModelBinder, TestModelBinder>("testmodelbinder");

			var binder = new WindsorModelBinder(container);

			var value = binder.BindModel(_context);

			Assert.That(value.Value, Is.EqualTo("TestResult"));
		}

		[Test]
		public void ShouldFallbackToDefaultBinder_WhenBinderDoesNotExist()
		{
			var container = new WindsorContainer();
			var fallbackBinder = MockRepository.GenerateMock<IModelBinder>();
			fallbackBinder.Expect(b => b.BindModel(_context))
				.Return(new ModelBinderResult("MockedResult"));

			var binder = new WindsorModelBinder(container, fallbackBinder);

			var value = binder.BindModel(_context);

			Assert.That(value.Value, Is.EqualTo("MockedResult"));
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void ShouldThrow_WhenComponentIsNotIModelBinder()
		{
			var container = new WindsorContainer();
			container.AddComponent<object>("testmodelbinder");

			var binder = new WindsorModelBinder(container);
			binder.BindModel(_context);
		}

		public class TestModelBinder : IModelBinder
		{
		    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		    {
		        return "TestResult";
		    }
		}
	}
}
