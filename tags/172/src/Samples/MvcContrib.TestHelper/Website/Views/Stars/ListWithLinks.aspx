<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="ListWithLinks.aspx.cs" Inherits="MvcContrib.TestHelper.Sample.Views.Stars.ListWithLinks" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<%@ Import Namespace="MvcContrib.TestHelper.Sample.Controllers" %>
<h2>Stars</h2>

<ul>
    <% foreach (var star in ViewData.Model) { %>
        <li>
            <%= star.Name %> approx: <%= star.Distance %> AU <%= this.Html.ActionLink("Nearby Stars","ListWithLinks")%>
        </li>
    <% } %>
</ul>

</asp:Content>