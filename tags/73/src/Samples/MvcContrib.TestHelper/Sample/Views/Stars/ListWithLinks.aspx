<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="ListWithLinks.aspx.cs" Inherits="MvcTestingFramework.Sample.Views.Stars.ListWithLinks" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<%@ Import Namespace="MvcTestingFramework.Sample.Controllers" %>

<h2>Stars</h2>

<ul>
    <% foreach (var star in ViewData) { %>
        <li>
            <%= star.Name %> approx: <%= star.Distance %> AU <a href="<%= star.NearbyLink%>">Nearby Stars</a>
        </li>
    <% } %>
</ul>

</asp:Content>