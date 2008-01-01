using System.Collections;
using System.IO;
using System.Web;
using System.Web.Mvc;
using MvcContrib.NHamlViewEngine;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.NHamlViewEngine
{
	[TestFixture, Category("NHamlViewEngine")]
	public class NHamlViewTester
	{
		private MockRepository _mocks;
		private StringWriter _output;
		private ViewContext _viewContext;

		[SetUp]
		public void SetUp()
		{
			_output = new StringWriter();

			_mocks = new MockRepository();
			IHttpContext httpContext = _mocks.DynamicMock<IHttpContext>();
			IHttpResponse httpResponse = _mocks.DynamicMock<IHttpResponse>();
			IHttpSessionState httpSessionState = _mocks.DynamicMock<IHttpSessionState>();
			SetupResult.For(httpContext.Session).Return(httpSessionState);
			SetupResult.For(httpContext.Response).Return(httpResponse);
			SetupResult.For(httpResponse.Output).Return(_output);
			RequestContext requestContext = new RequestContext(httpContext, new RouteData());
			IController controller = _mocks.DynamicMock<IController>();
			ControllerContext controllerContext = new ControllerContext(requestContext, controller);

			_mocks.ReplayAll();

			_viewContext =
				new ViewContext(controllerContext, new Hashtable(), new TempDataDictionary(controllerContext.HttpContext));
		}

		[Test]
		public void View_Renders_Output_To_HttpContext_Response_Output()
		{
			TestView view = new TestView();
			view.SetViewData("Rendered by NHaml");
			view.RenderView(_viewContext);
			Assert.AreEqual("Rendered by NHaml", _output.ToString());
		}

		[Test]
		public void View_Creates_AjaxHelper_On_Render()
		{
			TestView view = new TestView();

			Assert.IsNull(view.Ajax);
			view.RenderView(_viewContext);
			Assert.IsNotNull(view.Ajax);
		}

		[Test]
		public void View_Creates_HtmlHelper_On_Render()
		{
			TestView view = new TestView();

			Assert.IsNull(view.Html);
			view.RenderView(_viewContext);
			Assert.IsNotNull(view.Html);
		}

		[Test]
		public void View_Creates_UrlHelper_On_Render()
		{
			TestView view = new TestView();

			Assert.IsNull(view.Url);
			view.RenderView(_viewContext);
			Assert.IsNotNull(view.Url);
		}

		private class TestView : NHamlView<string>, ICompiledView
		{
			public string Render()
			{
				return ViewData;
			}
		}
	}
}