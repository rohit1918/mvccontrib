using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcContrib.UnitTests.ComponentControllerFactories
{
	public class SampleComponentController2 : ComponentController
	{
		public SampleComponentController2(INumberService numberService)
		{
			this.numberService = numberService;
		}

		private readonly INumberService numberService;

		public void ShowNumbersBetween(int low,int high)
		{
			var data = numberService.GetNumbersBetween(2, 4);
			
		}
	}
}
