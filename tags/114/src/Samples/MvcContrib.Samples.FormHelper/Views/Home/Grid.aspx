<%@ Page Language="C#" MasterPageFile="~/Shared/Layout.master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="MvcContrib.Samples.FormHelper.Models" %>
<%@ Import Namespace="MvcContrib.UI.Html.Grid" %>
<%@ Import Namespace="MvcContrib" %>
<asp:Content ContentPlaceHolderID="body" runat="server">
	<h1>Grid</h1>
	
<%
	Html.Grid<Person>(
		"people", 
		new Hash(empty => "There are no people", style => "width: 100%"),
		column => {
			column.For(p => p.Id, "ID Number");
			column.For(p => p.Name);
			column.For(p => p.Gender);
			column.For(p => p.RoleId).Formatted("Role ID: {0}");
			column.For("Custom Column").Do(p => { %>
				<td>A custom column...</td>	
			<% });
		}
	);
%>
	
</asp:Content>
