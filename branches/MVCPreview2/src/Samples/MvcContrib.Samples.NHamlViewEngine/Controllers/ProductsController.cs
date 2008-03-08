using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Mvc.BindingHelpers;
using MvcContrib.Samples.NHamlViewEngine.Models;

namespace MvcContrib.Samples.NHamlViewEngine.Controllers
{
	public class ProductsController : Controller
	{
		NorthwindDataContext northwind = new NorthwindDataContext(
			ConfigurationManager.ConnectionStrings["NorthwindConnectionString"].ConnectionString);

		//
		// Products/Category/1

		
		public void Category(int id)
		{
			Category category = northwind.GetCategoryById(id);

			RenderView("List", category);
		}

		//
		// Products/New

		
		public void New()
		{
			ProductsNewViewData viewData = new ProductsNewViewData();

			viewData.Suppliers = northwind.GetSuppliers();
			viewData.Categories = northwind.GetCategories();

			RenderView("New", viewData);
		}

		//
		// Products/Create

		
		public void Create()
		{
			Product product = new Product();
			product.UpdateFrom(Request.Form);

			northwind.AddProduct(product);
			northwind.SubmitChanges();

			RedirectToAction(new { Action = "Category", ID = product.CategoryID });
		}

		//
		// Products/Edit/5

		
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

		
		public void Update(int id)
		{
			Product product = northwind.GetProductById(id);
			product.UpdateFrom(Request.Form);

			northwind.SubmitChanges();

			RedirectToAction(new { Action = "Category", ID = product.CategoryID });
		}
	}
}
