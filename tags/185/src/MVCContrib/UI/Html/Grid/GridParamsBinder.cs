using System;
using System.Collections.Specialized;
using System.Web.Mvc;

namespace MvcContrib.UI.Html.Grid
{
	///<summary>
	/// Use the GridParamsBinder to bind your GridParams. Add it with 
	/// ModelBinders.Binders.Add(typeof(GridParams),new GridParamsBinder());
	/// This will automatically translate GridParams in your controller actions such as:
	/// public ActionResult Index(GridParams gp) { ...
	///</summary>
	public class GridParamsBinder : IModelBinder
	{
		private string _pageQueryName = "page";
		private string _sortQueryColName = "sort";
		private string _sortQueryDirName = "sdir";

		public GridParamsBinder() : this(false) { }

		public GridParamsBinder(bool useQueryKeys)
		{
			UseQueryKeys = useQueryKeys;
		}

		/// <summary>
		/// Will use the Action argument name to pass the page and sort parameters so multiple grids can be used on the same page
		/// </summary>
		public bool UseQueryKeys { get; set; }

		///<summary>
		/// Used to change the default page parameter name
		///</summary>
		public string PageQueryName { get { return _pageQueryName; } set { _pageQueryName = value; } }
		///<summary>
		/// Used to change the default sort column parameter name
		///</summary>
		public string SortQueryColName { get { return _sortQueryColName; } set { _sortQueryColName = value; } }

		///<summary>
		/// Used to change the default sort direction parameter name
		///</summary>
		public string SortQueryDirName { get { return _sortQueryDirName; } set { _sortQueryDirName = value; } }

		public ModelBinderResult BindModel(ModelBindingContext bindingContext)
		{
			var data = new GridParams();
			string key = "";
			if (UseQueryKeys)
				key = "_" + (data.QueryKey = bindingContext.ModelName);

			NameValueCollection store = bindingContext.HttpContext.Request.Params;
			string page = store[PageQueryName + key];
			int pageNumber = 1;
			if (!string.IsNullOrEmpty(page))
				Int32.TryParse(page, out pageNumber);
			data.PageNumber = pageNumber;

			bool dir = false;
			string sdir = store[SortQueryDirName + key];
			if (!string.IsNullOrEmpty(sdir))
				Boolean.TryParse(sdir, out dir);
			data.SortDescending = dir;
			data.SortColumn = store[SortQueryColName + key];
			return new ModelBinderResult(data);
		}
	}
}
