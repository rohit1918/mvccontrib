using System;
using System.Collections;
using System.IO;
using System.Web.Mvc;
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;

namespace MvcContrib.ViewFactories
{
	public class NVelocityViewFactory : IViewFactory
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

		public IView CreateView(ControllerContext controllerContext, string viewName, string masterName, object viewData)
		{
			string controllerName = (string)controllerContext.RouteData.Values["controller"];
			string controllerFolder = controllerName;

			Template viewTemplate = ResolveViewTemplate(controllerFolder, viewName);
			Template masterTemplate = ResolveMasterTemplate(masterName);

			return new NVelocityView(viewTemplate, masterTemplate, controllerContext, viewData);
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
	}
}