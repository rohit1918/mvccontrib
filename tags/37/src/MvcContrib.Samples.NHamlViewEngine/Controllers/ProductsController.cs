using System;
using System.Web.Mvc;
using MvcContrib;
using Mindscape.NHaml.Tests.TestApp.Models;
using MvcContrib.Attributes;
using MvcContrib.Samples.Models;

namespace MvcContrib.Samples.Controllers
{
	public class ProductsController : ConventionController
	{
		NorthwindDataContext northwind = new NorthwindDataContext();

		//
		// Products/Category/1

		[ControllerAction]
		public void Category(int id)
		{
			Category category = northwind.GetCategoryById(id);

			RenderView("List", category);
		}

		//
		// Products/New

		[ControllerAction]
		public void New()
		{
			ProductsNewViewData viewData = new ProductsNewViewData();

			viewData.Suppliers = northwind.GetSuppliers();
			viewData.Categories = northwind.GetCategories();

			RenderView("New", viewData);
		}

		//
		// Products/Create

		[ControllerAction]
		public void Create([Deserialize("product")] Product product)
		{
			RedirectToAction(new { Action = "Category", ID = product.CategoryID });
		}

		//
		// Products/Edit/5

		[ControllerAction]
		public void Edit(int id)
		{
			ProductsEditViewData viewData = new ProductsEditViewData();

			viewData.Product = northwind.GetProductById(id);
			viewData.Categories = northwind.GetCategories();
			viewData.Suppliers = northwind.GetSuppliers();

			RenderView("Edit", viewData);
		}

		//
		// Products/Update/5

		[ControllerAction]
		public void Update([Deserialize("product")] Product product)
		{
			RedirectToAction(new { Action = "Category", ID = product.CategoryID });
		}
	}
}