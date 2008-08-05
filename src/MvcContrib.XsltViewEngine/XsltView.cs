using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Xml.Xsl;
using Mvp.Xml.Common.Xsl;
using System.Web;

namespace MvcContrib.XsltViewEngine
{
	public class XsltView : IViewDataContainer
	{
		private readonly string ajaxDeclaration;
		private readonly XsltViewData viewData;
		private readonly XsltTemplate viewTemplate;
		private readonly MvpXslTransform xslTransformer;
		private readonly XmlResponseBuilder construct;
		private ViewContext viewContext;

		public XsltView(XsltTemplate viewTemplate, XsltViewData viewData, string ajaxDeclaration, HttpContextBase httpContext)
		{
			this.viewTemplate = viewTemplate;
			this.viewData = viewData;
			this.ajaxDeclaration = ajaxDeclaration;

			construct = new XmlResponseBuilder(httpContext);

			InitializeConstruct();

			xslTransformer = viewTemplate.XslTransformer;
		}

		public ViewDataDictionary ViewData
		{
			get { return viewContext.ViewData; }
			set { throw new NotSupportedException(); }
		}

		private void InitializeConstruct()
		{
			construct.InitMessageStructure();

			viewData.DataSources.ForEach(dataSource => construct.AppendDataSourceToResponse(dataSource.XmlFragment));

			viewData.Messages.ForEach(message =>{
				if (string.IsNullOrEmpty(message.ControlID))
					construct.AddMessage(message.Content, message.MessageType.ToString().ToUpperInvariant());
				else
					construct.AddMessage(message.Content, message.MessageType.ToString().ToUpperInvariant(), message.ControlID);
			});

			construct.AppendPage(viewTemplate.ViewName, viewTemplate.ViewUrl, viewData.PageVars);
		}

		#region IView Members

		public void RenderView(ViewContext viewContext)
		{
			this.viewContext = viewContext;

			var args = new XsltArgumentList();
			args.AddExtensionObject("urn:HtmlHelper", new HtmlHelper(viewContext, this));
            
			args.AddParam("AjaxProScriptReferences", "", ajaxDeclaration);

			var sb = new StringBuilder();
			using (var sw = new StringWriter(sb))
			{
				xslTransformer.Transform(new XmlInput(construct.Message.CreateNavigator()), args,
				                         new XmlOutput(sw));
			}

			PostTransform(ref sb);

			viewContext.HttpContext.Response.Output.Write(sb.ToString());
		}

		#endregion

		public virtual void PostTransform(ref StringBuilder sb)
		{
		}

        

        
	}
}
