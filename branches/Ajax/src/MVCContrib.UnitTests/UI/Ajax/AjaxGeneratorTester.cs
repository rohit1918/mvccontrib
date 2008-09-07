using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.UI.Ajax;
using MvcContrib.UI.Ajax.Internal;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.UI.Ajax
{
	[TestFixture]
	public class AjaxGeneratorTester
	{
		private TestAjaxGenerator _generator;
		private ViewContext _context;
		private MockRepository _mocks;

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			RouteTable.Routes.MapRoute("default", "{controller}/{action}/{id}", new {id = (string)null});
			RouteTable.Routes.MapRoute("other", "Test/Route");
		}

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			var controllerContext = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), _mocks.DynamicMock<ControllerBase>());
			_mocks.ReplayAll();

			_context = new ViewContext(controllerContext, "Index", new ViewDataDictionary(),new TempDataDictionary());
			
			controllerContext.RouteData.Values.Add("controller", "home");
			controllerContext.RouteData.Values.Add("action", "index");

			_generator = new TestAjaxGenerator(new AjaxHelper(_context));	
		}

		[TestFixtureTearDown]
		public void TestFixtureTeardown()
		{
			RouteTable.Routes.Clear();
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ActionLink_should_throw_if_linktext_is_null()
		{
			_generator.ActionLink(null, "Index", "Home", new RouteValueDictionary(), new AjaxOptions(), new Dictionary<string, object>());	
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ActionLink_should_throw_if_action_is_null()
		{
			_generator.ActionLink("Link", null, "Home", new RouteValueDictionary(), new AjaxOptions(), new Dictionary<string, object>());				
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ActionLink_should_throw_if_options_are_null()
		{
			_generator.ActionLink("Link", "Index", "Home", new RouteValueDictionary(), null, new Dictionary<string, object>());
		}

		[Test]
		public void CreateUrl_should_generate_url_using_action()
		{
			string url = _generator.CreateUrlPublic(null, "Show", null, null);
			Assert.That(url, Is.EqualTo("/home/Show"));
		}

		[Test]
		public void CreateUrl_should_generate_url_using_controller()
		{
			string url = _generator.CreateUrlPublic(null, "Show", "Customers", null);
			Assert.That(url, Is.EqualTo("/Customers/Show"));
		}

		[Test]
		public void CreateUrl_should_generate_url_using_route_link()
		{
			string url = _generator.CreateUrlPublic("other", null, null, null);
			Assert.That(url, Is.EqualTo("/Test/Route"));
		}

		[Test]
		public void CreateUrl_should_generate_url_with_custom_route_values()
		{
			string url = _generator.CreateUrlPublic(null, "Show", null, new RouteValueDictionary(new { id = 1 }));
			Assert.That(url, Is.EqualTo("/home/Show/1"));
		}


		[Test]
		public void ActionLink_overloads_should_delegate_to_final_implementation()
		{
			var options = new AjaxOptions();

			_generator.ActionLink("foo", "Index", options);
			Assert.That(_generator.LinkText, Is.EqualTo("foo"));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/home/Index"));
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));

			_generator.ActionLink("foo", "Index", new {id = 1}, options);
			Assert.That(_generator.LinkText, Is.EqualTo("foo"));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/home/Index/1"));
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));

			_generator.ActionLink("foo", "Index", new {id = 1}, options, new {@class = "bar"});
			Assert.That(_generator.LinkText, Is.EqualTo("foo"));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/home/Index/1"));
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.HtmlAttributes["class"], Is.EqualTo("bar"));

			_generator.ActionLink("foo", "Index", new RouteValueDictionary(new{id=1}), options);
			Assert.That(_generator.LinkText, Is.EqualTo("foo"));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/home/Index/1"));
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));

			_generator.ActionLink("foo", "Index", new RouteValueDictionary(new {id = 1}), options, new Hash(@class => "bar"));
			Assert.That(_generator.LinkText, Is.EqualTo("foo"));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/home/Index/1"));
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.HtmlAttributes["class"], Is.EqualTo("bar"));

			_generator.ActionLink("foo", "Index", "customers", options);
			Assert.That(_generator.LinkText, Is.EqualTo("foo"));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/customers/Index"));
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));

			_generator.ActionLink("foo", "Index", "customers", new { id = 1 }, options);
			Assert.That(_generator.LinkText, Is.EqualTo("foo"));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/customers/Index/1"));
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));

			_generator.ActionLink("foo", "Index", "customers",  new { id = 1 }, options, new { @class = "bar" });
			Assert.That(_generator.LinkText, Is.EqualTo("foo"));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/customers/Index/1"));
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.HtmlAttributes["class"], Is.EqualTo("bar"));

			_generator.ActionLink("foo", "Index", "customers", new RouteValueDictionary(new { id = 1 }), options);
			Assert.That(_generator.LinkText, Is.EqualTo("foo"));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/customers/Index/1"));
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));

			_generator.ActionLink("foo", "Index", "customers", new RouteValueDictionary(new { id = 1 }), options, new Hash(@class => "bar"));
			Assert.That(_generator.LinkText, Is.EqualTo("foo"));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/customers/Index/1"));
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.HtmlAttributes["class"], Is.EqualTo("bar"));
		}


		private class TestAjaxGenerator : AjaxGenerator
		{

			public string LinkText;
			public AjaxOptions AjaxOptions;
			public IDictionary<string, object> HtmlAttributes;
			public string TargetUrl;

			public TestAjaxGenerator(AjaxHelper ajaxHelper) : base(ajaxHelper)
			{
			}

			protected override string CreateLink(string linkText, string targetUrl, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
			{
				LinkText = null;
				AjaxOptions = null;
				HtmlAttributes = null;
				TargetUrl = null;

				LinkText = linkText;
				AjaxOptions = ajaxOptions;
				HtmlAttributes = htmlAttributes;
				TargetUrl = targetUrl;

				return string.Empty;
			}

			public override IDisposable Form(string actionName, string controllerName, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
			{
				throw new System.NotImplementedException();
			}

			public override bool IsMvcAjaxRequest()
			{
				throw new System.NotImplementedException();
			}

			public override string RouteLink(string linkText, string routeName, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
			{
				throw new System.NotImplementedException();
			}

			public string CreateUrlPublic(string routeName, string actionName, string controllerName, RouteValueDictionary values)
			{
				return CreateUrl(routeName, actionName, controllerName, values);
			}
		}
	}
}