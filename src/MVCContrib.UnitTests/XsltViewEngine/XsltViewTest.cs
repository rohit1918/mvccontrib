using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using MVCContrib.UnitTests.XsltViewEngine.Helpers;
using MvcContrib.ViewFactories;
using MvcContrib.XsltViewEngine;
using MvcContrib.XsltViewEngine.Messages;
using NUnit.Framework;

namespace MVCContrib.UnitTests.XsltViewEngine
{
	[TestFixture]
	public class XsltViewTest : ViewTestBase
	{
		private const string controller = "MyController";
		private const string view = "MyView";
		private static readonly string basePath = Path.Combine(Environment.CurrentDirectory, "../../XsltViewEngine/Data");

		[Test, Category("Integration")]
		public void RenderViewTest()
		{
			XsltViewData vData = new XsltViewData();
			string expectedSnippet = "<Root><MyElementID>1</MyElementID></Root>";
			XslDataSource dataSource = new XslDataSource(new MockXslDataSource(expectedSnippet));
			vData.DataSources.Add(dataSource);
			vData.Messages.Add(new Message(MessageType.Info, "This is a message"));
			vData.Messages.Add(new Message(MessageType.Info, "This is a message for a control", "controlID"));
			vData.Messages.Add(new InfoMessage("This is an info message"));
			vData.Messages.Add(new InfoMessage("This is an info message", "controlId2"));
			vData.Messages.Add(new InfoHtmlMessage("This is a html message"));
			vData.Messages.Add(new InfoHtmlMessage("This is a html message", "controlId3"));
			vData.Messages.Add(new ErrorHtmlMessage("This is an error html message"));
			vData.Messages.Add(new ErrorHtmlMessage("This is an error html message", "controlId4"));
			vData.Messages.Add(new ErrorMessage("This is an error message"));
			vData.Messages.Add(new ErrorMessage("This is an error message", "controlId4"));
			vData.Messages.Add(new AlertMessage("This is an alert message", "controlId4"));
			vData.Messages.Add(new AlertMessage("This is an alert message"));
			vData.Messages.Add(new AlertHtmlMessage("This is an alert html message", "controlId4"));
			vData.Messages.Add(new AlertHtmlMessage("This is an alert html message"));


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
			Request.Files = Activator.CreateInstance(typeof(HttpFileCollection), true) as HttpFileCollection;

			ControllerContext controllerContext = new ControllerContext(HttpContext, routeData, new Controller());

			IViewFactory viewFactory = new XsltViewFactory(basePath);

			IView viewObj = viewFactory.CreateView(controllerContext, view, string.Empty, vData);

			Assert.IsNotNull(viewObj);
			Assert.IsTrue(viewObj is XsltView);

			viewObj.RenderView(new ViewContext(controllerContext, vData, new TempDataDictionary(HttpContext)));

			string actual = ResponseOutput.ToString().Replace("\r\n", "");

			XmlDocument xDoc = LoadXmlDocument("ViewTest.xml");

			string expected = xDoc.OuterXml;

			Assert.AreEqual(expected, actual);
		}
	}
}