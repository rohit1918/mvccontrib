<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<SampleModel>" %>
<%@ Import Namespace="Web.Models"%>
<%@ Import Namespace="MvcContrib.UI.InputBuilder"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	InputForm Sample
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Html.InputForm() Template Sample</h2>
            <%=Html.InputForm("Home","Save")%>
            
     <pre>
            &lt;%=Html.InputForm("Home","Save")%&gt;
     </pre>
</asp:Content>
