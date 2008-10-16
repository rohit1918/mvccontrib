using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Binders;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.Binders
{
    [TestFixture,Ignore("Major changes to the binding in Beta will require refactoring of this approach")]
    public class SubControllerBinderTester
    {
        private static ControllerContext GetControllerContext()
        {
            var mockRequest = MockRepository.GenerateStub<HttpRequestBase>();
            mockRequest.Stub(r => r.Form).Return(new NameValueCollection()).Repeat.Any();
            var queryString = new NameValueCollection();
            queryString.Add("foo", "bar");
            mockRequest.Stub(r => r.QueryString).Return(queryString).Repeat.Any();

            var mockHttpContext = MockRepository.GenerateStub<HttpContextBase>();
            mockHttpContext.Stub(c => c.Request).Return(mockRequest).Repeat.Any();

            var routeData = new RouteData();
            return new ControllerContext(mockHttpContext, routeData,
                                         MockRepository.GenerateStub<ControllerBase>());
        }

        public class FooController : SubController
        {
        }

        [Test]
        public void ShouldCreateSubcontroller()
        {
            var binder = new SubControllerBinder();

            var bindingContext = new ModelBindingContext(GetControllerContext(),
                                                         binder,
                                                         typeof(FooController), "foo", null, new ModelStateDictionary(),
                                                         predicate);
            object value = binder.BindModel(bindingContext);
            //binder.GetValue(GetControllerContext(), "foo",typeof(FooController), new ModelStateDictionary());

            Assert.That(value, Is.InstanceOfType(typeof(FooController)));
        }

        private bool predicate(string obj)
        {
            if(obj=="ValueProvider")
                return true;
            else
            return false;
        }


        [Test]
        public void ShouldDeferToDefaultBinderIfNotSubcontroller()
        {
            var binder = new SubControllerBinder();

            object value = binder.BindModel(null);
            //object value = binder.GetValue(GetControllerContext(), "foo",
            //        typeof(string), new ModelStateDictionary());

            Assert.That(value, Is.EqualTo("bar"));
        }
    }
}