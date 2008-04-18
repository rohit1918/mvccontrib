using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Samples.NHamlViewEngine.Models;

namespace MvcContrib.Samples.NHamlViewEngine.Controllers
{
	public class ProductsController : Controller
	{
		NorthwindDataContext northwind = new NorthwindDataContext(
			ConfigurationManager.ConnectionStrings["NorthwindConnectionString"].ConnectionString);

		//
		// Products/Category/1


		public ActionResult Category(int id)
		{
			Category category = northwind.GetCategoryById(id);

			return RenderView("List", category);
		}

		//
		// Products/New

		
		public ActionResult New()
		{
			ProductsNewViewData viewData = new ProductsNewViewData();

			viewData.Suppliers = northwind.GetSuppliers();
			viewData.Categories = northwind.GetCategories();

			return RenderView("New", viewData);
		}

		//
		// Products/Create


		public ActionResult Create()
		{
			Product product = new Product();
			BindingHelperExtensions.UpdateFrom(product, Request.Form);

			northwind.AddProduct(product);
			northwind.SubmitChanges();

			return RedirectToAction(new RouteValueDictionary(new { Action = "Category", ID = product.CategoryID }));
		}

		//
		// Products/Edit/5


		public ActionResult Edit(int id)
		{
			ProductsEditViewData viewData = new ProductsEditViewData();

			viewData.Product = northwind.GetProductById(id);
			viewData.Categories = northwind.GetCategories();
			viewData.Suppliers = northwind.GetSuppliers();

			return RenderView("Edit", viewData);
		}

		//
		// Products/Update/5


		public ActionResult Update(int id)
		{
			Product product = northwind.GetProductById(id);
			BindingHelperExtensions.UpdateFrom(product, Request.Form);

			northwind.SubmitChanges();

			return RedirectToAction(new RouteValueDictionary(new { Action = "Category", ID = product.CategoryID }));
		}
	}
}
