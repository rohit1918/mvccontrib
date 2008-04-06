using System.Linq;
using System.Web.Routing;
using MvcContrib;
using MvcContrib.Rest.Routing.Descriptors;
using MvcContrib.Rest.Routing.Ext;
using MvcContrib.UnitTests.Rest.Routing;
using MvcContrib.UnitTests.Rest.Routing.Model;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;


namespace MvcContrib.UnitTests.Rest.Routing.Specs.For_RestfulRoutingBuilder
{
	[TestFixture]
	public class When_Add_Route_For_Controller_In_A_Different_Path_And_Default_Routes : RestfulRoutingFixture
	{
		protected override void SetUp()
		{
			_restfulRoutingBuilder.ForController<AController>().FromPath("admin").Register();
			_restfulRoutingBuilder.ForAnyController().Register();

			_restfulRoutingBuilder.BuildRoutes(_routingEngine.Rules);
		}

		[Test]
		public void Then_A_Controller_Besides_AController_Cannot_Be_Match()
		{
			Assert.That(With.GetRequest.To("/admin/test").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/admin/test/index").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/admin/test/123").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/admin/test/123/show").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/admin/test/123/edit").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/admin/test/123/delete").Match(), Is.Null);
		}

		[Test]
		public void Then_CreateUrl_To_Controller_Is_The_Default_Url()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "A"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/admin/a").IgnoreCase);
		}

		[Test]
		public void Then_CreateUrl_To_Delete()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "A", id => "123", action => "delete"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/admin/a/123/delete").IgnoreCase);
		}

		[Test]
		public void Then_CreateUrl_To_Edit()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "A", id => "123", action => "edit"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/admin/a/123/edit").IgnoreCase);
		}

		[Test]
		public void Then_CreateUrl_To_Index_Does_Not_Emit_Index()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "A", action => "index"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/admin/a").IgnoreCase);
		}

		[Test]
		public void Then_CreateUrl_To_New()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "A", action => "new"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/admin/a/new").IgnoreCase);
		}

		[Test]
		public void Then_CreateUrl_To_Show_Is_Default_Url_For_Entity()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "A", id => "123"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/admin/a/123").IgnoreCase);
		}

		[Test]
		public void Then_Default_Rules_Are_Added()
		{
			Assert.That(With.GetRequest.To("/test").Match(), Is.Not.Null);
			Assert.That(With.GetRequest.To("/test/index").Match(), Is.Not.Null);
			Assert.That(With.GetRequest.To("/test/123").Match(), Is.Not.Null);
			Assert.That(With.GetRequest.To("/test/123/show").Match(), Is.Not.Null);
			Assert.That(With.GetRequest.To("/test/123/edit").Match(), Is.Not.Null);
			Assert.That(With.GetRequest.To("/test/123/delete").Match(), Is.Not.Null);
		}

		[Test]
		public void Then_Four_Restful_Routes_Are_Created()
		{
			Assert.That(_restfulRoutingBuilder.RuleSets.Count(), Is.EqualTo(2));
			Assert.That(_restfulRoutingBuilder.RuleSets.First().Rules.Count(), Is.EqualTo(4));
		}

		[Test]
		public void Then_Matches_A_Delete_Request_To_The_Controller_Name_And_Integer_Id_With_Destroy_As_The_Action()
		{
			RouteData match = With.DeleteRequest.To("/admin/a/123").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("destroy").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_And_Index_With_Index_As_The_Action()
		{
			RouteData match = With.GetRequest.To("/admin/a/index").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["action"], Is.EqualTo("index").IgnoreCase);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_And_Integer_Id_And_Delete_With_Delete_As_The_Action()
		{
			RouteData match = With.GetRequest.To("/admin/a/123/delete").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("delete").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_And_Integer_Id_And_Edit_With_Edit_As_The_Action()
		{
			RouteData match = With.GetRequest.To("/admin/a/123/edit").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("edit").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_And_Integer_Id_And_Show_With_Show_As_The_Action()
		{
			RouteData match = With.GetRequest.To("/admin/a/123/show").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("show").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_And_Integer_Id_With_Show_As_The_Default_Action()
		{
			RouteData match = With.GetRequest.To("/admin/a/123").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("show").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_With_Index_As_Default_The_Action()
		{
			RouteData match = With.GetRequest.To("/admin/a").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["action"], Is.EqualTo("index").IgnoreCase);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
		}

		[Test]
		public void
			Then_Matches_A_Post_Request_To_The_Controller_Name_And_Integer_Id_And_Form_Input_Named_Method_With_Value_Of_Delete_With_Destroy_As_The_Action
			()
		{
			RouteData match = With.PostRequest.To("/admin/a/123")
				.WithFormValues(_method => "DELETE").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("destroy").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void
			Then_Matches_A_Post_Request_To_The_Controller_Name_And_Integer_Id_And_Form_Input_Named_Method_With_Value_Of_Put_With_Update_As_The_Action
			()
		{
			RouteData match = With.PostRequest.To("/admin/a/123")
				.WithFormValues(_method => "PUT").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("update").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Post_Request_To_The_Controller_Name_With_Create_As_The_Action()
		{
			RouteData match = With.PostRequest.To("/admin/a").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("create").IgnoreCase);
		}

		[Test]
		public void Then_Matches_A_Put_Request_To_The_Controller_Name_And_Integer_Id_With_Update_As_The_Action()
		{
			RouteData match = With.PutRequest.To("/admin/a/123").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("update").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Registered_Controller_In_Different_Path_Cannot_Be_Matched_By_Default_Path()
		{
			Assert.That(With.GetRequest.To("/a").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/a/index").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/a/123").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/a/123/show").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/a/123/edit").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/a/123/delete").Match(), Is.Null);
		}

		[Test]
		public void
			Then_The_First_Fragment_Of_First_Rule_Set_Each_Route_Is_A_Static_Fragment_To_Admin_And_The_Second_Fragment_Is_A_Controller_Fragment
			()
		{
			_restfulRoutingBuilder.RuleSets.First().Rules.ForEach(
				rule =>
					{
						Assert.That(Enumerable.First<IFragmentDescriptor>(rule.Fragments).Name, Is.Null);
						Assert.That(Enumerable.First<IFragmentDescriptor>(rule.Fragments).UrlPart, Is.EqualTo("admin").IgnoreCase);
						Assert.That(Enumerable.Skip<IFragmentDescriptor>(rule.Fragments, 1).First().Name, Is.EqualTo("controller"));
					});
		}

		[Test]
		public void Then_The_Second_RuleSet_The_First_Fragment_Of_Each_Route_Is_A_Controller_Fragment()
		{
			_restfulRoutingBuilder.RuleSets.Skip(1).First().Rules.ForEach(
				rule => Assert.That(rule.Fragments.First().Name, Is.EqualTo("controller")));
		}

		[Test]
		public void Then_Will_Not_Match_A_Get_Request_To_The_Controller_Name_And_String_Id()
		{
			RouteData match = With.GetRequest.To("/admin/a/goose").Match();
			Assert.That(match, Is.Null);
		}
	}

	[TestFixture]
	public class When_Add_Route_For_Controller_In_A_Different_Path_That_Is_Three_Levels_Deep_Default_Routes :
		RestfulRoutingFixture
	{
		protected override void SetUp()
		{
			_restfulRoutingBuilder.ForController<AController>().FromPath("level1/level2/level3").Register();
			_restfulRoutingBuilder.ForAnyController().Register();

			_restfulRoutingBuilder.BuildRoutes(_routingEngine.Rules);
		}

		[Test]
		public void Then_A_Controller_Besides_AController_Cannot_Be_Match()
		{
			Assert.That(With.GetRequest.To("/level1/level2/level3/test").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/level1/level2/level3/test/index").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/level1/level2/level3/test/123").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/level1/level2/level3/test/123/show").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/level1/level2/level3/test/123/edit").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/level1/level2/level3/test/123/delete").Match(), Is.Null);
		}

		[Test]
		public void Then_CreateUrl_To_Controller_Is_The_Default_Url()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "A"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/level1/level2/level3/a").IgnoreCase);
		}

		[Test]
		public void Then_CreateUrl_To_Delete()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "A", id => "123", action => "delete"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/level1/level2/level3/a/123/delete").IgnoreCase);
		}

		[Test]
		public void Then_CreateUrl_To_Edit()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "A", id => "123", action => "edit"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/level1/level2/level3/a/123/edit").IgnoreCase);
		}

		[Test]
		public void Then_CreateUrl_To_Index_Does_Not_Emit_Index()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "A", action => "index"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/level1/level2/level3/a").IgnoreCase);
		}

		[Test]
		public void Then_CreateUrl_To_New()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "A", action => "new"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/level1/level2/level3/a/new").IgnoreCase);
		}

		[Test]
		public void Then_CreateUrl_To_Show_Is_Default_Url_For_Entity()
		{
			string url = _routingEngine.CreateUrl(With.GetRequest, new Hash(controller => "A", id => "123"));

			Assert.That(url, Is.Not.Null);
			Assert.That(url, Is.EqualTo("/level1/level2/level3/a/123").IgnoreCase);
		}

		[Test]
		public void Then_Default_Rules_Are_Added()
		{
			Assert.That(With.GetRequest.To("/test").Match(), Is.Not.Null);
			Assert.That(With.GetRequest.To("/test/index").Match(), Is.Not.Null);
			Assert.That(With.GetRequest.To("/test/123").Match(), Is.Not.Null);
			Assert.That(With.GetRequest.To("/test/123/show").Match(), Is.Not.Null);
			Assert.That(With.GetRequest.To("/test/123/edit").Match(), Is.Not.Null);
			Assert.That(With.GetRequest.To("/test/123/delete").Match(), Is.Not.Null);
		}

		[Test]
		public void Then_Four_Restful_Routes_Are_Created()
		{
			Assert.That(_restfulRoutingBuilder.RuleSets.Count(), Is.EqualTo(2));
			Assert.That(_restfulRoutingBuilder.RuleSets.First().Rules.Count(), Is.EqualTo(4));
		}

		[Test]
		public void Then_Matches_A_Delete_Request_To_The_Controller_Name_And_Integer_Id_With_Destroy_As_The_Action()
		{
			RouteData match = With.DeleteRequest.To("/level1/level2/level3/a/123").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("destroy").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_And_Index_With_Index_As_The_Action()
		{
			RouteData match = With.GetRequest.To("/level1/level2/level3/a/index").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["action"], Is.EqualTo("index").IgnoreCase);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_And_Integer_Id_And_Delete_With_Delete_As_The_Action()
		{
			RouteData match = With.GetRequest.To("/level1/level2/level3/a/123/delete").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("delete").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_And_Integer_Id_And_Edit_With_Edit_As_The_Action()
		{
			RouteData match = With.GetRequest.To("/level1/level2/level3/a/123/edit").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("edit").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_And_Integer_Id_And_Show_With_Show_As_The_Action()
		{
			RouteData match = With.GetRequest.To("/level1/level2/level3/a/123/show").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("show").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_And_Integer_Id_With_Show_As_The_Default_Action()
		{
			RouteData match = With.GetRequest.To("/level1/level2/level3/a/123").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("show").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Get_Request_To_The_Controller_Name_With_Index_As_Default_The_Action()
		{
			RouteData match = With.GetRequest.To("/level1/level2/level3/a").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["action"], Is.EqualTo("index").IgnoreCase);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
		}

		[Test]
		public void
			Then_Matches_A_Post_Request_To_The_Controller_Name_And_Integer_Id_And_Form_Input_Named_Method_With_Value_Of_Delete_With_Destroy_As_The_Action
			()
		{
			RouteData match = With.PostRequest.To("/level1/level2/level3/a/123")
				.WithFormValues(_method => "DELETE").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("destroy").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void
			Then_Matches_A_Post_Request_To_The_Controller_Name_And_Integer_Id_And_Form_Input_Named_Method_With_Value_Of_Put_With_Update_As_The_Action
			()
		{
			RouteData match = With.PostRequest.To("/level1/level2/level3/a/123")
				.WithFormValues(_method => "PUT").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("update").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Matches_A_Post_Request_To_The_Controller_Name_With_Create_As_The_Action()
		{
			RouteData match = With.PostRequest.To("/level1/level2/level3/a").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("create").IgnoreCase);
		}

		[Test]
		public void Then_Matches_A_Put_Request_To_The_Controller_Name_And_Integer_Id_With_Update_As_The_Action()
		{
			RouteData match = With.PutRequest.To("/level1/level2/level3/a/123").Match();

			Assert.That(match, Is.Not.Null);
			Assert.That(match.Values["controller"], Is.EqualTo("a").IgnoreCase);
			Assert.That(match.Values["action"], Is.EqualTo("update").IgnoreCase);
			Assert.That(match.Values["id"], Is.EqualTo("123"));
		}

		[Test]
		public void Then_Registered_Controller_In_Different_Path_Cannot_Be_Matched_By_Default_Path()
		{
			Assert.That(With.GetRequest.To("/a").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/a/index").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/a/123").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/a/123/show").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/a/123/edit").Match(), Is.Null);
			Assert.That(With.GetRequest.To("/a/123/delete").Match(), Is.Null);
		}

		[Test]
		public void
			Then_The_First_Fragment_Of_First_Rule_Set_Each_Route_Is_A_Static_Fragment_To_Admin_And_The_Second_Fragment_Is_A_Controller_Fragment
			()
		{
			_restfulRoutingBuilder.RuleSets.First().Rules.ForEach(
				rule =>
					{
						Assert.That(rule.Fragments.First().Name, Is.Null);
						Assert.That(rule.Fragments.First().UrlPart, Is.EqualTo("level1").IgnoreCase);
						Assert.That(rule.Fragments.Skip(3).First().Name, Is.EqualTo("controller"));
					});
		}

		[Test]
		public void Then_The_Second_RuleSet_The_First_Fragment_Of_Each_Route_Is_A_Controller_Fragment()
		{
			_restfulRoutingBuilder.RuleSets.Skip(1).First().Rules.ForEach(
				rule => Assert.That(rule.Fragments.First().Name, Is.EqualTo("controller")));
		}

		[Test]
		public void Then_Will_Not_Match_A_Get_Request_To_The_Controller_Name_And_String_Id()
		{
			RouteData match = With.GetRequest.To("/level1/level2/level3/a/goose").Match();
			Assert.That(match, Is.Null);
		}
	}
}