<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Person>>" %>
<%@ Import Namespace="MvcContrib.Samples.UI.Models"%>
<%@ Import Namespace="MvcContrib.UI.Grid"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Grid Sections</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Grid with Custom Sections Example</h2>
    <p>
    Note that in this example, the grid is rendered with ToString using the &lt%= tag.   <br />  
    You can also render the grid using &lt% Html.Grid(...).Render(); %&gt
    </p>
    
	<%= Html.Grid(Model)
		.Columns(column => {
     		column.For(x => x.Id).Named("Person ID");
     		column.For(x => x.Name);
     		column.For(x => x.Gender);
     		column.For(x => x.DateOfBirth).Format("{0:d}").HeaderPartial("DateOfBirthHeaderPartial");
			column.For("View Person").Named("").Partial("ViewPersonPartial");
     	}) %>

</asp:Content>
