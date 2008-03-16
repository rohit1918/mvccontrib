using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Attributes;
using NUnit.Framework;
using Rhino.Mocks;
using MvcContrib;
using System.IO;

namespace MvcContrib.UnitTests.MetaData
{
	[TestFixture]
	public class XmlReturnBinderTester
	{
		private MockRepository _mocks;

		private ControllerContext _controllerContext;



		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();

			HttpRequestBase request = _mocks.DynamicMock<HttpRequestBase>();
			HttpContextBase context = _mocks.DynamicMock<HttpContextBase>();
			HttpResponseBase response = _mocks.DynamicMock<HttpResponseBase>();
			TextWriter writer = _mocks.DynamicMock<TextWriter>();
			SetupResult.For(context.Request).Return(request);
			SetupResult.For(context.Response).Return(response);

			SetupResult.For(response.Output).Return(writer);
			RouteData routeData = new RouteData();
            
			RequestContext requestContext = new RequestContext(context, routeData);

			_controllerContext = new ControllerContext(requestContext, _mocks.DynamicMock<IController>());
			Expect.Call(response.ContentType).PropertyBehavior();
			_mocks.ReplayAll();
		}

		[Test]
		public void XmlBinderReturnsXml()
		{	
			XMLReturnBinder rb = new XMLReturnBinder();
			rb.Bind(_controllerContext.Controller, _controllerContext, typeof(int[]), new int[] {2, 3, 4});
			Assert.AreEqual("text/xml", _controllerContext.HttpContext.Response.ContentType);
		}

		[TearDown]
		public void TearDown()
		{
			
		}
	}
}
