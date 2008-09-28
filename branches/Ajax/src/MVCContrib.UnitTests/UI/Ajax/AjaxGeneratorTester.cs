using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.UI;
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
			RouteTable.Routes.MapRoute("other", "Test/Route/{id}", new{id=(string)null});
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
		public void CreateLink_should_create_link()
		{
string expected = "<a class=\"foo\" href=\"www.mvccontrib.org\" onclick=\"bar\">MvcContrib</a>";
			string actual = _generator.BaseCreateLink("MvcContrib", "www.mvccontrib.org", new AjaxOptions(), new Hash(@class => "foo"));
		
			Assert.That(actual, Is.EqualTo(expected));		
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

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void Form_should_throw_if_action_name_is_null()
		{
			_generator.Form(null, "Foo", new RouteValueDictionary(), new AjaxOptions(), new Dictionary<string, object>());
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void Form_should_throw_if_ajax_options_are_null()
		{
			_generator.Form("Index", "Foo", new RouteValueDictionary(), null, new Dictionary<string, object>());
		}

		[Test]
		public void Form_should_return_disposable_form()
		{
			var form = _generator.Form("Show", "Customers", null, new AjaxOptions(), new Dictionary<string, object>());
			Assert.That(form, Is.InstanceOfType(typeof(DisposableElement)));
		}

		[Test]
		public void Form_overloads_should_delegate_to_final_implementation()
		{
			var options = new AjaxOptions()
			              	{
			              		Confirm = "confirm",
			              		HttpMethod = "POST",
			              		InsertionMode = System.Web.Mvc.Ajax.InsertionMode.InsertAfter,
			              		LoadingElementId = "loading",
			              		OnBegin = "begin",
			              		OnComplete = "complete",
			              		OnFailure = "failure",
			              		OnSuccess = "success",
			              		UpdateTargetId = "update",
			              		Url = "theUrl"
			              	};

			AssertForm(options, false, "/home/Index", _generator.Form("Index", options));
			AssertForm(options, false, "/home/Index/1", _generator.Form("Index", new{id=1}, options));
			AssertForm(options, true, "/home/Index/1", _generator.Form("Index", new{id=1}, options, new{@class = "foo"}));
			AssertForm(options, false, "/home/Index/1", _generator.Form("Index", new RouteValueDictionary(new{id=1}), options));
			AssertForm(options, true, "/home/Index/1", _generator.Form("Index", new RouteValueDictionary(new{id=1}), options, new Hash(@class => "foo")));
			AssertForm(options, false, "/Foo/Index", _generator.Form("Index", "Foo", options));
			AssertForm(options, false, "/Foo/Index/1", _generator.Form("Index", "Foo", new{id=1}, options));
			AssertForm(options, true, "/Foo/Index/1", _generator.Form("Index", "Foo", new{id=1}, options, new{@class = "foo"}));
			AssertForm(options, false, "/Foo/Index/1", _generator.Form("Index", "Foo", new RouteValueDictionary(new{id=1}), options));
			AssertForm(options, true, "/Foo/Index/1", _generator.Form("Index", "Foo", new RouteValueDictionary(new{id=1}), options, new Hash(@class => "foo")) );
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void RouteLink_should_throw_if_linktext_is_null()
		{
			_generator.RouteLink(null, "Route", new RouteValueDictionary(), new AjaxOptions(), new Dictionary<string, object>());
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void RouteLink_should_throw_if_options_are_null()
		{
			_generator.RouteLink("Text", "Route", new RouteValueDictionary(), null, new Dictionary<string, object>());
		}

		[Test]
		public void RouteLink_overloads_should_delegate_to_final_implementation()
		{
			var options = new AjaxOptions();

			_generator.RouteLink("Link", new {controller = "Home",action="Index"}, options);
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/Home/Index"));
			Assert.That(_generator.LinkText, Is.EqualTo("Link"));

			_generator.RouteLink("Link", new {controller = "Home", action = "Index"}, options, new Hash(@class => "Foo"));
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/Home/Index"));
			Assert.That(_generator.LinkText, Is.EqualTo("Link"));
			Assert.That(_generator.HtmlAttributes["class"], Is.EqualTo("Foo"));

			_generator.RouteLink("Link", new { controller = "Home", action = "Index" }, options, new{@class = "Foo"});
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/Home/Index"));
			Assert.That(_generator.LinkText, Is.EqualTo("Link"));
			Assert.That(_generator.HtmlAttributes["class"], Is.EqualTo("Foo"));

			_generator.RouteLink("Link", new RouteValueDictionary(new {controller = "Home", action = "Index"}), options);
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/Home/Index"));
			Assert.That(_generator.LinkText, Is.EqualTo("Link"));

			_generator.RouteLink("Link", new RouteValueDictionary(new { controller = "Home", action = "Index" }), options, new Hash(@class => "Foo"));
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/Home/Index"));
			Assert.That(_generator.LinkText, Is.EqualTo("Link"));
			Assert.That(_generator.HtmlAttributes["class"], Is.EqualTo("Foo"));

			_generator.RouteLink("Link", new RouteValueDictionary(new { controller = "Home", action = "Index" }), options, new { @class = "Foo" });
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/Home/Index"));
			Assert.That(_generator.LinkText, Is.EqualTo("Link"));
			Assert.That(_generator.HtmlAttributes["class"], Is.EqualTo("Foo"));

			_generator.RouteLink("Link", "Other", options);
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/Test/Route"));
			Assert.That(_generator.LinkText, Is.EqualTo("Link"));

			_generator.RouteLink("Link", "Other", options, new Hash(@class => "Foo"));
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/Test/Route"));
			Assert.That(_generator.LinkText, Is.EqualTo("Link"));
			Assert.That(_generator.HtmlAttributes["class"], Is.EqualTo("Foo"));

			_generator.RouteLink("Link", "Other", options, new {@class = "Foo"});
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/Test/Route"));
			Assert.That(_generator.LinkText, Is.EqualTo("Link"));
			Assert.That(_generator.HtmlAttributes["class"], Is.EqualTo("Foo"));

			_generator.RouteLink("Link", "Other", new {id = 1}, options);
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/Test/Route/1"));
			Assert.That(_generator.LinkText, Is.EqualTo("Link"));

			_generator.RouteLink("Link", "Other", new {id = 1}, options, new Hash(@class => "Foo"));
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/Test/Route/1"));
			Assert.That(_generator.LinkText, Is.EqualTo("Link"));
			Assert.That(_generator.HtmlAttributes["class"], Is.EqualTo("Foo"));

			_generator.RouteLink("Link", "Other", new {id = 1}, options, new {@class = "Foo"});
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/Test/Route/1"));
			Assert.That(_generator.LinkText, Is.EqualTo("Link"));
			Assert.That(_generator.HtmlAttributes["class"], Is.EqualTo("Foo"));

			_generator.RouteLink("Link", "Other", new RouteValueDictionary(new { id = 1 }), options);
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/Test/Route/1"));
			Assert.That(_generator.LinkText, Is.EqualTo("Link"));

			_generator.RouteLink("Link", "Other", new RouteValueDictionary(new { id = 1 }), options, new Hash(@class => "Foo"));
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/Test/Route/1"));
			Assert.That(_generator.LinkText, Is.EqualTo("Link"));
			Assert.That(_generator.HtmlAttributes["class"], Is.EqualTo("Foo"));

			_generator.RouteLink("Link", "Other", new RouteValueDictionary(new { id = 1 }), options, new { @class = "Foo" });
			Assert.That(_generator.AjaxOptions, Is.SameAs(options));
			Assert.That(_generator.TargetUrl, Is.EqualTo("/Test/Route/1"));
			Assert.That(_generator.LinkText, Is.EqualTo("Link"));
			Assert.That(_generator.HtmlAttributes["class"], Is.EqualTo("Foo"));

		}

		private void AssertForm(AjaxOptions options, bool checkAttributes, string url, IDisposable form)
		{
			Assert.That(form, Is.InstanceOfType(typeof(DisposableElement)));
			var element = ((DisposableElement)form);
			var tag = element.Tag;

			Assert.That(tag.Attributes["confirm"], Is.EqualTo(options.Confirm));
			Assert.That(tag.Attributes["method"], Is.EqualTo(options.HttpMethod));
			Assert.That(tag.Attributes["insertionmode"], Is.EqualTo(options.InsertionMode.ToString()));
			Assert.That(tag.Attributes["loading"], Is.EqualTo(options.LoadingElementId));
			Assert.That(tag.Attributes["begin"], Is.EqualTo(options.OnBegin));
			Assert.That(tag.Attributes["success"], Is.EqualTo(options.OnSuccess));
			Assert.That(tag.Attributes["complete"], Is.EqualTo(options.OnComplete));
			Assert.That(tag.Attributes["failure"], Is.EqualTo(options.OnFailure));
			Assert.That(tag.Attributes["update"], Is.EqualTo(options.UpdateTargetId));
			Assert.That(tag.Attributes["action"], Is.EqualTo(url));

			if(checkAttributes)
			{
				Assert.That(tag.Attributes["class"], Is.EqualTo("foo"));
			}
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

			public override string CreateLink(string linkText, string targetUrl, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
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

			public string BaseCreateLink(string linkText, string targetUrl, AjaxOptions options, IDictionary<string, object> htmlAttributes)
			{
				return base.CreateLink(linkText, targetUrl, options, htmlAttributes);
			}

			protected override string AjaxOptionsToString(AjaxOptions options)
			{
				return "bar";
			}

			public override TagBuilder CreateFormTag(string url, AjaxOptions options, IDictionary<string, object> htmlAttributes)
			{
				var tag = new TagBuilder("form");
				//Build a fake form which can be tested.
				tag.MergeAttribute("confirm", options.Confirm);
				tag.MergeAttribute("method", options.HttpMethod);
				tag.MergeAttribute("insertionmode", options.InsertionMode.ToString());
				tag.MergeAttribute("loading", options.LoadingElementId);
				tag.MergeAttribute("begin", options.OnBegin);
				tag.MergeAttribute("success", options.OnSuccess);
				tag.MergeAttribute("complete", options.OnComplete);
				tag.MergeAttribute("failure", options.OnFailure);
				tag.MergeAttribute("update", options.UpdateTargetId);
				tag.MergeAttribute("action", url);
				tag.MergeAttributes(htmlAttributes);
				return tag;
			}

			public override bool IsMvcAjaxRequest()
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