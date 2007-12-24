using System;
using System.IO;
using System.Web.Mvc;
using MVCContrib.UnitTests.XsltViewEngine.Helpers;
using MvcContrib.ViewFactories;
using MvcContrib.XsltViewEngine;
using NUnit.Framework;
using Rhino.Mocks;

namespace MVCContrib.UnitTests.XsltViewEngine
{
	[TestFixture, Category("XsltViewEngine")]
	public class XsltViewFactoryTest : ViewTestBase
	{
		private const string controller = "MyController";
		private const string view = "MyView";
		private static readonly string BASE_PATH = Path.Combine(Environment.CurrentDirectory, @"..\..\XsltViewEngine\Data");
		private IViewSourceLoader _viewSourceLoader;

		[SetUp]
		public void SetUp()
		{
			_viewSourceLoader = mockRepository.CreateMock<IViewSourceLoader>();
			mockRepository.BackToRecord(_viewSourceLoader);
			SetupResult.For(_viewSourceLoader.HasView("MyController/MyView.xslt")).Return(true);
			SetupResult.For(_viewSourceLoader.GetViewSource("MyController/MyView.xslt")).Return(new XsltViewSource());
			mockRepository.Replay(_viewSourceLoader);
		}

		[Test]
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

			IViewFactory viewFactory = new XsltViewFactory(_viewSourceLoader);

			IView viewObj = viewFactory.CreateView(controllerContext, view, string.Empty, new XsltViewData());

			Assert.IsNotNull(viewObj);
			Assert.IsTrue(viewObj is XsltView);
		}

		[Test]
		public void CreateView_DontSetTheAppPath()
		{
			RouteData routeData = new RouteData();
			routeData.Values["controller"] = controller;
			Request.PhysicalApplicationPath = BASE_PATH;
			Identity.Name = "";
			Version version = new Version(1, 1);
			Browser.EcmaScriptVersion = version;
			Browser.Browser = "Firefox 2.0.11";
			Request.UserHostName = "::1";
			Request.RequestType = Request.HttpMethod = "GET";
			Request.QueryString["myQueryString"] = "myQueryStringValue";

			ControllerContext controllerContext = new ControllerContext(HttpContext, routeData, new Controller());

			IViewFactory viewFactory = new XsltViewFactory(_viewSourceLoader);

			IView viewObj = viewFactory.CreateView(controllerContext, view, string.Empty, new XsltViewData());

			Assert.IsNotNull(viewObj);
			Assert.IsTrue(viewObj is XsltView);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void XsltViewFactory_DependsOn_ViewSourceLoader()
		{
			new XsltViewFactory();
			new XsltViewFactory(null);
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

			IViewFactory viewFactory = new XsltViewFactory(_viewSourceLoader);

			IView viewObj = viewFactory.CreateView(controllerContext, view, string.Empty, new Object());

			Assert.IsNotNull(viewObj);
			Assert.IsTrue(viewObj is XsltView);
		}
	}
}