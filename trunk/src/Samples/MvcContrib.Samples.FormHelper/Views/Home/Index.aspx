<%@ Page Language="C#" MasterPageFile="~/Shared/Layout.master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="MvcContrib.Samples.FormHelper.Models" %>
<asp:Content ContentPlaceHolderID="body" runat="server">
	<p>The UI Helpers sample application currently has two helpers:</p>
	<ul>
		<li><%= Html.ActionLink("Form Helper", "FormHelper") %></li>
		<li><%= Html.ActionLink("Validation Helper", "ValidationHelper") %></li>
		<li><%= Html.ActionLink("Grid", "Grid") %></li>
	</ul>
</asp:Content>
