using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib;
using MvcContrib.ActionResults;
using MvcContrib.Attributes;
using MvcContrib.Samples.Models;
using MvcContrib.Filters;

namespace MvcContrib.Samples.Controllers
{
	public class ShipmentController : ConventionController
	{
		public ActionResult Index()
		{
			return View("index");
		}

		public ActionResult New([Deserialize("shipment")] Shipment newShipment)
		{
			return View("new", newShipment);
		}

		[Rescue("Error")]
		[RegExPreconditionFilter("id", PreconditionFilter.ParamType.RouteData, ".+", typeof(ArgumentException))]
		public ActionResult TrackSingle(string id)
		{
			return View("track", new List<string>() { id });
		}

		public ActionResult Track([Deserialize("trackingNumbers")] string[] trackingNumbers)
		{
			List<string> validTrackingNumbers = new List<string>();
			foreach (string trackingNumber in trackingNumbers)
			{
				if (!string.IsNullOrEmpty(trackingNumber))
				{
					validTrackingNumbers.Add(trackingNumber);
				}
			}

			return View("track", validTrackingNumbers);
		}

		[Rescue("Error")]
		public ActionResult ToTheRescue()
		{
			throw new InvalidOperationException();
		}

		[Rescue("Error")]
		public ActionResult DivideByZero()
		{
			int j = 5;
			int f = 0;
			int y = j / f; //throw new DivideByZeroException(); // displays DivideByZeroException.aspx
			return null;
		}

		[NonAction]
		public ActionResult Hidden()
		{
			return Content("This action cannot be called.");
		}

		[DefaultAction]
		public ActionResult DefaultAction()
		{
			string originalAction = RouteData.Values["action"].ToString();
			return Content(string.Format("You tried to access action '{0}' but it does not exit.", originalAction));
		}

		public XmlResult XmlAction()
		{
			Dimension[] dims = new Dimension[] {
			                   		new Dimension{Height=2,Length=1,Units=UnitOfMeasure.English},
									new Dimension{Height=6,Length=8,Units=UnitOfMeasure.Metric},
								};
			return Xml(dims);
		}
	}
}
