<%@ Page Language="C#" Inherits="MvcContrib.Samples.Views.ShipmentTrack" %>
<asp:Content ContentPlaceHolderId="childContent" runat="server">
	You are tracking packages:<br />
	<% int i = 0; %>
	<% foreach( string trackingNumber in ViewData ) { %>
		<%= i++ %> <%= trackingNumber %><br />
	<% } %>
</asp:Content>