using System.Web.Routing;
using MvcContrib.UnitTests.Rest.Routing;
using MvcContrib.UnitTests.Rest.Routing.Model;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.Rest.Routing.Specs.For_RestfulRoutingBuilder
{
	[TestFixture]
	public class When_Adding_Nested_Route_With_Default_Rules : RestfulRoutingFixture
	{
		protected override void SetUp()
		{
			_restfulRoutingBuilder.ForAnyController().Register();
			_restfulRoutingBuilder.ForController<A1Controller>().FromRestfulParent<AController>().Register();
			_restfulRoutingBuilder.BuildRoutes(_routingEngine.Rules);
		}

		[Test]
		public void Then_Can_Get_Nested_Index_Rule()
		{
			RouteData match = With.GetRequest.To("/a/1/a1").Match();
			Assert.That(match, Is.Not.Null);
		}

		[Test]
		public void Then_Can_Not_Get_Inverse_Of_Nested_Index_Rule()
		{
			RouteData match = With.GetRequest.To("/a1/1/a").Match();
			Assert.That(match, Is.Null);
		}

		[Test]
		public void Then_Can_Not_Get_Nested_As_Root_Rule()
		{
			RouteData match = With.GetRequest.To("/a1/1").Match();
			Assert.That(match, Is.Null);
		}

		[Test]
		public void Then_Parent_Id_Parameter_Is_Named_aId()
		{
			RouteData match = With.GetRequest.To("/a/1/a1").Match();
			Assert.That(match.Values["aId"], Is.EqualTo("1"));
		}
	}
}