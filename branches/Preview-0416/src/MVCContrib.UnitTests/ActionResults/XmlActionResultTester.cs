using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using MvcContrib.ActionResults;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using System.IO;

namespace MvcContrib.UnitTests.ActionResults
{
	[TestFixture]
	public class XmlResultTester
	{
		private MockRepository _mocks;
		private ControllerContext _controllerContext;
		private StringWriter _writer;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();

			_writer = new StringWriter();
			HttpRequestBase request = _mocks.DynamicMock<HttpRequestBase>();
			HttpContextBase context = _mocks.DynamicMock<HttpContextBase>();
			HttpResponseBase response = _mocks.DynamicMock<HttpResponseBase>();
			SetupResult.For(context.Request).Return(request);
			SetupResult.For(context.Response).Return(response);
			SetupResult.For(response.Output).Return(_writer);
			RouteData routeData = new RouteData();

			RequestContext requestContext = new RequestContext(context, routeData);

			_controllerContext = new ControllerContext(requestContext, _mocks.DynamicMock<IController>());
			Expect.Call(response.ContentType).PropertyBehavior();
			_mocks.ReplayAll();
		}

		[Test]
		public void Should_set_content_type()
		{
			XmlResult result = new XmlResult(new int[] {2, 3, 4});
			result.ExecuteResult(_controllerContext);
			Assert.AreEqual("text/xml", _controllerContext.HttpContext.Response.ContentType);
		}

		[Test]
		public void Should_serialise_xml()
		{
			XmlResult result = new XmlResult(new Person {Id = 5, Name = "Jeremy"});
			result.ExecuteResult(_controllerContext);

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(_writer.ToString());
			Assert.That(doc.SelectSingleNode("/Person/Name").InnerText, Is.EqualTo("Jeremy"));
			Assert.That(doc.SelectSingleNode("/Person/Id").InnerText, Is.EqualTo("5"));
		}

		public class Person
		{
			public string Name { get; set; }
			public int Id { get; set; }
		}
	}
}