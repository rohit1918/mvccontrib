<%@ Import namespace="System.Web.Routing"%>
<%@ Application Inherits="System.Web.HttpApplication" Language="C#" %>
<script RunAt="server">
	protected void Application_Start(object sender, EventArgs e)
	{
		 RouteTable.Routes.Add(new Route("{controller}/{action}/{id}", new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(new { action = "Index", id = "" }),
            });

			RouteTable.Routes.Add(new Route("Default.aspx", new MvcRouteHandler())
			{
				Defaults = new RouteValueDictionary(new { controller="Shipment", action = "Index", id = "" }),
			});
	}
</script>
