<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Layout.master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="MvcContrib.Samples.FormHelper.Models" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<%= Html.Validation().ValidatorRegistrationScripts() %>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
	<h1>Validators</h1>
	<p>Validators utilize the ASP.NET client site validation scripts. It requires that you call <b>Form.ValidatorRegistrationScripts</b> within the head element, 
		<b>Form.ValidatorInitializationScripts</b> at the end of your body element, and when creating your form call <b>Form.FormValidation</b> as the htmlAttributes parameter. 
		You can also specify the different validation groups when creating your form validation and validators. You'll notice this was done below to separate the examples.</p>
		
	<% using (Html.BeginForm("ValidationHelper", "Home", FormMethod.Post, Html.Validation().FormValidation("required"))) { %>
		<p>You can use a <b>Required Validator:</b> (click the Validate button)</p>
		Name: <%= Html.TextBox("nameForRequired") %>
		<%= Html.Validation().RequiredValidator("nameForRequiredValidator", "nameForRequired", "You must supply a name.", "required") %>
		<br />
		<%= Html.SubmitButton("submitRequired", "Validate") %>
	<% } %>
	
	<% using (Html.BeginForm("ValidationHelper", "Home", FormMethod.Post, Html.Validation().FormValidation("regex"))) { %>
		<p>You can use a <b>Regular Expression Validator:</b> (type any non alpha character)</p>
		Name: <%= Html.TextBox("nameForRegex") %>
		<%= Html.Validation().RegularExpressionValidator("nameForRegexValidator", "nameForRegex", "[a-zA-Z]*", "The name can only contain alpha characters.", "regex") %>
		<br />
		<%= Html.SubmitButton("submitRegex", "Validate") %>
	<% } %>
	
	<% using (Html.BeginForm("ValidationHelper", "Home", FormMethod.Post, Html.Validation().FormValidation("range"))) { %>
		<p>You can use a <b>Range Validator:</b> (type a number less than 18 or greater than 35)</p>
		Age: <%= Html.TextBox("ageForRange") %>
		<%= Html.Validation().RangeValidator("ageForRangeValidator", "ageForRange", "18", "35", ValidationDataType.Integer, "You do not meet the target age range of 18-35.", "range") %>
		<br />
		<%= Html.SubmitButton("submitRange", "Validate") %>
	<% } %>
	
	<% using (Html.BeginForm("ValidationHelper", "Home", FormMethod.Post, Html.Validation().FormValidation("compare"))) { %>
		<p>You can use a <b>Compare Validator:</b> (type the same text in each)</p>
		First: <%= Html.TextBox("firstCompare") %>
		Second: <%= Html.TextBox("secondCompare") %>
		<%= Html.Validation().CompareValidator("compareValidator", "firstCompare", "secondCompare", ValidationDataType.String, ValidationCompareOperator.NotEqual, "You cannot specify the same text for each.", "compare") %>
		<br />
		<%= Html.SubmitButton("submitCompare", "Validate") %>
	<% } %>
	
	<% using (Html.BeginForm("ValidationHelper", "Home", FormMethod.Post, Html.Validation().FormValidation("custom"))) { %>
		<p>You can use a <b>Custom Validator:</b> (type anything but 'mvc')</p>
		<script type="text/javascript">
			function ValidateTextEquals(source, args) { 
				args.IsValid = (args.Value == 'mvc');
			}
		</script>
		Text: <%= Html.TextBox("textCustom") %>
		<%= Html.Validation().CustomValidator("textCustomValidator", "textCustom", "ValidateTextEquals", "The text must equal 'mvc'.", "custom") %>
		<br />
		<%= Html.SubmitButton("submitCustom", "Validate") %>
	<% } %>
	
	<%= Html.Validation().ValidatorInitializationScripts() %>
</asp:Content>
