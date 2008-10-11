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
	[TestFixture]
	public class SubControllerBinderTester
	{
		[Test]
		public void ShouldCreateSubcontroller()
		{
			var binder = new SubControllerBinder();

		    object value = binder.BindModel(null);
                //binder.GetValue(GetControllerContext(), "foo",typeof(FooController), new ModelStateDictionary());

			Assert.That(value, Is.InstanceOfType(typeof(FooController)));
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
	}
}