﻿<%@ Page Language="C#" MasterPageFile="~/Shared/Layout.master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ContentPlaceHolderID="body" runat="server">
	<p>
		FormHelper currently supports generating the following HTML elements:
		<ul>
			<li>Text fields</li>
			<li>Text areas</li>
			<li>Hidden fields</li>
			<li>Checkbox fields</li>
			<li>Dropdown lists &amp; listboxes</li>
			<li>Radio fields</li>
			<li>Checkbox Lists</li>
			<li>Radio Lists</li>
			<li>Submit buttons</li>			
		</ul>
		<p>
			Most of the helpers have three overloads - the simplest takes the element's name. The second can take an IDictionary of HTML attributes and the third takes a strongly typed object for the HTML attributes.
		</p>
	</p>
	
	<h1>Text Fields</h1>
	<p>
		The name given to the text box is used for databinding. This example automatically extracts the "Name" property from the "person" object in the ViewData:<br />
		<%= Html.Form().TextField("person.Name") %><br /><br />
		HTML attributes can be specified using a dictionary or a strongly typed object:<br />
		Using a dictionary: <%= Html.Form().TextField("person.Name", new Hash(@class => "demo1")) %><br />
		Using strongly typed options: <%= Html.Form().TextField(new MvcContrib.UI.Tags.TextBox { Name = "person.Name", Class = "demo1" }) %>
		<br />
		<p>The same applies to text areas:</p>
		<%= Html.Form().TextArea("person.Name", new Hash(rows => 10, cols => 40)) %>
	</p>
	<h1>Hidden fields</h1>
	<p>Hidden fields can be created using a similar syntax (view source to see generated HTML):</p>
	<%= Html.Form().HiddenField("person.Id") %>
	<h1>Checkbox fields</h1>
	<p>Note that for checkbox fields, a hidden field is also generated with the same name to allow for easier databinding.</p>
	<p>Is user a developer? <%= Html.Form().CheckBoxField("person.IsDeveloper") %></p>
	<h1>Dropdown (select)</h1>
	<p>
		A dropdown takes an enumerable object as its DataSource parameter. The TextField and ValueField properties are used for databinding: <br />
		<%= Html.Form().Select("person.RoleId", ViewData["roles"], "Name", "Id", new Hash(firstOption => "Please select...")) %>
		<br /><br />
		The options can also be strongly typed.
		
	</p>
	
	<h1>Checkbox Lists and Radio Lists</h1>
	<p>Checkbox lists and radio lists can be generated in one of three ways. The standard method outputs a horizontal list:</p>
	<%= Html.Form().CheckBoxList("accessLevel", ViewData["roles"], "Name", "Id") %>
	<p>You can use the ToFormattedString method to create a vertical list:</p>
	<%= Html.Form().CheckBoxList("accessLevel2", ViewData["roles"], "Name", "Id").ToFormattedString("{0}<br />") %>
	<p>You can also iterate over the checkbox list to manually manipulate each checkbox/radio button</p>
	<% foreach(var checkbox in Html.Form().CheckBoxList("accessLevel3", ViewData["roles"], "Name", "Id")) { %>
		<% if(checkbox.Value.Equals("2")) { checkbox.Checked = true; }%>	
		<%= checkbox %>
	<% } %>	
</asp:Content>
