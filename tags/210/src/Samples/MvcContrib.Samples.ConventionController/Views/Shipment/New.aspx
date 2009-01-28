<%@ Page Language="C#" Inherits="MvcContrib.Samples.Views.ShipmentNew" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content ContentPlaceHolderId="childContent" runat="server">
	You created a new Shipment:<br />
	<span style="font-weight: bolder;">Ship To</span>
		<div>
			<%= ViewData.Model.ShipTo.Name %><br />
			<%= ViewData.Model.ShipTo.StreetAddress %><br />
			<%= ViewData.Model.ShipTo.City %>, <%= ViewData.Model.ShipTo.StateProvince %> <%= ViewData.Model.ShipTo.ZipPostalCode %><br />
			<%= ViewData.Model.ShipTo.Country %><br />
		</div>
		<span style="font-weight: bolder;">Dimensions</span>
		<div>
			<%= ViewData.Model.Dimensions.Length %>L, <%= ViewData.Model.Dimensions.Width %>W, <%= ViewData.Model.Dimensions.Height %>H <%= ViewData.Model.Dimensions.Units.ToString() %><br />
		</div>
		</div>
</asp:Content>