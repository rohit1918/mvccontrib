<%@ Application Inherits="System.Web.HttpApplication" Language="C#" %>
<script RunAt="server">
	protected void Application_Start(object sender, EventArgs e)
	{
		RouteTable.Routes.Add(new Route
		{
			Url = "[controller]/[action]/[id]",
			Defaults = new { action = "Index", id = (string)null },
			RouteHandler = typeof(MvcRouteHandler)
		});

		RouteTable.Routes.Add(new Route
		{
			Url = "Default.aspx",
			Defaults = new { controller = "Shipment", action = "Index", id = (string)null },
			RouteHandler = typeof(MvcRouteHandler)
		});
	}
</script>
