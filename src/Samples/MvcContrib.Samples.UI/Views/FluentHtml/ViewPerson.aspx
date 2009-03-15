<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Person>" %>
<%@ Import Namespace="MvcContrib.Samples.UI.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Fluent HTML</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<p>These were the values that were submitted:</p>
	<p>
		<strong>Name:</strong> <%= Model.Name %>
	</p>
	<p>
		<strong>Gender:</strong> <%= Model.Gender %>
	</p>
	<p>
		<strong>Roles:</strong>
		<br />
		<% foreach (var role in Model.Roles) { %>
			<%= role %><br />
		<% } %>
	</p>
</asp:Content>
