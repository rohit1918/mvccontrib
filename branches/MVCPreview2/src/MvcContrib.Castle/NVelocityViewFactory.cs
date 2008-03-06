using System;
using System.Collections;
using System.IO;
using System.Web.Mvc;
using Commons.Collections;
using MvcContrib.Castle;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;

namespace MvcContrib.Castle
{
	public class NVelocityViewFactory : IViewEngine
	{
		private static readonly IDictionary DEFAULT_PROPERTIES = new Hashtable();
		private readonly VelocityEngine _engine;
		private readonly string _masterFolder;

		static NVelocityViewFactory()
		{
			string targetViewFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "views");
			DEFAULT_PROPERTIES.Add(RuntimeConstants.RESOURCE_LOADER, "file");
			DEFAULT_PROPERTIES.Add(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, targetViewFolder);
			DEFAULT_PROPERTIES.Add("master.folder", "masters");
		}

		public NVelocityViewFactory() : this(DEFAULT_PROPERTIES)
		{
		}

		public NVelocityViewFactory(IDictionary properties)
		{
			if( properties == null ) properties = DEFAULT_PROPERTIES;

			ExtendedProperties props = new ExtendedProperties();
			foreach(string key in properties.Keys)
			{
				props.AddProperty(key, properties[key]);
			}

			_masterFolder = props.GetString("master.folder", string.Empty);

			_engine = new VelocityEngine();
			_engine.Init(props);
		}

		private Template ResolveMasterTemplate(string masterName)
		{
			Template masterTemplate = null;

			if(!string.IsNullOrEmpty(masterName))
			{
				string targetMaster = Path.Combine(_masterFolder, masterName);

				if(!Path.HasExtension(targetMaster))
				{
					targetMaster += ".vm";
				}

				if(!_engine.TemplateExists(targetMaster))
				{
					throw new InvalidOperationException("Could not find view for master template named " + masterName +
					                                    ". I searched for '" + targetMaster + "' file. Maybe the file doesn't exist?");
				}

				masterTemplate = _engine.GetTemplate(targetMaster);
			}
			return masterTemplate;
		}

		private Template ResolveViewTemplate(string controllerFolder, string viewName)
		{
			string targetView = Path.Combine(controllerFolder, viewName);

			if(!Path.HasExtension(targetView))
			{
				targetView += ".vm";
			}

			if(!_engine.TemplateExists(targetView))
			{
				throw new InvalidOperationException("Could not find view " + viewName +
				                                    ". I searched for '" + targetView + "' file. Maybe the file doesn't exist?");
			}

			return _engine.GetTemplate(targetView);
		}

	    public void RenderView(ViewContext viewContext)
	    {
			string controllerName = (string)viewContext.RouteData.Values["controller"];
			string controllerFolder = controllerName;

			Template viewTemplate = ResolveViewTemplate(controllerFolder, viewContext.ViewName);
			Template masterTemplate = ResolveMasterTemplate(viewContext.MasterName);

	    	NVelocityView view = new NVelocityView(viewTemplate, masterTemplate, viewContext);
			view.RenderView();
	    }
	}
}
