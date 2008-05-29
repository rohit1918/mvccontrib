using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using NVelocity;

namespace MvcContrib.Castle
{
	public class NVelocityView : IViewDataContainer
	{
		private readonly ViewContext _viewContext;
		private readonly Template _masterTemplate;
		private readonly Template _viewTemplate;

		public NVelocityView(Template viewTemplate, Template masterTemplate, ViewContext viewContext)
		{
			_viewTemplate = viewTemplate;
			_masterTemplate = masterTemplate;
			_viewContext = viewContext;
		}

		public Template ViewTemplate
		{
			get { return _viewTemplate; }
		}

		public Template MasterTemplate
		{
			get { return _masterTemplate; }
		}

		public ViewDataDictionary ViewData
		{
			get { return _viewContext.ViewData; }
			set { throw new NotSupportedException(); }
		}

		public void RenderView()
		{
			bool hasLayout = _masterTemplate != null;
			TextWriter writer = hasLayout ? new StringWriter() : _viewContext.HttpContext.Response.Output;

			VelocityContext context = CreateContext(_viewContext);

			_viewTemplate.Merge(context, writer);

			if(hasLayout)
			{
				context.Put("childContent", (writer as StringWriter).GetStringBuilder().ToString());

				_masterTemplate.Merge(context, _viewContext.HttpContext.Response.Output);
			}
		}

		private VelocityContext CreateContext(ViewContext context)
		{
			Hashtable entries = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
			IDictionary<string, object> viewDataEntries = _viewContext.ViewData as IDictionary<string, object>;

			if(viewDataEntries != null)
			{
				foreach(KeyValuePair<string, object> pair in viewDataEntries)
				{
					entries[pair.Key] = pair.Value;
				}
			}
			else
			{
				entries["viewdata"] = _viewContext.ViewData;
			}

			entries["routedata"] = context.RouteData;
			entries["controller"] = _viewContext.Controller;
			entries["httpcontext"] = _viewContext.HttpContext;

			CreateAndAddHelpers(entries, context);

			return new VelocityContext(entries);
		}

		private void CreateAndAddHelpers(Hashtable entries, ViewContext context)
		{
			entries["html"] = entries["htmlhelper"] = new HtmlExtensionDuck(context, this);
			entries["url"] = entries["urlhelper"] = new UrlHelper(context);
		}
	}
}