using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.UI.Html.Grid;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.UI.Html
{
	[TestFixture]
	public class GridParamsBinderTester
	{
		private MockRepository _mocks;
		private HttpContextBase _context;
		private HtmlHelper _helper;
		private ViewContext _viewContext;
		
		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_context = _mocks.DynamicHttpContextBase();
			SetupResult.For(_context.Request.FilePath).Return("Test.mvc");
			var view = _mocks.DynamicMock<IView>();
			_viewContext = new ViewContext(_context, new RouteData(), _mocks.DynamicMock<ControllerBase>(), view, new ViewDataDictionary(), null);
			_helper = new HtmlHelper(_viewContext, new ViewPage());
			
			_mocks.ReplayAll();

		}
		
		[Test]
		public void GridParams_should_use_default_if_no_page_size_specified()
		{
			var gridParams = new GridParams();
			Assert.AreEqual(gridParams.PageSize, GridParams.DefaultPageSize);
		}

		[Test]
		public void GridParams_should_use_specified()
		{
			var gridParams = new GridParams();
			gridParams.PageSize = 4;
			Assert.AreEqual(gridParams.PageSize, 4);
		}

		[Test]
		public void GridParamsBinder_ctor_should_set_UseQueryKeys()
		{
			var binder_default = new GridParamsBinder();
			var binder_true = new GridParamsBinder(true);
			var binder_false = new GridParamsBinder(false);

			Assert.IsFalse(binder_default.UseQueryKeys);
			Assert.IsTrue(binder_true.UseQueryKeys);
			Assert.IsFalse(binder_false.UseQueryKeys);
		}

		private ModelBindingContext CreateContext(Type type, string modelName)
		{
			return new ModelBindingContext(_viewContext, MockRepository.GenerateStub<IValueProvider>(), type, modelName, null, new ModelStateDictionary(), null);
		}


		[Test]
		public void Binding_produces_GridParams()
		{
			var binder = new GridParamsBinder();
			var context = CreateContext(typeof(GridParams), "model");
			ModelBinderResult model = binder.BindModel(context);
			Assert.IsInstanceOfType(typeof(GridParams), model.Value); 
			GridParams gridParams = (GridParams)model.Value;
			Assert.AreEqual(gridParams.PageNumber, 1);
			Assert.AreEqual(gridParams.SortColumn, null);
		}

		[Test]
		public void Can_bind_column_and_sort_data()
		{
			var binder = new GridParamsBinder();
			var context = CreateContext(typeof(GridParams), "model");
			_viewContext.HttpContext.Request.QueryString["page"] = "2";
			_viewContext.HttpContext.Request.QueryString["sdir"] = "true";
			_viewContext.HttpContext.Request.QueryString["sort"] = "Name";

			ModelBinderResult model = binder.BindModel(context);
			Assert.IsInstanceOfType(typeof(GridParams), model.Value);
			GridParams gridParams = (GridParams)model.Value;
			Assert.AreEqual(gridParams.PageNumber, 2);
			Assert.AreEqual(gridParams.SortColumn, "Name");
			Assert.AreEqual(gridParams.SortDescending, true);
		}

		[Test]
		public void Can_bind_to_QueryKey()
		{
			var binder = new GridParamsBinder(true);
			var context = CreateContext(typeof(GridParams), "model");
			_viewContext.HttpContext.Request.QueryString["page_model"] = "2";
			_viewContext.HttpContext.Request.QueryString["sdir_model"] = "true";
			_viewContext.HttpContext.Request.QueryString["sort_model"] = "Name";

			ModelBinderResult model = binder.BindModel(context);
			Assert.IsInstanceOfType(typeof(GridParams), model.Value);
			GridParams gridParams = (GridParams)model.Value;
			Assert.AreEqual(gridParams.PageNumber, 2);
			Assert.AreEqual(gridParams.SortColumn, "Name");
			Assert.AreEqual(gridParams.SortDescending, true);
		}

		[Test]
		public void Can_bind_to_custom_param_names()
		{
			var binder = new GridParamsBinder();
			binder.PageQueryName = "bob";
			binder.SortQueryDirName = "joe";
			binder.SortQueryColName = "fred";

			var context = CreateContext(typeof(GridParams), "model");
			_viewContext.HttpContext.Request.QueryString["bob"] = "2";
			_viewContext.HttpContext.Request.QueryString["joe"] = "true";
			_viewContext.HttpContext.Request.QueryString["fred"] = "Name";

			ModelBinderResult model = binder.BindModel(context);
			Assert.IsInstanceOfType(typeof(GridParams), model.Value);
			GridParams gridParams = (GridParams)model.Value;
			Assert.AreEqual(gridParams.PageNumber, 2);
			Assert.AreEqual(gridParams.SortColumn, "Name");
			Assert.AreEqual(gridParams.SortDescending, true);
		}


	}
}
