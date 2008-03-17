using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib;
using MvcContrib.Attributes;
using MvcContrib.Samples.Models;
using MvcContrib.Filters;

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

		[PostOnly]
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

		[NonAction]
		public void Hidden()
		{
			Response.Write("This action cannot be called.");
		}

		[DefaultAction]
		public void DefaultAction()
		{
			string originalAction = RouteData.Values["action"].ToString();
			Response.Write(string.Format("You tried to access action '{0}' but it does not exit.", originalAction));
		}

		[return: XMLReturnBinder]
		public Dimension[] XmlAction()
		{
			Dimension[] dims = new Dimension[]
			                   	{
			                   		new Dimension{Height=2,Length=1,Units=UnitOfMeasure.English},
									new Dimension{Height=6,Length=8,Units=UnitOfMeasure.Metric},
		};
			return dims;
		}

		protected  bool OnError(string actionName, MethodInfo methodInfo, Exception exception)
		{
			return false;
		}
	}
}
