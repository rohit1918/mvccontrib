using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using MvcContrib.UnitTests.XsltViewEngine.Helpers;
using MvcContrib.ViewFactories;
using MvcContrib.XsltViewEngine;
using MvcContrib.XsltViewEngine.Messages;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.XsltViewEngine
{
	[TestFixture, Category("XsltViewEngine")]
	public class XsltViewTest : ViewTestBase
	{
		private const string controller = "MyController";
		private const string view = "MyView";
		private Controller _fakeController;

		private IViewSourceLoader _viewSourceLoader;

		public override void SetUp()
		{
			base.SetUp();
			_viewSourceLoader = mockRepository.CreateMock<IViewSourceLoader>();
			SetupResult.For(_viewSourceLoader.HasView("MyController/MyView.xslt")).Return(true);
			SetupResult.For(_viewSourceLoader.GetViewSource("MyController/MyView.xslt")).Return(new XsltViewSource());
			mockRepository.Replay(_viewSourceLoader);
			_fakeController = mockRepository.CreateMock<Controller>();
			mockRepository.Replay(_fakeController);
		}

		[Test]
		public void RenderViewTest()
		{
			var vData = new XsltViewData();
			string expectedSnippet = "<Root><MyElementID>1</MyElementID></Root>";
			var dataSource = new XslDataSource(new MockXslDataSource(expectedSnippet));
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

			var routeData = new RouteData();
			routeData.Values["controller"] = controller;
			Request.QueryString["myQueryString"] = "myQueryStringValue";

			var viewContext = new ViewContext(HttpContext, routeData, _fakeController, view, string.Empty, new ViewDataDictionary(vData), 
			                                          new TempDataDictionary());

			IViewEngine viewFactory = new XsltViewFactory(_viewSourceLoader);
			viewFactory.RenderView(viewContext);

			string actual = Response.Output.ToString().Replace("\r\n", "");

			XmlDocument xDoc = LoadXmlDocument("ViewTest.xml");

			string expected = xDoc.OuterXml;

			Assert.AreEqual(expected, actual);
		}
	}
}
