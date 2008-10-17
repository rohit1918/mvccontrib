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
	    private ControllerContext _controllerContext;

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
			_controllerContext = new ControllerContext(requestContext, controller);

			_mocks.ReplayAll();

		}

		[Test]
		public void View_Renders_Output_To_Writer()
		{
			var view = new TestView();
            _viewContext = new ViewContext(_controllerContext, view, new ViewDataDictionary(), new TempDataDictionary());
            _viewContext.ViewData.Model = "Rendered by NHaml";
            view.Render(_viewContext, _output);
			Assert.AreEqual("Rendered by NHaml", _output.ToString());
		}

		[Test]
		public void View_Creates_AjaxHelper_On_Render()
		{
			var view = new TestView();
			Assert.IsNull(view.Ajax);
            _viewContext = new ViewContext(_controllerContext, view, new ViewDataDictionary(), new TempDataDictionary());
            view.Render(_viewContext, _output);
			Assert.IsNotNull(view.Ajax);
		}

		[Test]
		public void View_Creates_HtmlHelper_On_Render()
		{
			var view = new TestView();

			Assert.IsNull(view.Html);
            _viewContext = new ViewContext(_controllerContext, view, new ViewDataDictionary(), new TempDataDictionary());
            view.Render(_viewContext, _output);
			Assert.IsNotNull(view.Html);
		}

		[Test]
		public void View_Creates_UrlHelper_On_Render()
		{
			var view = new TestView();

			Assert.IsNull(view.Url);
            _viewContext = new ViewContext(_controllerContext, view, new ViewDataDictionary(), new TempDataDictionary());
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