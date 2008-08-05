using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Filters;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using System.Collections.Generic;

namespace MvcContrib.UnitTests.ConventionController
{
    [TestFixture]
    public class ConventionRescueTester
    {
        private RescueViewEngine _viewEngine;
        private ControllerContext _controllerContext;
        private MockRepository _mocks;
        private BaseConventionRescueTestController _controller;

        [SetUp]
        public void Setup()
        {
            _mocks = new MockRepository();
            _viewEngine = new RescueViewEngine();
            SetupController(new ConventionRescueTestController());
        }

        private void SetupController(BaseConventionRescueTestController controller)
        {
            _controller = controller;
            _controller.ViewEngine = _viewEngine;
            _controllerContext = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), controller);
            _controller.ControllerContext = _controllerContext;
        }

        [Test]
        public void When_PerformRescue_is_invoked_with_no_matching_view_the_default_rescue_should_be_rendered()
        {
            var rescue =
                new CustomViewExists_ConventionRescueAttribute("TestRescue");

        	var context = new ExceptionContext(_controllerContext, new Exception());
			rescue.OnException(context);

        	Assert.That(context.ExceptionHandled);
            Assert.That(_viewEngine.ViewContext.ViewName, Is.EqualTo("Rescues/TestRescue"));
        }

        [Test]
        public void When_PerformRescue_is_invoked_with_matching_view_it_should_be_rendered()
        {
            var rescue =
                new CustomViewExists_ConventionRescueAttribute("TestRescue");
            rescue.ExistingRescues.Add("ConventionRescueTestException");

			var context = new ExceptionContext(_controllerContext, new ConventionRescueTestException());
			rescue.OnException(context);
			Assert.That(context.ExceptionHandled);
            Assert.That(_viewEngine.ViewContext.ViewName, Is.EqualTo("Rescues/ConventionRescueTestException"));
        }

        [Test]
        public void When_PerformRescue_is_invoked_with_matching_view_and_AutoLocate_off_it_should_not_be_rendered()
        {
            var rescue =
                new CustomViewExists_ConventionRescueAttribute("TestRescue");
            rescue.ExistingRescues.Add("ConventionRescueTestException");
            rescue.AutoLocate = false;

			var context = new ExceptionContext(_controllerContext, new ConventionRescueTestException());
			rescue.OnException(context);
			Assert.That(context.ExceptionHandled);
            Assert.That(_viewEngine.ViewContext.ViewName, Is.EqualTo("Rescues/TestRescue"));
        }


        [Test]
        public void When_PerformRescue_exact_exception_executed_first()
        {
            var rescue =
               new CustomViewExists_ConventionRescueAttribute("TestRescue");

            rescue.ExistingRescues.Add("InheritedConventionRescueTestException");
            rescue.ExistingRescues.Add("ConventionRescueTestException");

			var context = new ExceptionContext(_controllerContext, new InheritedConventionRescueTestException());
			rescue.OnException(context);

			Assert.That(context.ExceptionHandled);
            Assert.That(_viewEngine.ViewContext.ViewName, Is.EqualTo("Rescues/InheritedConventionRescueTestException"));

			context = new ExceptionContext(_controllerContext, new ConventionRescueTestException());
			rescue.OnException(context);

			Assert.That(context.ExceptionHandled);
            Assert.That(_viewEngine.ViewContext.ViewName, Is.EqualTo("Rescues/ConventionRescueTestException"));

            rescue = new CustomViewExists_ConventionRescueAttribute("TestRescue", typeof(ConventionRescueTestException));
            rescue.ExistingRescues.Add("ConventionRescueTestException");
            context = new ExceptionContext(_controllerContext, new InheritedConventionRescueTestException());
			rescue.OnException(context);

			Assert.That(context.ExceptionHandled);
            Assert.That(_viewEngine.ViewContext.ViewName, Is.EqualTo("Rescues/ConventionRescueTestException"));

        }

        internal class CustomViewExists_ConventionRescueAttribute : RescueAttribute
        {
            public CustomViewExists_ConventionRescueAttribute(string view) : base(view) { }

            public CustomViewExists_ConventionRescueAttribute(string view, params Type[] exceptionTypes) : base(view, exceptionTypes) { }

            private List<string> existingRescues = new List<string>();
            public List<string> ExistingRescues
            {
                get { return existingRescues; }
                set { existingRescues = value; }
            }

            protected override bool ViewExists(Type exceptionType, ControllerContext controllerContext)
            {
                return ExistingRescues.Contains(exceptionType.Name);
            }

        }


        public class RescueViewEngine : IViewEngine
        {
            public ViewContext ViewContext { get; set; }

            public void RenderView(ViewContext viewContext)
            {
                ViewContext = viewContext;
            }
        }

        internal class ConventionRescueTestController : BaseConventionRescueTestController
        {
            
        }

        internal abstract class BaseConventionRescueTestController : Controller
        {
            public string ViewRendered
            {
                get;
                private set;
            }

            public void InvokeActionPublic(string actionName)
            {
                ControllerContext.RouteData.Values.Add("action", actionName);
                Execute(ControllerContext);
            }
        }

        internal class ConventionRescueTestException : Exception
        {
        }

        internal class InheritedConventionRescueTestException : ConventionRescueTestException
        {
        }
    }
}
