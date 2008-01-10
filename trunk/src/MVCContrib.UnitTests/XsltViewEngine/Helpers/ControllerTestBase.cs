using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.XsltViewEngine.Helpers
{
	public class ControllerTestBase
	{
		private readonly MockRepository mockRepository = new MockRepository();
		private readonly TestViewFactory testViewFactory;

		public ControllerTestBase()
		{
			testViewFactory = new TestViewFactory();
		}

		protected void PrepareController(Controller controller)
		{
			controller.ViewFactory = testViewFactory;

			controller.ControllerContext =
				new ControllerContext(mockRepository.DynamicIHttpContext(), new RouteData(), controller);
		}

		public void AssertRenderedViewNameIs(string expectedViewName)
		{
			Assert.AreEqual(expectedViewName, testViewFactory.viewName, "Unexpected view name");
		}
	}

	internal class TestViewFactory : IViewFactory
	{
		public string masterName;
		public TestView testView;
		public object viewData;
		public string viewName;

		public TestViewFactory()
		{
			testView = new TestView();
		}

		#region IViewFactory Members

		public IView CreateView(ControllerContext controllerContext, string viewName, string masterName, object viewData)
		{
			this.viewName = viewName;
			this.masterName = masterName;
			this.viewData = viewData;
			return testView;
		}

		#endregion
	}

	internal class TestView : IView
	{
		public ViewContext viewContext;

		#region IView Members

		public void RenderView(ViewContext viewContext)
		{
			this.viewContext = viewContext;
		}

		#endregion
	}
}