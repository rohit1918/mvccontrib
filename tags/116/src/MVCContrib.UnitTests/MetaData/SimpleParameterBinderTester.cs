using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.MetaData;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.MetaData
{
	[TestFixture]
	public class SimpleParameterBinderTester
	{
		private MockRepository _mocks;
		private ControllerContext _controllerContext;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();

			HttpRequestBase request = _mocks.DynamicMock<HttpRequestBase>();
			SetupResult.For(request["test"]).Return("testValue");
			SetupResult.For(request["keyWithNullValue"]).Return(null);

			HttpContextBase context = _mocks.DynamicMock<HttpContextBase>();
			SetupResult.For(context.Request).Return(request);

			RouteData routeData = new RouteData();
			routeData.Values.Add("key", "value");

			RequestContext requestContext = new RequestContext(context, routeData);
			_controllerContext = new ControllerContext(requestContext, _mocks.DynamicMock<IController>());

			_mocks.ReplayAll();
		}

		[Test]
		public void CanDeserializeFromRequest()
		{
			SimpleParameterBinder attr = new SimpleParameterBinder();
			string value = attr.Bind(typeof(string), "test", _controllerContext) as string;

			Assert.IsNotNull(value);
			Assert.AreEqual("testValue", value);
		}

		[Test]
		public void CanDeserializeFromRouteData()
		{
			SimpleParameterBinder attr = new SimpleParameterBinder();
			string value = attr.Bind(typeof(string), "key", _controllerContext) as string;

			Assert.IsNotNull(value);
			Assert.AreEqual("value", value);
		}

		[Test]
		public void ShouldNotThrowIfRouteDataValueIsNull()
		{
			_controllerContext.RouteData.Values.Add("keyWithNullValue", null);

			SimpleParameterBinder attr = new SimpleParameterBinder();
			string value = attr.Bind(typeof(string), "keyWithNullValue", _controllerContext) as string;

			Assert.IsNull(value);
		}
	}
}
