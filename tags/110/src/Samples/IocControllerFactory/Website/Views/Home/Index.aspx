<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Website.Views.Home.Index" %>
<%@ Import namespace="Website.Models"%>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>
        Introduction to IocControllerFactory!</h2>
    <p>
        
    </p>
    <p>
    <ul>
    <%foreach (Link link in this.ViewData)
      {%>
    <li><a target="_blank" href="<%=link.Url%>"><%=link.Title%></a> </li>
    <%}%>
    </ul>
        
    </p>
</asp:Content>
