using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcContrib.UnitTests.ComponentControllerFactories
{
	public class SampleComponentController:ComponentController
	{
		public void ShowMeTheNumbers(int i,int y)
		{
			this.RenderedHtml = "sample";
		}
	}
}
