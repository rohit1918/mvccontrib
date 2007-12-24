using System;
using System.IO;
using System.Web.Mvc;
using System.Xml;
using Mvp.Xml.Common.Xsl;

namespace MvcContrib.XsltViewEngine
{
	public class XsltTemplate
	{
		private readonly string appPath;
		private readonly string controllerName;
		private readonly string viewName;

		public XsltTemplate(string appPath, string controllerName, string viewName)
		{
			this.appPath = appPath;
			this.controllerName = controllerName;
			this.viewName = viewName;
		}

		public XsltTemplate(ControllerContext controller, string viewName) :
			this(controller.HttpContext.Request.PhysicalApplicationPath,
			     (string)controller.RouteData.Values["controller"],
			     viewName
			)
		{
		}

		public string ViewName
		{
			get { return viewName; }
		}

		public string ViewUrl
		{
			get { return string.Format("/{0}/{1}", controllerName, viewName); }
		}

		public MvpXslTransform XslTransformer
		{
			get
			{
				string viewPath = Path.Combine(appPath, "Views");
				string path = Path.Combine(Path.Combine(viewPath, controllerName), viewName + ".xslt");

				if(!File.Exists(path))
				{
					throw new InvalidOperationException(
						string.Format("Couldn't find the template with name {0}. Verify that the file {1} exists",
						              Path.GetFileNameWithoutExtension(path), path));
				}

				MvpXslTransform transform = new MvpXslTransform();
				XmlReaderSettings settings = new XmlReaderSettings();
				settings.ProhibitDtd = false;
				using(XmlReader reader = XmlReader.Create(path, settings))
				{
					transform.Load(reader);

					reader.Close();
				}

				return transform;
			}
		}
	}
}