using System.Web;
using System.Web.Mvc;
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
			RouteCollection routeCollection = new RouteCollection();
			SimplyRestfulRouteHandler.BuildRoutes(routeCollection);
			Assert.That(routeCollection.Count, Is.EqualTo(8));
		}

		[Test]
		public void BuildRoutes_WhenAreaHasLeadingSlash_StripsTheSlash()
		{
			RouteCollection routeCollection = new RouteCollection();
			SimplyRestfulRouteHandler.BuildRoutes(routeCollection, "/admin");
			foreach(Route route in routeCollection)
			{
				Assert.That(route.Url, Text.StartsWith("admin"));
			}
		}

		[Test]
		public void BuildRoutes_WhenAreaHasTrailingSlash_StripsTheSlash()
		{
			RouteCollection routeCollection = new RouteCollection();
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
			RouteCollection routeCollection = new RouteCollection();
			SimplyRestfulRouteHandler.BuildRoutes(routeCollection, null);
			foreach(Route route in routeCollection)
			{
				Assert.That(route.Url, Text.StartsWith("[controller]"));
			}
		}

		[Test]
		public void EnsureActionResolver_WhenResolverIsNull_ResolvesAndUsesOneFromTheContainer()
		{
			MockRepository mocks = new MockRepository();
			IHttpContext httpContext = mocks.CreateMock<IHttpContext>();
			IRestfulActionResolver resolver = mocks.CreateMock<IRestfulActionResolver>();
			IHttpRequest httpRequest = mocks.DynamicMock<IHttpRequest>();

			IRouteHandler handler = new SimplyRestfulRouteHandler();
			RequestContext requestContext = new RequestContext(httpContext, new RouteData());

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
			MockRepository mocks = new MockRepository();
			IHttpContext httpContext = mocks.DynamicMock<IHttpContext>();
			IRouteHandler handler = new SimplyRestfulRouteHandler();
			RequestContext requestContext = new RequestContext(httpContext, new RouteData());

			using(mocks.Record())
			{
				SetupResult.For(httpContext.Request).Return(mocks.DynamicMock<IHttpRequest>());
			}
			using(mocks.Playback())
			{
				handler.GetHttpHandler(requestContext);
			}
		}

		[Test]
		public void EnsureActionResolver_WhenResolverIsUsedInTheConstructor_UsesRestfulActionResolver()
		{
			MockRepository mocks = new MockRepository();
			IHttpContext httpContext = mocks.DynamicMock<IHttpContext>();
			IRestfulActionResolver resolver = mocks.CreateMock<IRestfulActionResolver>();
			IRouteHandler handler = new SimplyRestfulRouteHandler(resolver);
			RequestContext requestContext = new RequestContext(httpContext, new RouteData());

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
			MockRepository mocks = new MockRepository();
			IHttpContext httpContext = mocks.DynamicMock<IHttpContext>();
			IRestfulActionResolver resolver = mocks.DynamicMock<IRestfulActionResolver>();
			IRouteHandler handler = new SimplyRestfulRouteHandler(resolver);
			RequestContext requestContext = new RequestContext(httpContext, new RouteData());

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
			MockRepository mocks = new MockRepository();
			IHttpContext httpContext = mocks.DynamicMock<IHttpContext>();
			IRestfulActionResolver resolver = mocks.DynamicMock<IRestfulActionResolver>();

			IRouteHandler handler = new SimplyRestfulRouteHandler(resolver);
			RouteData routeData = new RouteData();
			routeData.Values.Add("action", "goose");
			RequestContext requestContext = new RequestContext(httpContext, routeData);

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
