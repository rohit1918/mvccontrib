<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" CodeBehind="DivideByZeroException.aspx.cs" Inherits="MvcContrib.Samples.Views.Shared.Rescues.DivideByZeroException" %>
<asp:Content ContentPlaceHolderId="childContent" runat="server">
You fail at math rescue says: 
<%= ViewData.Model.Exception.GetBaseException().Message %>
</asp:Content>
