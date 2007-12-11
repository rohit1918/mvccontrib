using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using MVCContrib.SimplyRestful;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MVCContrib.UnitTests.SimplyRestful
{
	[TestFixture]
	public class SimplyRestfulRouteMatchTester
	{
		private MockRepository mocks;
		private IHttpContext httpContext;
		private IHttpRequest httpRequest;
		private RouteCollection routeCollection;

		[SetUp]
		public void Setup()
		{
			mocks = new MockRepository();
			httpContext = mocks.DynamicMock<IHttpContext>();
			httpRequest = mocks.DynamicMock<IHttpRequest>();
			routeCollection = new RouteCollection();
			SimplyRestfulRouteHandler.InitializeRoutes(routeCollection);
		}

		[Test]
		public void GetRouteData_WithAControllerAndIdUsingAGetRequest_SetsTheShowAction()
		{
			using(mocks.Record())
			{
				SetupContext("~/controller/123", "GET", null);
			}
			using(mocks.Playback())
			{
				RouteData routeData = routeCollection.GetRouteData(httpContext);
				Assert.That(routeData.Values["action"], Is.EqualTo("show").IgnoreCase);
			}
		}

		[Test]
		public void GetRouteData_WithAControllerAndIdAndEditActionUsingAGetRequest_SetsTheEditAction()
		{
			using(mocks.Record())
			{
				SetupContext("~/controller/123/edit", "GET", null);
			}
			using(mocks.Playback())
			{
				RouteData routeData = routeCollection.GetRouteData(httpContext);
				Assert.That(routeData.Values["action"], Is.EqualTo("edit").IgnoreCase);
			}
		}

		[Test]
		public void GetRouteData_WithAControllerAndIdAndNewActionUsingAGetRequest_SetsTheNewAction()
		{
			using(mocks.Record())
			{
				SetupContext("~/controller/new", "GET", null);
			}
			using(mocks.Playback())
			{
				RouteData routeData = routeCollection.GetRouteData(httpContext);
				Assert.That(routeData.Values["action"], Is.EqualTo("new").IgnoreCase);
			}
		}

		[Test]
		public void GetRouteData_WithAControllerAndIdAndDeleteActionUsingAGetRequest_SetsTheDeleteAction()
		{
			using(mocks.Record())
			{
				SetupContext("~/controller/123/delete", "GET", null);
			}
			using(mocks.Playback())
			{
				RouteData routeData = routeCollection.GetRouteData(httpContext);
				Assert.That(routeData.Values["action"], Is.EqualTo("delete").IgnoreCase);
			}
		}

		[Test]
		public void GetRouteData_WithAControllerUsingAGetRequest_SetsTheIndexAction()
		{
			using(mocks.Record())
			{
				SetupContext("~/controller", "GET", null);
			}
			using(mocks.Playback())
			{
				RouteData routeData = routeCollection.GetRouteData(httpContext);
				Assert.That(routeData.Values["action"], Is.EqualTo("index").IgnoreCase);
			}
		}

		[Test]
		public void GetRouteData_WithAControllerUsingAPostRequest_SetsTheCreateAction()
		{
			using(mocks.Record())
			{
				SetupContext("~/controller", "POST", null);
			}
			using(mocks.Playback())
			{
				RouteData routeData = routeCollection.GetRouteData(httpContext);
				Assert.That(routeData.Values["action"], Is.EqualTo("create").IgnoreCase);
			}
		}

		[Test]
		public void GetRouteData_WithAControllerAndIdUsingHttpPut_SetsTheUpdateAction()
		{
			using(mocks.Record())
			{
				SetupContext("~/controller/123", "PUT", null);
			}
			using(mocks.Playback())
			{
				RouteData routeData = routeCollection.GetRouteData(httpContext);
				Assert.That(routeData.Values["action"], Is.EqualTo("update").IgnoreCase);
			}
		}

		[Test]
		public void GetRouteData_WithAControllerAndIdUsingHttpMethodDelete_SetsTheDestroyAction()
		{
			using(mocks.Record())
			{
				SetupContext("~/controller/123", "DELETE", null);
			}
			using(mocks.Playback())
			{
				RouteData routeData = routeCollection.GetRouteData(httpContext);
				Assert.That(routeData.Values["action"], Is.EqualTo("destroy").IgnoreCase);
			}
		}

		[Test]
		public void GetRouteData_WithAControllerAndIdUsingFormMethodPut_UsesSimplyRestfulHandler()
		{
			using(mocks.Record())
			{
				SetupContext("~/controller/123", "POST", "PUT");
			}
			using(mocks.Playback())
			{
				RouteData routeData = routeCollection.GetRouteData(httpContext);
				Assert.That(routeData.Route.RouteHandler, Is.EqualTo(typeof(SimplyRestfulRouteHandler)));
			}
		}

		[Test]
		public void GetRouteData_WithAControllerAndIdUsingFormMethodDelete_UsesSimplyRestfulHandler()
		{
			using(mocks.Record())
			{
				SetupContext("~/controller/123", "POST", "DELETE");
			}
			using(mocks.Playback())
			{
				RouteData routeData = routeCollection.GetRouteData(httpContext);
				Assert.That(routeData.Route.RouteHandler, Is.EqualTo(typeof(SimplyRestfulRouteHandler)));
			}
		}

		[Test]
		public void GetRouteData_WithAControllerAndIdUsingHttpGetWithAnyIdValidator_SetsTheShowAction()
		{
			routeCollection = new RouteCollection();
			SimplyRestfulRouteHandler.InitializeRoutes(routeCollection, null);
			using(mocks.Record())
			{
				SetupContext("~/controller/123", "POST", "DELETE");
			}
			using(mocks.Playback())
			{
				RouteData routeData = routeCollection.GetRouteData(httpContext);
				Assert.That(routeData.Route.RouteHandler, Is.EqualTo(typeof(SimplyRestfulRouteHandler)));
			}
		}

		[Test]
		public void GetRouteData_WithAControllerAndIdUsingHttpGetWithAStringId_DoesNotMatch()
		{
			using(mocks.Record())
			{
				SetupContext("~/controller/Goose-Me", "POST", "DELETE");
			}
			using(mocks.Playback())
			{
				RouteData routeData = routeCollection.GetRouteData(httpContext);
				Assert.That(routeData, Is.Null);
			}
		}

		[Test]
		public void GetRouteData_WithAControllerAndIdUsingHttpGetWithStringIdValidatorAndANumericId_DoesNotMatch()
		{
			routeCollection = new RouteCollection();
			SimplyRestfulRouteHandler.InitializeRoutes(routeCollection, "[a-zA-Z]+");
			using(mocks.Record())
			{
				SetupContext("~/controller/123", "POST", "DELETE");
			}
			using(mocks.Playback())
			{
				RouteData routeData = routeCollection.GetRouteData(httpContext);
				Assert.That(routeData, Is.Null);
			}
		}

		private void SetupContext(string url, string httpMethod, string formMethod)
		{
			SetupResult.For(httpContext.Request).Return(httpRequest);
			SetupResult.For(httpRequest.AppRelativeCurrentExecutionFilePath).Return(url);
			SetupResult.For(httpRequest.HttpMethod).Return(httpMethod);
			if(!string.IsNullOrEmpty(formMethod))
			{
				NameValueCollection form = new NameValueCollection();
				form.Add("_method", formMethod);
				SetupResult.For(httpRequest.Form).Return(form);
			}
		}
	}
}