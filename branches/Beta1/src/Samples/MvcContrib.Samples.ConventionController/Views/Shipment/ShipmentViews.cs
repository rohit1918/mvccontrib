using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.Samples.Models;

namespace MvcContrib.Samples.Views
{
	public class ShipmentIndex : ViewPage
	{
	}

	public class ShipmentNew : ViewPage<Shipment>
	{
	}

	public class ShipmentTrack : ViewPage<List<string>>
	{
	}
}
