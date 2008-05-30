<%@ Page Language="C#" Inherits="MvcContrib.Samples.Views.ShipmentTrack" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content ContentPlaceHolderId="childContent" runat="server">
	You are tracking packages:<br />
	<% int i = 0; %>
	<% foreach( string trackingNumber in ViewData.Model ) { %>
		<%= i++ %> <%= trackingNumber %><br />
	<% } %>
</asp:Content>