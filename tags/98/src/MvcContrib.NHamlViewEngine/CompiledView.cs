using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;

namespace MvcContrib.NHamlViewEngine
{
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class CompiledView
	{
		private readonly TemplateCompiler _templateCompiler;

		private readonly string _templatePath;
		private readonly string _layoutPath;

		private Type _viewType;

		private readonly object _sync = new object();

		private readonly Dictionary<string, DateTime> _fileTimestamps
			= new Dictionary<string, DateTime>();

		public CompiledView(TemplateCompiler templateCompiler,
		                    string templatePath, string layoutPath, object viewData)
		{
			_templateCompiler = templateCompiler;
			_templatePath = templatePath;
			_layoutPath = layoutPath;

			CompileView(viewData);
		}

		public INHamlView CreateView()
		{
			return (INHamlView)Activator.CreateInstance(_viewType);
		}

		public void RecompileIfNecessary(object viewData)
		{
			lock(_sync)
			{
				foreach(KeyValuePair<string, DateTime> inputFile in _fileTimestamps)
				{
					if(File.GetLastWriteTime(inputFile.Key) > inputFile.Value)
					{
						CompileView(viewData);

						break;
					}
				}
			}
		}

		private void CompileView(object viewData)
		{
            var viewDataType = viewData.GetType();

            if (!NHamlViewFactory.ViewDataIsDictionary(viewData))
            {
                AddReferences(viewDataType);
            }
            else
            {
                viewDataType = typeof(ViewData);
            }

			List<string> inputFiles = new List<string>();

			_viewType = _templateCompiler.Compile(_templatePath, _layoutPath, inputFiles, viewDataType);

			foreach(string inputFile in inputFiles)
			{
				_fileTimestamps[inputFile] = File.GetLastWriteTime(inputFile);
			}
		}

        private void AddReferences(Type type)
        {
            _templateCompiler.AddReference(type.Assembly.Location);

            if (type.IsGenericType)
            {
                foreach (var t in type.GetGenericArguments())
                {
                    AddReferences(t);
                }
            }
        }
	}
}
