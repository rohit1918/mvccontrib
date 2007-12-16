using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using NVelocity;

namespace MvcContrib.Castle
{
	public class NVelocityView : IView
	{
		private readonly ControllerContext _controllerContext;
		private readonly Template _masterTemplate;
		private readonly object _viewData;
		private readonly Template _viewTemplate;

		public NVelocityView(Template viewTemplate, Template masterTemplate,
		                     ControllerContext controllerContext, object viewData)
		{
			_viewTemplate = viewTemplate;
			_masterTemplate = masterTemplate;
			_controllerContext = controllerContext;
			_viewData = viewData;
		}

		public Template ViewTemplate
		{
			get { return _viewTemplate; }
		}

		public Template MasterTemplate
		{
			get { return _masterTemplate; }
		}

		public void RenderView(ViewContext viewContext)
		{
			bool hasLayout = _masterTemplate != null;
			TextWriter writer = hasLayout ? new StringWriter() : viewContext.HttpContext.Response.Output;

			VelocityContext context = CreateContext(viewContext);

			_viewTemplate.Merge(context, writer);

			if(hasLayout)
			{
				context.Put("childContent", (writer as StringWriter).GetStringBuilder().ToString());

				_masterTemplate.Merge(context, viewContext.HttpContext.Response.Output);
			}
		}

		private VelocityContext CreateContext(ViewContext context)
		{
			Hashtable entries = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
			IDictionary<string, object> viewDataEntries = _viewData as IDictionary<string, object>;

			if(viewDataEntries != null)
			{
				foreach(KeyValuePair<string, object> pair in viewDataEntries)
				{
					entries[pair.Key] = pair.Value;
				}
			}
			else
			{
				entries["viewdata"] = _viewData;
			}

			entries["routedata"] = context.RouteData;
			entries["controller"] = _controllerContext.Controller;
			entries["httpcontext"] = _controllerContext.HttpContext;

			CreateAndAddHelpers(entries, context);

			return new VelocityContext(entries);
		}

		private static void CreateAndAddHelpers(Hashtable entries, ViewContext context)
		{
			entries["html"] = entries["htmlhelper"] = new HtmlHelper(context);
			entries["url"] = entries["urlhelper"] = new UrlHelper(context);
		}
	}
}