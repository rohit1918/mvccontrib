using System;

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
				_mocks.DynamicMock<ControllerBase>()
			);

		}

		[Test]
		public void Should_convert_parameter_from_routedata()
		{
			_context.RouteData.Values.Add("foo", "1");
			object value = _binder.BindModel(CreateContext(typeof(int)));
			Assert.That(value, Is.EqualTo(1));
		}

		[Test]
		public void Should_convert_parameter_from_request()
		{
			SetupResult.For(_context.HttpContext.Request["foo"]).Return("1");
			_mocks.ReplayAll();

			object value = _binder.BindModel(CreateContext(typeof(int)));
			Assert.That(value, Is.EqualTo(1));
		}

		[Test]
		public void When_parameter_is_in_both_routedata_and_request_then_routedata_should_have_priority()
		{
			_context.RouteData.Values.Add("foo", "2");
			SetupResult.For(_context.HttpContext.Request["foo"]).Return("1");
			_mocks.ReplayAll();

			object value = _binder.BindModel(CreateContext(typeof(int)));
			Assert.That(value, Is.EqualTo(2));
		}

		[Test]
		public void When_no_parameter_can_be_found_then_the_default_value_should_be_returned_for_value_types()
		{
			object value = _binder.BindModel(CreateContext(typeof(int)));
			Assert.That(value, Is.EqualTo(0));
		}

		[Test]
		public void When_no_parameter_can_be_found_then_null_should_be_returned_for_reference_types()
		{
			object value = _binder.BindModel(CreateContext(typeof(object)));
			Assert.That(value, Is.Null);
		}

		[Test]
		public void When_conversion_fails_then_default_value_should_be_returned_for_value_types()
		{
			_context.RouteData.Values.Add("foo", "bar");
			object value = _binder.BindModel(CreateContext(typeof(int)));
			Assert.That(value, Is.EqualTo(0));
		}

		[Test]
		public void When_conversion_fails_then_null_should_be_returned_for_reference_types()
		{
			_context.RouteData.Values.Add("foo", "bar");
			object value = _binder.BindModel(CreateContext(typeof(CastleSimpleBinderTester)));
			Assert.That(value, Is.Null);
		}

		[Test]
		public void Should_be_able_to_look_up_simple_value_from_routedata() {
			_context.RouteData.Values.Add("foo", 1);
			var value = _binder.BindModel(CreateContext(typeof(int)));
			Assert.That(value, Is.TypeOf(typeof(int)));
			Assert.That(value, Is.EqualTo(1));
		}

		[Test]
		public void Should_be_able_to_look_up_complex_value_from_routedata() {
			_context.RouteData.Values.Add("foo", new CastleBinderTestObject { Name = "Foo" });
			var value = _binder.BindModel(CreateContext(typeof(CastleBinderTestObject)));
			Assert.That(value, Is.Not.Null);
			Assert.That(((CastleBinderTestObject)value).Name, Is.EqualTo("Foo"));
		}

		private ModelBindingContext CreateContext(Type type)
		{
			var context = new ModelBindingContext();
			context.ModelType = type;
			context.ModelName = "foo";
			
			return context;
			//TODO: this could be a problem?
			//    _context, 
			//    MockRepository.GenerateStub<IValueProvider>(),
			//    type,
			//    "foo",
			//    null,
			//    new ModelStateDictionary(),
			//    null
			//);
		}

		public class CastleBinderTestObject
		{
			public string Name { get; set; }
		}
	}
}