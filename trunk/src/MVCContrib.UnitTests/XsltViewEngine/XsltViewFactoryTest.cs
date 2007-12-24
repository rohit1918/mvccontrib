using System;
using System.IO;
using System.Web.Mvc;
using MVCContrib.UnitTests.XsltViewEngine.Helpers;
using MvcContrib.ViewFactories;
using MvcContrib.XsltViewEngine;
using NUnit.Framework;

namespace MVCContrib.UnitTests.XsltViewEngine
{
	[TestFixture]
	public class XsltViewFactoryTest : ViewTestBase
	{
		private const string controller = "MyController";
		private const string view = "MyView";
		private static readonly string basePath = Path.Combine(Environment.CurrentDirectory, "../../XsltViewEngine/Data");

		[Test, Category("Integration")]
		public void CreateView()
		{
			RouteData routeData = new RouteData();
			routeData.Values["controller"] = controller;
			Request.PhysicalApplicationPath = "http://testing/mycontroller/test";
			Identity.Name = "";
			Version version = new Version(1, 1);
			Browser.EcmaScriptVersion = version;
			Browser.Browser = "Firefox 2.0.11";
			Request.UserHostName = "::1";
			Request.RequestType = Request.HttpMethod = "GET";
			Request.QueryString["myQueryString"] = "myQueryStringValue";

			ControllerContext controllerContext = new ControllerContext(HttpContext, routeData, new Controller());

			IViewFactory viewFactory = new XsltViewFactory(basePath);

			IView viewObj = viewFactory.CreateView(controllerContext, view, string.Empty, new XsltViewData());

			Assert.IsNotNull(viewObj);
			Assert.IsTrue(viewObj is XsltView);
		}

		[Test, Category("Integration")]
		public void CreateView_DontSetTheAppPath()
		{
			RouteData routeData = new RouteData();
			routeData.Values["controller"] = controller;
			Request.PhysicalApplicationPath = basePath;
			Identity.Name = "";
			Version version = new Version(1, 1);
			Browser.EcmaScriptVersion = version;
			Browser.Browser = "Firefox 2.0.11";
			Request.UserHostName = "::1";
			Request.RequestType = Request.HttpMethod = "GET";
			Request.QueryString["myQueryString"] = "myQueryStringValue";

			ControllerContext controllerContext = new ControllerContext(HttpContext, routeData, new Controller());

			IViewFactory viewFactory = new XsltViewFactory();

			IView viewObj = viewFactory.CreateView(controllerContext, view, string.Empty, new XsltViewData());

			Assert.IsNotNull(viewObj);
			Assert.IsTrue(viewObj is XsltView);
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void ThrowExceptionWhenDataTypeIsInvalid()
		{
			RouteData routeData = new RouteData();
			routeData.Values["controller"] = controller;
			Request.PhysicalApplicationPath = "http://testing/mycontroller/test";
			Identity.Name = "";
			Version version = new Version(1, 1);
			Browser.EcmaScriptVersion = version;
			Browser.Browser = "Firefox 2.0.11";
			Request.UserHostName = "::1";
			Request.RequestType = Request.HttpMethod = "GET";
			Request.QueryString["myQueryString"] = "myQueryStringValue";

			ControllerContext controllerContext = new ControllerContext(HttpContext, routeData, new Controller());

			IViewFactory viewFactory = new XsltViewFactory(basePath);

			IView viewObj = viewFactory.CreateView(controllerContext, view, string.Empty, new Object());

			Assert.IsNotNull(viewObj);
			Assert.IsTrue(viewObj is XsltView);
		}
	}
}