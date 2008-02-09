using System;
using System.IO;
using MvcContrib.UnitTests.XsltViewEngine.Helpers;
using MvcContrib.ViewFactories;
using MvcContrib.XsltViewEngine;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.XsltViewEngine
{
	[TestFixture, Category("XsltViewEngine")]
	public class XslTemplateTest
	{
		private const string controller = "MyController";
		private const string view = "MyView";
		private static readonly string VIEW_ROOT_DIRECTORY = Path.Combine(Environment.CurrentDirectory, "../../XsltViewEngine/Data/Views");
		private MockRepository _mocks;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
		}

		[Test]
		public void CreateTransformer()
		{
			IViewSourceLoader viewSourceLoader = _mocks.DynamicMock<IViewSourceLoader>();

			SetupResult.For(viewSourceLoader.HasView("MyController/MyView.xslt")).Return(true);
			SetupResult.For(viewSourceLoader.GetViewSource("MyController/MyView.xslt")).Return(new XsltViewSource());

			_mocks.ReplayAll();

			XsltTemplate template = new XsltTemplate(viewSourceLoader, controller, view);

			_mocks.VerifyAll();

			Assert.IsNotNull(template.XslTransformer);
			Assert.AreEqual("/" + controller + "/" + view, template.ViewUrl);
			Assert.AreEqual(view, template.ViewName);
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void ThrowExceptionWhenTemplateCantBeFound()
		{
			IViewSourceLoader viewSourceLoader = new FileSystemViewSourceLoader(Environment.CurrentDirectory);
			XsltTemplate template = new XsltTemplate(viewSourceLoader, controller, view);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void XsltTemplate_DependsOn_ViewSourceLoader()
		{
			XsltTemplate template = new XsltTemplate(null, controller, view);
		}
	}
}