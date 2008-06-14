using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;
using MvcContrib.Castle;
using NUnit.Framework.SyntaxHelpers;
namespace MvcContrib.UnitTests.MetaData
{
	[TestFixture]
	public class CastleBinderTester
	{
		private MockRepository _mocks;
		private ControllerContext _context;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_context = new ControllerContext(
				_mocks.DynamicHttpContextBase(), new RouteData(), _mocks.DynamicMock<IController>() 	
			);
			_mocks.ReplayAll();
		}

		[Test]
		public void Should_bind_using_parameter_name()
		{
			_context.HttpContext.Request.Form["cust.Id"] = "5";
			_context.HttpContext.Request.Form["cust.Name"] = "Jeremy";

			var binder = new CastleBindAttribute();
			object value = binder.Bind(typeof(Customer), "cust", _context);
			var customer = value as Customer;
			
			Assert.That(customer, Is.Not.Null);
			Assert.That(customer.Name, Is.EqualTo("Jeremy"));
			Assert.That(customer.Id, Is.EqualTo(5));
		}

		[Test]
		public void Should_bind_using_custom_prefix()
		{
			_context.HttpContext.Request.Form["cust.Id"] = "5";
			_context.HttpContext.Request.Form["cust.Name"] = "Jeremy";

			var binder = new CastleBindAttribute("cust");
			object value = binder.Bind(typeof(Customer), "Foo", _context);
			var customer = value as Customer;

			Assert.That(customer, Is.Not.Null);
			Assert.That(customer.Name, Is.EqualTo("Jeremy"));
			Assert.That(customer.Id, Is.EqualTo(5));
		}

		public class Customer
		{
			public string Name { get; set; }
			public int Id { get; set; }
		}
	}
}