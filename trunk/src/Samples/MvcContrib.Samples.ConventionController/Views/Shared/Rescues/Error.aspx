<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content ContentPlaceHolderId="childContent" runat="server">
Whoops an error occured! 
<p>Error Message:</p>
<p><%= ((Exception)ViewData.Model).GetBaseException().Message %></p>
</asp:Content>