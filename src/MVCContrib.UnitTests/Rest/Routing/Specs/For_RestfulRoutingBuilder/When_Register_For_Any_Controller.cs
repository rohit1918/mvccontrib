using System.Linq;
using System.Web.Routing;
using MvcContrib;
using MvcContrib.Rest.Routing.Descriptors;
using MvcContrib.Rest.Routing.Ext;
using MvcContrib.UnitTests.Rest.Routing;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.Rest.Routing.Specs.For_RestfulRoutingBuilder
{
	[TestFixture]
	public class When_Register_For_Any_Controller : RestfulRoutingFixture
	{
		protected override void SetUp()
		{
			_restfulRoutingBuilder.ForAnyController().Register();
			_restfulRoutingBuilder.BuildRoutes(_routingEngine.Rules);
		}

		[Test]
		public void Then_CreateUrl_To_Controller_Is_The_Default_Url()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "Test"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/test").IgnoreCase);
		}

		[Test]
		public void Then_CreateUrl_To_Delete()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "Test", id => "123", action => "delete"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/test/123/delete").IgnoreCase);
		}

		[Test]
		public void Then_CreateUrl_To_Edit()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "Test", id => "123", action => "edit"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/test/123/edit").IgnoreCase);
		}

		[Test]
		public void Then_CreateUrl_To_Index_Does_Not_Emit_Index()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "Test", action => "index"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/test").IgnoreCase);
		}

		[Test]
		public void Then_CreateUrl_To_New()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "Test", action => "new"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/test/new").IgnoreCase);
		}

		[Test]
		public void Then_CreateUrl_To_Show_Is_Default_Url_For_Entity()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "Test", id => "123"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/test/123").IgnoreCase);
		}

		[Test]
		public void Then_Four_Restful_Routes_Are_Created()
		{
			Assert.That(_restfulRoutingBuilder.RuleSets.Count(), Is.EqualTo(1));
			Assert.That(_restfulRoutingBuilder.RuleSets.First().Rules.Count(), Is.EqualTo(4));
		}

		[Test]
		public void Then_Matches_A_Delete_Request_To_The_Controller_Name_And_Integer_Id_With_Destroy_As_The_Action()
		{
			RouteData match = With.DeleteRequest.To("/test/123").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("test").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("destroy").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_And_Index_With_Index_As_The_Action()
		{
			RouteData match = With.GetRequest.To("/test/index").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["action"], Is.EqualTo("index").IgnoreCase);
			Assert.That(match.Values["controller"], Is.EqualTo("test").IgnoreCase);
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_And_Integer_Id_And_Delete_With_Delete_As_The_Action()
		{
			RouteData match = With.GetRequest.To("/test/123/delete").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("test").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("delete").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_And_Integer_Id_And_Edit_With_Edit_As_The_Action()
		{
			RouteData match = With.GetRequest.To("/test/123/edit").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("test").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("edit").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_And_Integer_Id_And_Show_With_Show_As_The_Action()
		{
			RouteData match = With.GetRequest.To("/test/123/show").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("test").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("show").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_And_Integer_Id_With_Show_As_The_Default_Action()
		{
			RouteData match = With.GetRequest.To("/test/123").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("test").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("show").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_With_Index_As_Default_The_Action()
		{
			RouteData match = With.GetRequest.To("/test").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["action"], Is.EqualTo("index").IgnoreCase);
			Assert.That(match.Values["controller"], Is.EqualTo("test").IgnoreCase);
		}

		[Test]
		public void
			Then_Matches_A_Post_Request_To_The_Controller_Name_And_Integer_Id_And_Form_Input_Named_Method_With_Value_Of_Delete_With_Destroy_As_The_Action
			()
		{
			RouteData match = With.PostRequest.To("/test/123")
				.WithFormValues(_method => "DELETE").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("test").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("destroy").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void
			Then_Matches_A_Post_Request_To_The_Controller_Name_And_Integer_Id_And_Form_Input_Named_Method_With_Value_Of_Put_With_Update_As_The_Action
			()
		{
			RouteData match = With.PostRequest.To("/test/123")
				.WithFormValues(_method => "PUT").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("test").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("update").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Post_Request_To_The_Controller_Name_With_Create_As_The_Action()
		{
			RouteData match = With.PostRequest.To("/test").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("test").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("create").IgnoreCase);
		}

		[Test]
		public void Then_Matches_A_Put_Request_To_The_Controller_Name_And_Integer_Id_With_Update_As_The_Action()
		{
			RouteData match = With.PutRequest.To("/test/123").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("test").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("update").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_The_First_Fragment_Of_Each_Route_Is_A_Controller_Fragment()
		{
			_restfulRoutingBuilder.RuleSets.ForEach(
				ruleSet =>
				ruleSet.Rules.ForEach(
					rule => Assert.That(Enumerable.First<IFragmentDescriptor>(rule.Fragments).Name, Is.EqualTo("controller"))));
		}

		[Test]
		public void Then_Will_Not_Match_A_Get_Request_To_The_Controller_Name_And_String_Id()
		{
			RouteData match = With.GetRequest.To("/test/goose").Match();
			Assert.That(match, Is.Null);
		}
	}
}