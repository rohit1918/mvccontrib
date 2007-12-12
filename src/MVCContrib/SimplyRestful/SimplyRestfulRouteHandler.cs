using System;
using System.Web;
using System.Web.Mvc;

namespace MVCContrib.SimplyRestful
{
	/// <summary>A Rails inspired Restful Routing Handler</summary>
	public class SimplyRestfulRouteHandler : MvcRouteHandler
	{
		/// <summary>Matches anything.</summary>
		public const string MatchAny = ".+";

		/// <summary>Matches a base64 encoded <see cref="Guid"/></summary>
		public const string MatchBase64Guid = @"[a-zA-Z0-9+/=]{22,24}";

		/// <summary>Matches a <see cref="Guid"/><c>@"\{?[a-fA-F0-9]{8}(?:-(?:[a-fA-F0-9]){4}){3}-[a-fA-F0-9]{12}\}?"</c></summary>
		public const string MatchGuid = @"\{?[a-fA-F0-9]{8}(?:-(?:[a-fA-F0-9]){4}){3}-[a-fA-F0-9]{12}\}?";

		/// <summary>Matches a Positive <see cref="int"/> <c>@"\d{1,10}"</c></summary>
		public const string MatchPositiveInteger = @"\d{1,10}";

		/// <summary>Matches a Positive <see cref="long"/> <c>@"\d{1,19}"</c></summary>
		public const string MatchPositiveLong = @"\d{1,19}";

		private IRestfulActionResolver _actionResolver;
		
		public SimplyRestfulRouteHandler()
		{
		}

		public SimplyRestfulRouteHandler(IRestfulActionResolver actionResolver)
		{
			this._actionResolver = actionResolver;
		}

		protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			EnsureActionResolver(requestContext.HttpContext);

			RestfulAction action = _actionResolver.ResolveAction(requestContext);
			if(action != RestfulAction.None)
			{
				requestContext.RouteData.Values["action"] = action.ToString();
			}
			return base.GetHttpHandler(requestContext);
		}

		/// <summary>
		/// Adds the set of SimplyRestful routes to the <paramref name="routeCollection"/>.
		/// By default a positive integer validator is set for the Id parameter of the <see cref="Route.Values"/>.
		/// </summary>
		/// <param name="routeCollection">The route collection to add the routes to.</param>
		/// <seealso cref="BuildRoutes(RouteCollection,string,string,string)"/>
		public static void BuildRoutes(RouteCollection routeCollection)
		{
			BuildRoutes(routeCollection, "[controller]", MatchPositiveInteger, null);
		}

		/// <summary>
		/// Adds the set of SimplyRestful routes to the <paramref name="routeCollection"/>.
		/// By default a positive integer validator is set for the Id parameter of the <see cref="Route.Values"/>.
		/// </summary>
		/// <param name="routeCollection">The route collection to add the routes to.</param>
		/// <param name="areaPrefix">An area inside the site to prefix the <see cref="Route.Url"/> with.</param>
		/// <seealso cref="BuildRoutes(RouteCollection,string,string,string)"/>
		/// <example lang="c#">
		/// SimplyRestfulRouteHandler.BuildRoutes(RouteTable.Routes, "private/admin")
		/// // Generates the Urls private/admin/[controller]/new, private/admin/[controller]/[id]/edit, etc.
		/// </example>
		public static void BuildRoutes(RouteCollection routeCollection, string areaPrefix)
		{
			BuildRoutes(routeCollection, FixPath(areaPrefix) + "/[controller]", MatchPositiveInteger, null);
		}

		/// <summary>
		/// Adds the set of SimplyRestful routes to the <paramref name="routeCollection"/>.
		/// </summary>
		/// <param name="routeCollection">The route collection to add the  routes to.</param>
		/// <param name="controllerPath">The path to the controller, you can use the special matching [controller]</param>
		/// <param name="idValidationRegex">The <see cref="System.Text.RegularExpressions.Regex"/> 
		/// validator to add to the Id parameter of the <see cref="Route.Values"/>, use <c>null</c> to not validate the id.</param>
		/// <param name="controller">The name of the controller.  Only required if you are trying to route to a specific controller using a non-standard url.</param>
		public static void BuildRoutes(RouteCollection routeCollection, string controllerPath, string idValidationRegex, string controller)
		{
			controllerPath = FixPath(controllerPath);

			routeCollection.Add(new Route
			{
				Url = controllerPath + "/new",
				Defaults = BuildDefaults(RestfulAction.New, controller),
				RouteHandler = typeof(MvcRouteHandler)
			});

			routeCollection.Add(new Route
			{
				Url = controllerPath + "/[id]/[action]",
				Defaults = BuildDefaults(RestfulAction.None, controller),
				Validation = new
				{
					Method = "GET",
					Id = idValidationRegex ?? MatchAny,
					Action = "[eE][dD][iI][tT]|[dD][eE][lL][eE][tT][eE]"
				},
				RouteHandler = typeof(MvcRouteHandler)
			});

			routeCollection.Add(new Route
			{
				Url = controllerPath + "/[id]",
				Defaults = BuildDefaults(RestfulAction.None, controller),
				Validation = new
				{
					Method = "POST",
					Id = idValidationRegex ?? MatchAny,
				},
				RouteHandler = typeof(SimplyRestfulRouteHandler)
			});

			routeCollection.Add(new Route
			{
				Url = controllerPath + "/[id]",
				Defaults = BuildDefaults(RestfulAction.Show, controller),
				Validation = new
				{
					Method = "GET",
					Id = idValidationRegex ?? MatchAny,
				},
				RouteHandler = typeof(MvcRouteHandler)
			});

			routeCollection.Add(new Route
			{
				Url = controllerPath + "/[id]",
				Defaults = BuildDefaults(RestfulAction.Update, controller),
				Validation = new
				{
					Method = "PUT",
					Id = idValidationRegex ?? MatchAny
				},
				RouteHandler = typeof(MvcRouteHandler)
			});

			routeCollection.Add(new Route
			{
				Url = controllerPath + "/[id]",
				Defaults = BuildDefaults(RestfulAction.Destroy, controller),
				Validation = new
				{
					Method = "DELETE",
					Id = idValidationRegex ?? MatchAny
				},
				RouteHandler = typeof(MvcRouteHandler)
			});

			routeCollection.Add(new Route
			{
				Url = controllerPath,
				Defaults = BuildDefaults(RestfulAction.Index, controller),
				Validation = new { Method = "GET" },
				RouteHandler = typeof(MvcRouteHandler)
			});

			routeCollection.Add(new Route
			{
				Url = controllerPath,
				Defaults = BuildDefaults(RestfulAction.Create, controller),
				Validation = new { Method = "POST" },
				RouteHandler = typeof(MvcRouteHandler)
			});
		}

		/// <summary>Ensures that a <see cref="IRestfulActionResolver"/> exists.</summary>
		/// <param name="serviceProvider">The <see cref="IHttpContext"/> as an <see cref="IServiceProvider"/> to try and use to resolve an instance of the <see cref="IRestfulActionResolver"/></param>
		/// <remarks>If no <see cref="IRestfulActionResolver"/> can be resolved the default <see cref="RestfulActionResolver"/> is used.</remarks>
		private void EnsureActionResolver(IServiceProvider serviceProvider)
		{
			if (_actionResolver == null)
			{
				_actionResolver = (IRestfulActionResolver)serviceProvider.GetService(typeof(IRestfulActionResolver));
				if (_actionResolver == null)
					_actionResolver = new RestfulActionResolver();
			}
		}

		/// <summary>Fixes an area prefix for the route url.</summary>
		/// <param name="path">The area prefix to fix.</param>
		/// <returns>A non null string with leading and trailing /'s stripped</returns>
		private static string FixPath(string path)
		{
			if (path == null)
			{
				return "";
			}
			return path.Trim().Trim(new char[]{'/'});
		}

		/// <summary>Builds a Default object for a route.</summary>
		/// <param name="action">The default action for the route.</param>
		/// <param name="controller">The default controller for the route.</param>
		/// <returns>An Anonymous Type with a default Action property and and default Controller property
		/// if <paramref name="controller"/> is not null or empty.</returns>
		private static object BuildDefaults(RestfulAction action, string controller)
		{
			if (string.IsNullOrEmpty(controller))
				return new { Action = (action == RestfulAction.None) ? "" : action.ToString() };
			return new { Action = (action == RestfulAction.None) ? "" : action.ToString(), Controller = controller };
		}
	}
}
