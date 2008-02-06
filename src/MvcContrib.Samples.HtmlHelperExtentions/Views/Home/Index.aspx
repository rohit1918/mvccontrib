<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="MvcContrib.Samples.HtmlHelperExtentions.Views.Home.Index" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h2>Welcome to my ASP.NET MVC Application!</h2>
    <%=Html.FormText("testingText","a value",new HtmlAttribList(new {OnClick="alert(\"here I am\");", Style="width:100%"})) %><br />
    <%=Html.FormFile("TestFile") %><br />
    <%=Html.FormPassword("TestPassword","a value") %><br />
    <%
        System.Collections.Generic.Dictionary<string,string> Options = new System.Collections.Generic.Dictionary<string,string>();
        Options.Add("Bob","1");
        Options.Add("Jack","2");
        Options.Add("Frank","3");
        Options.Add("Larry","4");
    %>
    <%=Html.FormSelect("A Select List", Options) %><br />
    
    <fieldset>
        <legend>CheckBox List</legend>
        <ul>
    <%string[] CheckList = Html.FormCheckBoxList("CheckList", Options);
      foreach (string checkbox in CheckList)
      {%>
            <li><%=checkbox%></li>
    <%} %>
    </ul></fieldset>
    
        <fieldset>
        <legend>Radio List</legend>
        <ul>
    <%string[] RadioList = Html.FormRadioList("RadioList", Options);
      foreach (string radio in RadioList)
      {%>
            <li><%=radio%></li>
    <%} %>
    </ul></fieldset>
    
    <%=Html.FormButton("aButton", "Click me", new HtmlAttribList(new { OnClick = "alert('I was clicked');" }))%><br />
    <%=Html.FormSubmit("aSubmit")%><br />
    <%=Html.FormReset("aReset")%><br />
    <%=Html.FormImageButton("aImage", "../../Content/forwardbutton.gif")%><br />
</asp:Content>