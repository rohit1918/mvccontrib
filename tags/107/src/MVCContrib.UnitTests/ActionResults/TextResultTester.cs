using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ActionResults;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ActionResults
{
	[TestFixture]
	public class TextResultTester
	{
		private ControllerContext _context;
		private MockRepository _mocks;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_context = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), _mocks.DynamicMock<IController>());
			_mocks.ReplayAll();
		}

		[Test]
		public void ToWrite_property_should_return_string_to_write()
		{
			var result = new TextResult("Test 1 2 3");
			Assert.That(result.ToWrite, Is.EqualTo("Test 1 2 3"));
		}

		[Test]
		public void Should_render_item_to_output_stream()
		{
			var expected = "Test 1 2 3";
			var result = new TextResult(expected);
			result.ExecuteResult(_context);
			Assert.That(_context.HttpContext.Response.Output.ToString(), Is.EqualTo(expected));
		}
	}
}