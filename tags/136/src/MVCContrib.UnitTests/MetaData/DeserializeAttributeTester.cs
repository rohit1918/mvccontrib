using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Attributes;
using NUnit.Framework;
using Rhino.Mocks;

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

			var requestContext = new RequestContext(context, new RouteData());
			_controllerContext = new ControllerContext(requestContext, _mocks.DynamicMock<IController>());
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

			var ids = (int[])attr.Bind(typeof(int[]), null, _controllerContext);
			Assert.IsNotNull(ids);
			Assert.AreEqual(1, ids.Length);
			Assert.AreEqual(1, ids[0]);
		}

		[Test]
		public void CanDeserializeFromForm()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.Form);

			var ids = (int[])attr.Bind(typeof(int[]), null, _controllerContext);
			Assert.IsNotNull(ids);
			Assert.AreEqual(1, ids.Length);
			Assert.AreEqual(2, ids[0]);
		}

		[Test]
		public void CanDeserializeFromParams()
		{
			var attr = new DeserializeAttribute("ids", RequestStore.Params);

			var ids = (int[])attr.Bind(typeof(int[]), null, _controllerContext);
			Assert.IsNotNull(ids);
			Assert.AreEqual(2, ids.Length);
		}
	}
}