using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
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

			NameValueCollection queryString = new NameValueCollection();
			queryString["ids[0]"] = "1";

			NameValueCollection form = new NameValueCollection();
			form["ids[1]"] = "2";

			IHttpRequest request = _mocks.DynamicMock<IHttpRequest>();
			SetupResult.For(request.QueryString).Return(queryString);
			SetupResult.For(request.Form).Return(form);

			IHttpContext context = _mocks.DynamicMock<IHttpContext>();
			SetupResult.For(context.Request).Return(request);

			RequestContext requestContext = new RequestContext(context, new RouteData());
			_controllerContext = new ControllerContext(requestContext, _mocks.DynamicMock<IController>());

			_mocks.ReplayAll();
		}

		[Test]
		public void CanCreateAttribute()
		{
			DeserializeAttribute attr = new DeserializeAttribute("ids", RequestStore.Params);
		}

		[Test]
		public void CanDeserializeFromQueryString()
		{
			DeserializeAttribute attr = new DeserializeAttribute("ids", RequestStore.QueryString);

			int[] ids = (int[])attr.Bind(typeof(int[]), null, _controllerContext);
			Assert.IsNotNull(ids);
			Assert.AreEqual(1, ids.Length);
			Assert.AreEqual(1, ids[0]);
		}

		[Test]
		public void CanDeserializeFromForm()
		{
			DeserializeAttribute attr = new DeserializeAttribute("ids", RequestStore.Form);

			int[] ids = (int[])attr.Bind(typeof(int[]), null, _controllerContext);
			Assert.IsNotNull(ids);
			Assert.AreEqual(1, ids.Length);
			Assert.AreEqual(2, ids[0]);
		}

		[Test]
		public void CanDeserializeFromParams()
		{
			DeserializeAttribute attr = new DeserializeAttribute("ids", RequestStore.Params);

			int[] ids = (int[])attr.Bind(typeof(int[]), null, _controllerContext);
			Assert.IsNotNull(ids);
			Assert.AreEqual(2, ids.Length);
		}
	}
}
