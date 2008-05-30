﻿using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;

namespace MvcContrib.NHamlViewEngine
{
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class NHamlView<TViewData> : INHamlView where TViewData : class
	{
		private readonly ICompiledView _compiledView;

		private ViewContext _viewContext;
		private AjaxHelper _ajax;
		private HtmlHelper _html;
		private UrlHelper _url;

		private ViewDataDictionary<TViewData> _viewData = new ViewDataDictionary<TViewData>();

		protected NHamlView()
		{
			_compiledView = (ICompiledView)this;
		}

		public void RenderView(ViewContext context)
		{
			_viewContext = context;

			_ajax = new AjaxHelper(_viewContext);
			_html = new HtmlHelper(_viewContext, this);
			_url = new UrlHelper(_viewContext);

			context.HttpContext.Response.Output.Write(_compiledView.Render());
		}

		public AjaxHelper Ajax
		{
			get { return _ajax; }
		}

		public HtmlHelper Html
		{
			get { return _html; }
		}

		public UrlHelper Url
		{
			get { return _url; }
		}

		ViewDataDictionary IViewDataContainer.ViewData
		{
			get { return _viewData; }
			set { _viewData = (ViewDataDictionary<TViewData>)value; }
		}

		public ViewDataDictionary<TViewData> ViewData
		{
			get { return _viewData; }
		}

		public TViewData Model
		{
			get { return _viewData.Model; }
		}

		public void SetViewData(ViewDataDictionary viewData)
		{
			_viewData = new ViewDataDictionary<TViewData>(viewData);
		}
	}
}
