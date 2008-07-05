namespace MvcContrib.UnitTests.MetaData
{
	using NUnit.Framework;
	using NUnit.Framework.SyntaxHelpers;
	using Rhino.Mocks;
	using MvcContrib.Castle;
	using System.Web.Routing;
	using System.Web.Mvc;

	[TestFixture]
	public class CastleSimpleBinderTester
	{
		private CastleSimpleBinder _binder;
		private MockRepository _mocks;
		private ControllerContext _context;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_binder = new CastleSimpleBinder();
			_context = new ControllerContext(
				_mocks.DynamicHttpContextBase(), 
				new RouteData(), 
				_mocks.DynamicMock<IController>()
			);

		}

		[Test]
		public void Should_convert_parameter_from_routedata()
		{
			_context.RouteData.Values.Add("foo", "1");
			object value = _binder.Bind(typeof(int), "foo", _context);
			Assert.That(value, Is.EqualTo(1));
		}

		[Test]
		public void Should_convert_parameter_from_request()
		{
			SetupResult.For(_context.HttpContext.Request["foo"]).Return("1");
			_mocks.ReplayAll();

			object value = _binder.Bind(typeof(int), "foo", _context);
			Assert.That(value, Is.EqualTo(1));
		}

		[Test]
		public void When_parameter_is_in_both_routedata_and_request_then_routedata_should_have_priority()
		{
			_context.RouteData.Values.Add("foo", "2");
			SetupResult.For(_context.HttpContext.Request["foo"]).Return("1");
			_mocks.ReplayAll();

			object value = _binder.Bind(typeof(int), "foo", _context);
			Assert.That(value, Is.EqualTo(2));
		}

		[Test]
		public void When_no_parameter_can_be_found_then_the_default_value_should_be_returned_for_value_types()
		{
			object value = _binder.Bind(typeof(int), "foo", _context);
			Assert.That(value, Is.EqualTo(0));
		}

		[Test]
		public void When_no_parameter_can_be_found_then_null_should_be_returned_for_reference_types()
		{
			object value = _binder.Bind(typeof(object), "foo", _context);
			Assert.That(value, Is.Null);
		}

		[Test]
		public void When_conversion_fails_then_default_value_should_be_returned_for_value_types()
		{
			_context.RouteData.Values.Add("foo", "bar");
			object value = _binder.Bind(typeof(int), "foo", _context);
			Assert.That(value, Is.EqualTo(0));
		}

		[Test]
		public void When_conversion_fails_then_null_should_be_returned_for_reference_types()
		{
			_context.RouteData.Values.Add("foo", "bar");
			object value = _binder.Bind(typeof(CastleSimpleBinderTester), "foo", _context);
			Assert.That(value, Is.Null);
		}

	}
}