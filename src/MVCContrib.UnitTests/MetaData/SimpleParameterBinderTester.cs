using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.MetaData;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
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

			var request = _mocks.DynamicMock<HttpRequestBase>();
			SetupResult.For(request["test"]).Return("testValue");
			SetupResult.For(request["keyWithNullValue"]).Return(null);

			var context = _mocks.DynamicMock<HttpContextBase>();
			SetupResult.For(context.Request).Return(request);

			var routeData = new RouteData();
			routeData.Values.Add("key", "value");

			var requestContext = new RequestContext(context, routeData);
			_controllerContext = new ControllerContext(requestContext, _mocks.DynamicMock<ControllerBase>());

			_mocks.ReplayAll();
		}

		[Test]
		public void CanDeserializeFromRequest()
		{
			var attr = new SimpleParameterBinder();
			var value = attr.GetValue(_controllerContext, "test", typeof(string), null) as string;

			Assert.IsNotNull(value);
			Assert.AreEqual("testValue", value);
		}

		[Test]
		public void CanDeserializeFromRouteData()
		{
			var attr = new SimpleParameterBinder();
			var value = attr.GetValue(_controllerContext, "key", typeof(string), null) as string;

			Assert.IsNotNull(value);
			Assert.AreEqual("value", value);
		}

		[Test]
		public void ShouldNotThrowIfRouteDataValueIsNull()
		{
			_controllerContext.RouteData.Values.Add("keyWithNullValue", null);

			var attr = new SimpleParameterBinder();
			var value = attr.GetValue(_controllerContext, "keyWithNullValue", typeof(string), null) as string;

			Assert.IsNull(value);
		}

		[Test]
		public void CanLoadSimpleObjectDirectlyFromRouteData()
		{
			_controllerContext.RouteData.Values.Add("foo", 1);
			var attr = new SimpleParameterBinder();
			var value = attr.GetValue(_controllerContext, "foo", typeof(int), null );
			Assert.That(value, Is.TypeOf(typeof(int)));
			Assert.That(value, Is.EqualTo(1));
		}

		[Test]
		public void CanLoadComplexObjectDirectlyFromRouteData()
		{
			_controllerContext.RouteData.Values.Add("foo", new SimpleParameterBinderTestObject { Name = "Foo" });
			var attr = new SimpleParameterBinder();
			var value = attr.GetValue(_controllerContext, "foo", typeof(SimpleParameterBinderTestObject), null) as SimpleParameterBinderTestObject;
			Assert.That(value, Is.Not.Null);
			Assert.That(((SimpleParameterBinderTestObject)value).Name, Is.EqualTo("Foo"));
		}

		private class SimpleParameterBinderTestObject
		{
			public string Name { get; set; }	
		}
	}
}
