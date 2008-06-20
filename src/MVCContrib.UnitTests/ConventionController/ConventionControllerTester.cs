using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ActionResults;
using MvcContrib.MetaData;
using MvcContrib.UnitTests.ConventionController;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using System.Web;

namespace MvcContrib.UnitTests.ConventionController
{
	[TestFixture]
	public class ConventionControllerTester
	{
		private TestController _controller;
		private MockRepository _mocks;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			_controller = new TestController();
		}

		[Test]
		public void RenderXml_should_return_XmlResult_object()
		{
			var result = _controller.XmlResult() as XmlResult;
			Assert.That(result, Is.Not.Null);
			Assert.That(result.ObjectToSerialize, Is.EqualTo("Test 1 2 3"));
		}

		[Test]
		public void Binary_should_return_BinaryResult_object()
		{
			var result = _controller.BinaryResult() as BinaryResult;
			Assert.That(result, Is.Not.Null);
		}
	}
}
