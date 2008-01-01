<%@ Page Language="C#" Inherits="MvcContrib.Samples.Views.ShipmentNew" %>
<asp:Content ContentPlaceHolderId="childContent" runat="server">
	You created a new Shipment:<br />
	<span style="font-weight: bolder;">Ship To</span>
		<div>
			<%= ViewData.ShipTo.Name %><br />
			<%= ViewData.ShipTo.StreetAddress %><br />
			<%= ViewData.ShipTo.City %>, <%= ViewData.ShipTo.StateProvince %> <%= ViewData.ShipTo.ZipPostalCode %><br />
			<%= ViewData.ShipTo.Country %><br />
		</div>
		<span style="font-weight: bolder;">Dimensions</span>
		<div>
			<%= ViewData.Dimensions.Length %>L, <%= ViewData.Dimensions.Width %>W, <%= ViewData.Dimensions.Height %>H <%= ViewData.Dimensions.Units.ToString() %><br />
		</div>
		</div>
</asp:Content>