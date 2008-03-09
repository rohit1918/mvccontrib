<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
    CodeBehind="List.aspx.cs" Inherits="MvcContrib.TestHelper.Sample.Views.Stars.List" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<%@ Import Namespace="MvcContrib.TestHelper.Sample.Controllers" %>

<h2>Stars</h2>

<ul>
    <% foreach (var star in ViewData) { %>
        <li>
            <%= star.Name %> approx: <%= star.Distance %> AU
        </li>
    <% } %>
</ul>
<%using(Html.Form<StarsController>(action=>action.AddFormStar())){%>
    <%= Html.TextBox("NewStarName") %>
    <%= Html.SubmitButton("FormSubmit", "AddFormStar") %>
<%}%>

<%using(Html.Form<StarsController>(action=>action.AddSessionStar())){%>
    <%= Html.TextBox("NewStarName") %>
    <%= Html.SubmitButton("SessionSubmit", "AddSessionStar") %>
<%}%>
Form: <%= Html.ViewContext.TempData["NewStarName"] != null? Html.ViewContext.TempData["NewStarName"]:"" %>
Session: <%= Html.ViewContext.HttpContext.Session["NewStarName"] != null ? Html.ViewContext.HttpContext.Session["NewStarName"] : ""%>

</asp:Content>