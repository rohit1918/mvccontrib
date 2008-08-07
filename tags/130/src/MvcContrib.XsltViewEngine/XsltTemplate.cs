using System;
using System.IO;
using System.Xml;
using MvcContrib.ViewFactories;
using Mvp.Xml.Common.Xsl;

namespace MvcContrib.XsltViewEngine
{
	public class XsltTemplate
	{
		private readonly string _viewName;
		private readonly string _viewUrl;
		private readonly MvpXslTransform _transform;

		public XsltTemplate(IViewSourceLoader viewSourceLoader, string controllerName, string viewName)
		{
			_viewName = viewName;
			_viewUrl = string.Format("/{0}/{1}", controllerName, _viewName);

			string viewPath = string.Concat(controllerName, "/", viewName);
			if( !Path.HasExtension(viewPath) )
			{
				viewPath += ".xslt";
			}

			if( viewSourceLoader == null )
			{
				throw new ArgumentNullException("viewSourceLoader");
			}

			if( !viewSourceLoader.HasView(viewPath) )
			{
				throw new InvalidOperationException(string.Format("Couldn't find the template with name {0}.", viewPath));
			}

			IViewSource viewSource = viewSourceLoader.GetViewSource(viewPath);

			_transform = new MvpXslTransform();

			var settings = new XmlReaderSettings();
			settings.ProhibitDtd = false;

			using(Stream viewSourceStream = viewSource.OpenViewStream())
			{
				using (XmlReader reader = XmlReader.Create(viewSourceStream, settings))
				{
					_transform.Load(reader);
				}
			}
		}

		public string ViewName
		{
			get { return _viewName; }
		}

		public string ViewUrl
		{
			get { return _viewUrl; }
		}

		public MvpXslTransform XslTransformer
		{
			get { return _transform; }
		}
	}
}
