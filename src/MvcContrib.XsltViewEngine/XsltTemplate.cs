using System;
using System.IO;
using System.Xml;
using MvcContrib.ViewFactories;
using Mvp.Xml.Common.Xsl;

namespace MvcContrib.XsltViewEngine
{
	public class XsltTemplate
	{
		private readonly MvpXslTransform _transform;

		public XsltTemplate(IViewSourceLoader viewSourceLoader,string viewPath)
		{

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

			var settings = new XmlReaderSettings {ProhibitDtd = false};

			using(var viewSourceStream = viewSource.OpenViewStream())
			{
				using (var xmlReader = XmlReader.Create(viewSourceStream, settings))
				{
					_transform.Load(xmlReader);
				}
			}
		}

		public MvpXslTransform XslTransformer
		{
			get { return _transform; }
		}
	}
}
