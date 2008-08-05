using System.Web;
using System.Web.Routing;
using MvcContrib.SimplyRestful;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.SimplyRestful
{
	[TestFixture]
	public class SimplyRestfulRouteHandlerTester
	{
		[Test]
		public void BuildRoutes_CreatesEightRoutes()
		{
			var routeCollection = new RouteCollection();
			SimplyRestfulRouteHandler.BuildRoutes(routeCollection);
			Assert.That(routeCollection.Count, Is.EqualTo(7));
		}

		[Test]
		public void BuildRoutes_WhenAreaHasLeadingSlash_StripsTheSlash()
		{
			var routeCollection = new RouteCollection();
			SimplyRestfulRouteHandler.BuildRoutes(routeCollection, "/admin");
			foreach(Route route in routeCollection)
			{
				Assert.That(route.Url, Text.StartsWith("admin"));
			}
		}

		[Test]
		public void BuildRoutes_WhenAreaHasTrailingSlash_StripsTheSlash()
		{
			var routeCollection = new RouteCollection();
			SimplyRestfulRouteHandler.BuildRoutes(routeCollection, "/admin/");
			foreach(Route route in routeCollection)
			{
				Assert.That(route.Url, Text.StartsWith("admin"));
				Assert.That(route.Url, Text.DoesNotStartWith("admin//"));
			}
		}

		[Test]
		public void BuildRoutes_WhenNullArea_CreatesDefaultRoutes()
		{
			var routeCollection = new RouteCollection();
			SimplyRestfulRouteHandler.BuildRoutes(routeCollection, null);
			foreach(Route route in routeCollection)
			{
				Assert.That(route.Url, Text.StartsWith("{controller}"));
			}
		}

		[Test]
		public void EnsureActionResolver_WhenResolverIsNull_ResolvesAndUsesOneFromTheContainer()
		{
			var mocks = new MockRepository();
			var httpContext = mocks.CreateMock<HttpContextBase>();
			var resolver = mocks.CreateMock<IRestfulActionResolver>();
			var httpRequest = mocks.DynamicMock<HttpRequestBase>();

			IRouteHandler handler = new SimplyRestfulRouteHandler();
			var requestContext = new RequestContext(httpContext, new RouteData());

			using(mocks.Record())
			{
				/// Context has to be a mock b/c we want to assert that GetService is called.
				Expect.Call(httpContext.Request).Repeat.Any().Return(httpRequest);
				Expect.Call(httpContext.GetService(typeof(IRestfulActionResolver))).Return(resolver).Repeat.Once();

				/// Resolver has to be a mock b/c we want to assert that it was gotten from the container and actually used.
				Expect.Call(resolver.ResolveAction(requestContext)).Return(RestfulAction.None);
			}
			using(mocks.Playback())
			{
				handler.GetHttpHandler(requestContext);
			}
		}

		[Test]
		public void EnsureActionResolver_WhenResolverIsNullAndCannotGetOneFromTheContainer_UsesRestfulActionResolver()
		{
			var mocks = new MockRepository();
			var httpContext = mocks.DynamicMock<HttpContextBase>();
			IRouteHandler handler = new SimplyRestfulRouteHandler();
			var requestContext = new RequestContext(httpContext, new RouteData());

			using(mocks.Record())
			{
				SetupResult.For(httpContext.Request).Return(mocks.DynamicMock<HttpRequestBase>());
			}
			using(mocks.Playback())
			{
				handler.GetHttpHandler(requestContext);
			}
		}

		[Test]
		public void EnsureActionResolver_WhenResolverIsUsedInTheConstructor_UsesRestfulActionResolver()
		{
			var mocks = new MockRepository();
			var httpContext = mocks.DynamicMock<HttpContextBase>();
			var resolver = mocks.CreateMock<IRestfulActionResolver>();
			IRouteHandler handler = new SimplyRestfulRouteHandler(resolver);
			var requestContext = new RequestContext(httpContext, new RouteData());

			using(mocks.Record())
			{
				Expect.Call(resolver.ResolveAction(requestContext)).Return(RestfulAction.None);
			}
			using(mocks.Playback())
			{
				handler.GetHttpHandler(requestContext);
			}
		}

		[Test]
		public void GetHttpHandler_WithARestfulAction_SetsRouteDataAction()
		{
			var mocks = new MockRepository();
			var httpContext = mocks.DynamicMock<HttpContextBase>();
			var resolver = mocks.DynamicMock<IRestfulActionResolver>();
			IRouteHandler handler = new SimplyRestfulRouteHandler(resolver);
			var requestContext = new RequestContext(httpContext, new RouteData());

			using(mocks.Record())
			{
				SetupResult.For(resolver.ResolveAction(requestContext)).Return(RestfulAction.Update);
			}
			using(mocks.Playback())
			{
				handler.GetHttpHandler(requestContext);
				Assert.That(requestContext.RouteData.Values["action"], Is.EqualTo("Update").IgnoreCase);
			}
		}

		[Test]
		public void GetHttpHandler_WithoutARestfulAction_DoesNotSetRouteDataAction()
		{
			var mocks = new MockRepository();
			var httpContext = mocks.DynamicMock<HttpContextBase>();
			var resolver = mocks.DynamicMock<IRestfulActionResolver>();

			IRouteHandler handler = new SimplyRestfulRouteHandler(resolver);
			var routeData = new RouteData();
			routeData.Values.Add("action", "goose");
			var requestContext = new RequestContext(httpContext, routeData);

			using(mocks.Record())
			{
				SetupResult.For(resolver.ResolveAction(requestContext)).Return(RestfulAction.None);
			}
			using(mocks.Playback())
			{
				handler.GetHttpHandler(requestContext);
				Assert.That(requestContext.RouteData.Values["action"], Is.EqualTo("goose"));
			}
		}
	}
}
