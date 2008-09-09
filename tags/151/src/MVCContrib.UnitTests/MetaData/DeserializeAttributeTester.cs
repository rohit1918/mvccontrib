using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Attributes;
using NUnit.Framework;
using Rhino.Mocks;
using System.Collections.Specialized;

namespace MvcContrib.UnitTests.MetaData
{
	[TestFixture]
	public class DeserializeAttributeTester
	{
		private MockRepository _mocks;
		private ControllerContext _controllerContext;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			var context = _mocks.DynamicHttpContextBase();
			_mocks.Replay(context.Request);

			context.Request.QueryString["ids[0]"] = "1";
			context.Request.Form["ids[1]"] = "2";
			context.Request.Cookies.Add(new HttpCookie("ids[2]", "3"));
			context.Request.ServerVariables["ids[3]"] = "4";

			var routeData = new RouteData();
			routeData.Values.Add("ids[4]", 5);

			var requestContext = new RequestContext(context, routeData);
			_controllerContext = new ControllerContext(requestContext, _mocks.DynamicMock<ControllerBase>());
		}

		[Test]
		public void CanCreateAttribute()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.Params);
		}

		[Test]
		public void CanDeserializeFromQueryString()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.QueryString);

			var ids = (int[])attr.GetValue(_controllerContext, null, typeof(int[]), null);
			Assert.IsNotNull(ids);
			Assert.AreEqual(1, ids.Length);
			Assert.AreEqual(1, ids[0]);
		}

		[Test]
		public void CanDeserializeFromForm()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.Form);

			var ids = (int[])attr.GetValue(_controllerContext, null, typeof(int[]), null);
			Assert.IsNotNull(ids);
			Assert.AreEqual(1, ids.Length);
			Assert.AreEqual(2, ids[0]);
		}

		[Test]
		public void CanDeserializeFromCookies()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.Cookies);

			var ids = (int[])attr.GetValue(_controllerContext, null, typeof(int[]), null);
			Assert.IsNotNull(ids);
			Assert.AreEqual(1, ids.Length);
			Assert.AreEqual(3, ids[0]);
		}

		[Test]
		public void CanDeserializeFromServerVariables()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.ServerVariables);

			var ids = (int[])attr.GetValue(_controllerContext, null, typeof(int[]), null);
			Assert.IsNotNull(ids);
			Assert.AreEqual(1, ids.Length);
			Assert.AreEqual(4, ids[0]);
		}

		[Test]
		public void CanDeserializeFromParams()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.Params);

			var ids = (int[])attr.GetValue(_controllerContext, null, typeof(int[]), null);
			Assert.IsNotNull(ids);
			Assert.AreEqual(4, ids.Length);
		}

		[Test]
		public void CanDeserializeFromRouteData()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.RouteData);

			var ids = (int[])attr.GetValue(_controllerContext, null, typeof(int[]), null);
			Assert.IsNotNull(ids);
			Assert.AreEqual(1, ids.Length);
			Assert.AreEqual(5, ids[0]);
		}

		[Test]
		public void CanDeserializeFromAll()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.All);

			var ids = (int[])attr.GetValue(_controllerContext, null, typeof(int[]), null);
			Assert.IsNotNull(ids);
			Assert.AreEqual(5, ids.Length);
		}
	}
}