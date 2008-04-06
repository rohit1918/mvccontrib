using System.Web.Routing;
using MvcContrib.UnitTests.Rest.Routing;
using MvcContrib.UnitTests.Rest.Routing.Model;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;


namespace MvcContrib.UnitTests.Rest.Routing.Specs.For_RestfulRoutingBuilder
{
	[TestFixture]
	public class When_Not_Setting_Id : RestfulRoutingFixture
	{
		protected override void SetUp()
		{
			_restfulRoutingBuilder.ForController<AController>().Register();
			_restfulRoutingBuilder.BuildRoutes(_routingEngine.Rules);
		}

		[Test]
		public void Then_A_Negative_Int32_Can_Not_Be_Matched()
		{
			RouteData match = With.GetRequest.To("a/-123/").Match();
			Assert.That(match, Is.Null);
		}

		[Test]
		public void Then_A_Positive_Int32_Can_Be_Matched()
		{
			RouteData match = With.GetRequest.To("a/123/").Match();
			Assert.That(match, Is.Not.Null);
		}

		[Test, Explicit("Need to figure out an efficient way to fix this.")]
		public void Then_A_Positive_Int64_Can_Not_Be_Matched()
		{
			RouteData match = With.GetRequest.To("a/" + (((long) int.MaxValue) + 1) + "/").Match();
			Assert.That(match, Is.Null);
		}

		[Test]
		public void Then_Matched_Parameter_Name_Is_id()
		{
			RouteData match = With.GetRequest.To("a/123/").Match();
			Assert.That(match.Values.ContainsKey("id"), Is.True);
		}

		[Test]
		public void Then_The_Max_Int32_Can_Be_Matched()
		{
			RouteData match = With.GetRequest.To("a/" + int.MaxValue + "/").Match();
			Assert.That(match.Values["id"], Is.EqualTo(int.MaxValue.ToString()));
		}

		[Test]
		public void Then_The_Url_Can_Be_Created()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "a", id => 123, action => "edit"));
			Assert.That(url, Is.EqualTo("/a/123/edit").IgnoreCase);
		}
	}

	[TestFixture]
	public class When_Setting_Id_Name : RestfulRoutingFixture
	{
		[Test]
		public void On_Id_Builder_From_Root_Then_Parameter_Matched_Is_Named_foo()
		{
			_restfulRoutingBuilder.ForController<AController>().UsingId().Named("foo").Return().Register();
			_restfulRoutingBuilder.BuildRoutes(_routingEngine.Rules);

			RouteData match = With.GetRequest.To("a/123/").Match();
			Assert.That(match.Values.ContainsKey("foo"), Is.True);
		}

		[Test]
		public void On_Id_Builder_From_When_Entity_Is_Closed_Then_Parameter_Matched_Is_Named_foo()
		{
			_restfulRoutingBuilder.ForController<AController>().UsingId("bar").ToEntity().UsingId().Named("foo").Return().
				Register();
			_restfulRoutingBuilder.BuildRoutes(_routingEngine.Rules);

			RouteData match = With.GetRequest.To("a/123/").Match();
			Assert.That(match.Values.ContainsKey("foo"), Is.True);
		}

		[Test]
		public void On_Root_Builder_With_Parameter_Then_Parameter_Matched_Is_Named_foo()
		{
			_restfulRoutingBuilder.ForController<AController>().UsingId("foo").Register();
			_restfulRoutingBuilder.BuildRoutes(_routingEngine.Rules);

			RouteData match = With.GetRequest.To("a/123/").Match();
			Assert.That(match.Values.ContainsKey("foo"), Is.True);
		}
	}
}