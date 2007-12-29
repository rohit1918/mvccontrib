using System;
using System.Collections.Generic;
using System.Reflection;
using MvcContrib;
using MvcContrib.Attributes;
using MvcContrib.Samples.Models;

namespace MvcContrib.Samples.Controllers
{
	public class ShipmentController : ConventionController
	{
		public void Index()
		{
			RenderView("index");
		}

		public void New([Deserialize("shipment")] Shipment newShipment)
		{
			RenderView("new", newShipment);
		}

		public void Track([Deserialize("trackingNumbers")] string[] trackingNumbers)
		{
			List<string> validTrackingNumbers = new List<string>();
			foreach(string trackingNumber in trackingNumbers)
			{
				if(!string.IsNullOrEmpty(trackingNumber))
				{
					validTrackingNumbers.Add(trackingNumber);
				}
			}

			RenderView("track", validTrackingNumbers);
		}

		[Rescue("Error")]
		public void ToTheRescue()
		{
			throw new InvalidOperationException();
		}

		protected override bool OnError(string actionName, MethodInfo methodInfo, Exception exception)
		{
			return false;
		}
	}
}