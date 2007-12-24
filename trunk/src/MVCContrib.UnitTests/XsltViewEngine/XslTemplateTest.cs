using System;
using System.IO;
using MvcContrib.XsltViewEngine;
using NUnit.Framework;

namespace MVCContrib.UnitTests.XsltViewEngine
{
	[TestFixture]
	public class XslTemplateTest
	{
		private const string controller = "MyController";
		private const string view = "MyView";
		private static readonly string basePath = Path.Combine(Environment.CurrentDirectory, "../../XsltViewEngine/Data");

		[Test, Category("Integration")]
		public void CreateTransformer()
		{
			XsltTemplate template = new XsltTemplate(basePath, controller, view);

			Assert.IsNotNull(template.XslTransformer);

			Assert.AreEqual("/" + controller + "/" + view, template.ViewUrl);
			Assert.AreEqual(view, template.ViewName);
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void ThrowExceptionWhenTemplateCantBeFound()
		{
			XsltTemplate template = new XsltTemplate(Environment.CurrentDirectory, controller, view);

			Assert.IsNotNull(template.XslTransformer);
		}
	}
}