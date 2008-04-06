using System.Web.Routing;
using MvcContrib;
using MvcContrib.UnitTests.Rest.Routing;
using MvcContrib.UnitTests.Rest.Routing.Model;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.Rest.Routing.Specs.For_RestfulRoutingBuilder
{
	[TestFixture]
	public class When_Adding_Restful_Route_With_Extra_Action_On_Entity_Resource : RestfulRoutingFixture
	{
		[Test]
		public void Then_The_Route_Can_Be_Matched()
		{
			_restfulRoutingBuilder.ForController<AController>().WithExtraEntityRouteTo("detail").Register();
			_restfulRoutingBuilder.BuildRoutes(_routingEngine.Rules);

			RouteData match = With.GetRequest.To("a/123/detail").Match();
			Assert.That(match, Is.Not.Null);
		}

		[Test]
		public void Then_The_Url_Can_Be_Created()
		{
			_restfulRoutingBuilder.ForController<AController>().WithExtraEntityRouteTo("detail").Register();
			_restfulRoutingBuilder.BuildRoutes(_routingEngine.Rules);

			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "a", id => 123, action => "detail"));
			Assert.That(url, Is.EqualTo("/a/123/detail").IgnoreCase);
		}
	}

	[TestFixture]
	public class When_Adding_Restful_Route_With_Extra_Action_On_Listing_Resource : RestfulRoutingFixture
	{
		[Test]
		public void Then_The_Route_Can_Be_Matched_With_Valid_Parameter_Value()
		{
			_restfulRoutingBuilder.ForController<AController>().WithExtraListingRoute().ToAction("listbyletter", "list")
				.ToOptionalParameter("filter", null, new[] {"a", "b", "c", "d", "e"}).Register().Register();
			_restfulRoutingBuilder.BuildRoutes(_routingEngine.Rules);

			Assert.That(With.GetRequest.To("a/list/B").Match(), Is.Not.Null,
			            "Extra listing rule with accepted value should match.");
		}

		[Test]
		public void Then_The_Route_Will_Be_Matched_With_No_Parameter()
		{
			_restfulRoutingBuilder.ForController<AController>().WithExtraListingRoute().ToAction("listbyletter", "list")
				.ToOptionalParameter("filter", null, new[] {"a", "b", "c", "d", "e"}).Register().Register();
			_restfulRoutingBuilder.BuildRoutes(_routingEngine.Rules);

			Assert.That(With.GetRequest.To("a/list/").Match(), Is.Not.Null,
			            "Extra listing rule with missing optional parameter should match.");
		}

		[Test]
		public void Then_The_Route_Will_Be_Matched_With_No_Parameter_And_No_Trailing_Slash()
		{
			_restfulRoutingBuilder.ForController<AController>().WithExtraListingRoute().ToAction("listbyletter", "list")
				.ToOptionalParameter("filter", null, new[] {"a", "b", "c", "d", "e"}).Register().Register();
			_restfulRoutingBuilder.BuildRoutes(_routingEngine.Rules);

			Assert.That(With.GetRequest.To("a/list").Match(), Is.Not.Null,
			            "Extra listing rule with missing optional parameter and no trailing slash should match.");
		}

		[Test]
		public void Then_The_Route_Will_Not_Be_Matched_With_InValid_Parameter_Value()
		{
			_restfulRoutingBuilder.ForController<AController>().WithExtraListingRoute().ToAction("listbyletter", "list")
				.ToOptionalParameter("filter", null, new[] {"a", "b", "c", "d", "e"}).Register().Register();
			_restfulRoutingBuilder.BuildRoutes(_routingEngine.Rules);

			Assert.That(With.GetRequest.To("a/list/Z").Match(), Is.Null,
			            "Extra listing rule with invalid value should not match.");
		}

		[Test]
		public void Then_The_Url_Can_Be_Created_With_Value_For_Optional_Parameter()
		{
			_restfulRoutingBuilder.ForController<AController>().WithExtraListingRoute().ToAction("listbyletter", "list")
				.ToOptionalParameter("filter", null, new[] {"a", "b", "c", "d", "e"}).Register().Register();
			_restfulRoutingBuilder.BuildRoutes(_routingEngine.Rules);

			string url = _routingEngine.CreateUrl(With.GetRequest,
			                                      new Hash(controller => "a", action => "listbyletter", filter => "b"));
			Assert.That(url, Is.EqualTo("/a/list/b").IgnoreCase);
		}

		[Test]
		public void Then_The_Url_Can_Be_Created_Without_Value_For_Optional_Parameter()
		{
			_restfulRoutingBuilder.ForController<AController>().WithExtraListingRoute().ToAction("list")
				.ToOptionalParameter("filter", null, new[] {"a", "b", "c", "d", "e"}).Register().Register();
			_restfulRoutingBuilder.BuildRoutes(_routingEngine.Rules);

			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "a", action => "list"));
			Assert.That(url, Is.EqualTo("/a/list").IgnoreCase);
		}
	}
}