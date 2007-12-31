using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using MvcContrib.NHamlViewEngine;
using MvcContrib.NHamlViewEngine.Configuration;
using MvcContrib.NHamlViewEngine.Utilities;

namespace MvcContrib.ViewFactories
{
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class NHamlViewFactory : IViewFactory
	{
		private static readonly Dictionary<string, CompiledView> _viewCache = new Dictionary<string, CompiledView>();

		private static readonly TemplateCompiler _templateCompiler = new TemplateCompiler();

		private static bool _production;

		private readonly IViewSourceLoader _viewSourceLoader;

		[SuppressMessage("Microsoft.Performance", "CA1810")]
		static NHamlViewFactory()
		{
			_templateCompiler.AddUsing("System.Web");
			_templateCompiler.AddUsing("System.Web.Mvc");
			_templateCompiler.AddUsing("MvcContrib.NHamlViewEngine");

			_templateCompiler.ViewBaseType = typeof(NHamlView<>);

			_templateCompiler.AddReference(typeof(IView).Assembly.Location);
			_templateCompiler.AddReference(typeof(DataContext).Assembly.Location);
			_templateCompiler.AddReference(typeof(TextInputExtensions).Assembly.Location);

			LoadConfiguration();
		}

		public NHamlViewFactory()
			: this(new FileSystemViewSourceLoader())
		{
		}

		public NHamlViewFactory(IViewSourceLoader viewSourceLoader)
		{
			if(viewSourceLoader == null) throw new ArgumentNullException("viewSourceLoader");

			_viewSourceLoader = viewSourceLoader;
		}

		private static void LoadConfiguration()
		{
			NHamlViewEngineSection section = NHamlViewEngineSection.Read();

			if(section != null)
			{
				_production = section.Production;

				foreach(AssemblyConfigurationElement cfgAsm in section.Views.Assemblies)
				{
					_templateCompiler.AddReference(Assembly.Load(cfgAsm.Name).Location);
				}

				foreach(NamespaceConfigurationElement cfgNs in section.Views.Namespaces)
				{
					_templateCompiler.AddUsing(cfgNs.Name);
				}
			}
		}

		public IView CreateView(ControllerContext controllerContext, string viewName,
		                        string masterName, object viewData)
		{
			string controller = (string)controllerContext.RouteData.Values["controller"];
			string viewKey = controller + "/" + viewName;

			CompiledView compiledView;

			if(!_viewCache.TryGetValue(viewKey, out compiledView))
			{
				lock(_viewCache)
				{
					if(!_viewCache.TryGetValue(viewKey, out compiledView))
					{
						string viewPath = viewKey;

						if(!Path.HasExtension(viewPath))
						{
							viewPath = string.Concat(viewPath, ".haml");
						}

						IViewSource viewSource = _viewSourceLoader.GetViewSource(viewPath);

						Invariant.IsNotNull(viewSource);

						IViewSource layoutSource = FindLayout("Shared", masterName, controller);

						string layoutPath = null;

						if(layoutSource != null)
						{
							layoutPath = layoutSource.FullName;
						}

						compiledView = new CompiledView(_templateCompiler, viewSource.FullName, layoutPath, viewData);

						_viewCache.Add(viewKey, compiledView);
					}
				}
			}

			if(!_production)
			{
				compiledView.RecompileIfNecessary(viewData);
			}

			INHamlView view = compiledView.CreateView();

			if(ViewDataIsDictionary(viewData))
			{
				viewData = new ViewData(viewData);
			}

			view.SetViewData(viewData);

			return view;
		}

		public static bool ViewDataIsDictionary(object viewData)
		{
			return (viewData != null)
			       && (typeof(IDictionary).IsAssignableFrom(viewData.GetType()));
		}

		protected virtual IViewSource FindLayout(string mastersFolder, string masterName, string controller)
		{
			string masterPath = mastersFolder + "\\" + masterName + ".haml";

			if(_viewSourceLoader.HasView(masterPath))
			{
				return _viewSourceLoader.GetViewSource(masterPath);
			}

			masterPath = mastersFolder + "\\" + controller + ".haml";

			if(_viewSourceLoader.HasView(masterPath))
			{
				return _viewSourceLoader.GetViewSource(masterPath);
			}

			masterPath = mastersFolder + "\\application.haml";

			if(_viewSourceLoader.HasView(masterPath))
			{
				return _viewSourceLoader.GetViewSource(masterPath);
			}

			return null;
		}

		public static void ClearViewCache()
		{
			lock(_viewCache)
			{
				_viewCache.Clear();
			}
		}
	}
}