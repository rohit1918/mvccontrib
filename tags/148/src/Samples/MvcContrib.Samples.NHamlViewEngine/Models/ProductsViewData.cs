using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcContrib.Samples.NHamlViewEngine.Models
{
	public class ProductsEditViewData
	{
		public Product Product { get; set; }
		public SelectList Suppliers { get; set; }
		public SelectList Categories { get; set; }
	}

	public class ProductsNewViewData
	{
		public SelectList Suppliers { get; set; }
		public SelectList Categories { get; set; }
	}
}
