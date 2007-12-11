using System;
using System.Web;
using System.Web.Mvc;

namespace MVCContrib.SimplyRestful
{
	public class SimplyRestfulRouteHandler : MvcRouteHandler
	{
		private const string MatchAny = "*";

		private IRestfulActionResolver actionResolver;

		public SimplyRestfulRouteHandler()
		{
			actionResolver = new RestfulActionResolver();
		}

		public SimplyRestfulRouteHandler(IRestfulActionResolver actionResolver)
		{
			this.actionResolver = actionResolver;
		}

		protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			EnsureActionResolver(requestContext.HttpContext);

			RestfulAction action = actionResolver.ResolveAction(requestContext);
			if(action != RestfulAction.None)
			{
				requestContext.RouteData.Values["action"] = action.ToString();
			}
			return base.GetHttpHandler(requestContext);
		}

		private void EnsureActionResolver(IServiceProvider serviceProvider)
		{
			if(actionResolver == null)
			{
				actionResolver = (IRestfulActionResolver)serviceProvider.GetService(typeof(IRestfulActionResolver));
				if(actionResolver == null)
					actionResolver = new RestfulActionResolver();
			}
		}

		public static void InitializeRoutes(RouteCollection routeCollection)
		{
			InitializeRoutes(routeCollection, @"\d{1,10}");
		}

		public static void InitializeRoutes(RouteCollection routeCollection, string idValidationRegex)
		{
			routeCollection.Add(new Route
			{
				Url = "[controller]/new",
				Defaults = new
				{
					Action = "new"
				}
			,
				RouteHandler = typeof(MvcRouteHandler)
			}
		)
			;
			routeCollection.Add(new Route
			{
				Url = "[controller]/[id]/[action]",
				Validation = new
				{
					Method = "GET",
					Id = idValidationRegex ?? MatchAny,
					Action = "[eE][dD][iI][tT]|[dD][eE][lL][eE][tT][eE]"
				}
			,
				RouteHandler = typeof(MvcRouteHandler)
			}
		)
			;

			routeCollection.Add(new Route
			{
				Url = "[controller]/[id]",
				Validation = new
				{
					Method = "POST",
					Id = idValidationRegex ?? MatchAny,
				}
			,
				RouteHandler = typeof(SimplyRestfulRouteHandler)
			}
		)
			;

			routeCollection.Add(new Route
			{
				Url = "[controller]/[id]",
				Defaults = new
				{
					Action = "show"
				}
			,
				Validation = new
				{
					Method = "GET",
					Id = idValidationRegex ?? MatchAny,
				}
			,
				RouteHandler = typeof(MvcRouteHandler)
			}
		)
			;

			routeCollection.Add(new Route
			{
				Url = "[controller]/[id]",
				Defaults = new
				{
					Action = "update"
				}
			,
				Validation = new
				{
					Method = "PUT",
					Id = idValidationRegex ?? MatchAny
				}
			,
				RouteHandler = typeof(MvcRouteHandler)
			}
		)
			;

			routeCollection.Add(new Route
			{
				Url = "[controller]/[id]",
				Defaults = new
				{
					Action = "destroy"
				}
			,
				Validation = new
				{
					Method = "DELETE",
					Id = idValidationRegex ?? MatchAny
				}
			,
				RouteHandler = typeof(MvcRouteHandler)
			}
		)
			;

			routeCollection.Add(new Route
			{
				Url = "[controller]",
				Defaults = new
				{
					Action = "index"
				}
			,
				Validation = new
				{
					Method = "GET"
				}
			,
				RouteHandler = typeof(MvcRouteHandler)
			}
		)
			;

			routeCollection.Add(new Route
			{
				Url = "[controller]",
				Defaults = new
				{
					Action = "create"
				}
			,
				Validation = new
				{
					Method = "POST"
				}
			,
				RouteHandler = typeof(MvcRouteHandler)
			}
		)
			;
		}
	}
}