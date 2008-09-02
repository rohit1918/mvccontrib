using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.NHamlViewEngine;
using NHaml;
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
			var httpContext = _mocks.DynamicMock<HttpContextBase>();
			var httpResponse = _mocks.DynamicMock<HttpResponseBase>();
			var httpSessionState = _mocks.DynamicMock<HttpSessionStateBase>();
			SetupResult.For(httpContext.Session).Return(httpSessionState);
			SetupResult.For(httpContext.Response).Return(httpResponse);
			SetupResult.For(httpResponse.Output).Return(_output);
			var requestContext = new RequestContext(httpContext, new RouteData());
			var controller = _mocks.DynamicMock<ControllerBase>();
			var controllerContext = new ControllerContext(requestContext, controller);

			_mocks.ReplayAll();

			_viewContext = new ViewContext(controllerContext, "Index", new ViewDataDictionary(), new TempDataDictionary());
		}

		[Test]
		public void View_Renders_Output_To_Writer()
		{
			_viewContext.ViewData.Model = "Rendered by NHaml";
			var view = new TestView();
			view.Render(_viewContext, _output);
			Assert.AreEqual("Rendered by NHaml", _output.ToString());
		}

		[Test]
		public void View_Creates_AjaxHelper_On_Render()
		{
			var view = new TestView();
			Assert.IsNull(view.Ajax);
			view.Render(_viewContext, _output);
			Assert.IsNotNull(view.Ajax);
		}

		[Test]
		public void View_Creates_HtmlHelper_On_Render()
		{
			var view = new TestView();

			Assert.IsNull(view.Html);
			view.Render(_viewContext, _output);
			Assert.IsNotNull(view.Html);
		}

		[Test]
		public void View_Creates_UrlHelper_On_Render()
		{
			var view = new TestView();

			Assert.IsNull(view.Url);
			view.Render(_viewContext, _output);
			Assert.IsNotNull(view.Url);
		}

		private class TestView : NHamlView<string>, ICompiledView
		{
			public string Render()
			{
				return ViewData.Model;
			}
		}
	}
}