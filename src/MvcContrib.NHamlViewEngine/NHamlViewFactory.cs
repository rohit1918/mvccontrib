using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using MvcContrib.NHamlViewEngine.Configuration;
using MvcContrib.NHamlViewEngine.Utilities;
using MvcContrib.ViewFactories;
using NHaml;

namespace MvcContrib.NHamlViewEngine
{
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class NHamlViewFactory : IViewEngine
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
			_templateCompiler.AddUsing("System.Web.Routing");
			_templateCompiler.AddUsing("MvcContrib.NHamlViewEngine");

			_templateCompiler.ViewBaseType = typeof(NHamlView<>);
			_templateCompiler.AddReference(typeof(UserControl).Assembly.Location);
			_templateCompiler.AddReference(typeof(RouteValueDictionary).Assembly.Location);
			_templateCompiler.AddReference(typeof(DataContext).Assembly.Location);
//			_templateCompiler.AddReference(typeof(TextInputExtensions).Assembly.Location);

			LoadConfiguration();
		}

		public NHamlViewFactory()
			: this(new FileSystemViewSourceLoader())
		{
		}

		public NHamlViewFactory(IViewSourceLoader viewSourceLoader)
		{
			Invariant.ArgumentNotNull(viewSourceLoader, "viewSourceLoader");

			_viewSourceLoader = viewSourceLoader;
		}

		private static void LoadConfiguration()
		{
			var section = NHamlViewEngineSection.Read();

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

		protected virtual IViewSource FindLayout(string mastersFolder, string masterName, string controller)
		{
			if(!string.IsNullOrEmpty(masterName))
			{
				var requestedPath = mastersFolder + "\\" + masterName + ".haml";

				if(_viewSourceLoader.HasView(requestedPath))
				{
					return _viewSourceLoader.GetViewSource(requestedPath);
				}

				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
				                                                  "Layout {0} was specified but couldn't be found.",
				                                                  masterName));
			}

			var controllerPath = mastersFolder + "\\" + controller + ".haml";

			if(_viewSourceLoader.HasView(controllerPath))
			{
				return _viewSourceLoader.GetViewSource(controllerPath);
			}

			var applicationPath = mastersFolder + "\\application.haml";

			if(_viewSourceLoader.HasView(applicationPath))
			{
				return _viewSourceLoader.GetViewSource(applicationPath);
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

        //public void RenderView(ViewContext viewContext)
        //{
        //    var controller = (string)viewContext.RouteData.Values["controller"];
        //    var viewKey = controller + "/" + viewContext.ViewName;

        //    CompiledView compiledView;

        //    if(!_viewCache.TryGetValue(viewKey, out compiledView))
        //    {
        //        lock(_viewCache)
        //        {
        //            if(!_viewCache.TryGetValue(viewKey, out compiledView))
        //            {
        //                var viewPath = viewKey;

        //                if(!Path.HasExtension(viewPath))
        //                {
        //                    viewPath = string.Concat(viewPath, ".haml");
        //                }

        //                if(!_viewSourceLoader.HasView(viewPath))
        //                {
        //                    throw new InvalidOperationException(
        //                        string.Format(CultureInfo.CurrentCulture,
        //                                      "Couldn't find the template with name {0}.",
        //                                      viewPath));
        //                }

        //                var viewSource = _viewSourceLoader.GetViewSource(viewPath);

        //                Invariant.IsNotNull(viewSource);

        //                var layoutSource = FindLayout("Shared", viewContext.MasterName, controller);

        //                string layoutPath = null;

        //                if(layoutSource != null)
        //                {
        //                    layoutPath = layoutSource.FullName;
        //                }

        //                compiledView = new CompiledView(_templateCompiler, viewSource.FullName, layoutPath,
        //                                                viewContext.ViewData);

        //                _viewCache.Add(viewKey, compiledView);
        //            }
        //        }
        //    }

        //    if(!_production)
        //    {
        //        compiledView.RecompileIfNecessary(viewContext.ViewData);
        //    }

        //    var view = compiledView.CreateView();

        //    view.SetViewData(viewContext.ViewData);
        //    view.RenderView(viewContext);
        //}

        #region IViewEngine Members

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName)
        {
            throw new NotImplementedException();
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName)
        {
            throw new NotImplementedException();
        }

        #endregion

        public void RenderView(ViewContext context)
        {
            throw new NotImplementedException();
        }
    }
}