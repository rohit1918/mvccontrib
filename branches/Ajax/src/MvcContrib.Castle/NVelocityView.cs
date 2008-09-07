using System;
using System.Collections;
using System.IO;
using System.Web.Mvc;
using NVelocity;

namespace MvcContrib.Castle
{
	public class NVelocityView : IViewDataContainer, IView
	{
		private ViewContext _viewContext;
		private readonly Template _masterTemplate;
		private readonly Template _viewTemplate;

		public NVelocityView(Template viewTemplate, Template masterTemplate)
		{
			_viewTemplate = viewTemplate;
			_masterTemplate = masterTemplate;
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

		public void Render(ViewContext viewContext, TextWriter writer)
		{
			_viewContext = viewContext;
			bool hasLayout = _masterTemplate != null;

			TextWriter writerToUse = hasLayout ? new StringWriter() : writer;

			VelocityContext context = CreateContext();

			_viewTemplate.Merge(context, writerToUse);

			if(hasLayout)
			{
				context.Put("childContent", (writerToUse as StringWriter).GetStringBuilder().ToString());

				_masterTemplate.Merge(context, writer);
			}
		}

		private VelocityContext CreateContext()
		{
			var entries = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
			if (_viewContext.ViewData != null)
			{
				foreach(var pair in _viewContext.ViewData)
				{
					entries[pair.Key] = pair.Value;
				}
			}
			entries["viewdata"] = _viewContext.ViewData;

			entries["routedata"] = _viewContext.RouteData;
			entries["controller"] = _viewContext.Controller;
			entries["httpcontext"] = _viewContext.HttpContext;

			CreateAndAddHelpers(entries);

			return new VelocityContext(entries);
		}

		private void CreateAndAddHelpers(Hashtable entries)
		{
			entries["html"] = entries["htmlhelper"] = new HtmlExtensionDuck(_viewContext, this);
			entries["url"] = entries["urlhelper"] = new UrlHelper(_viewContext);
		}
	}
}
