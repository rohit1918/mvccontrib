using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MvcContrib.Attributes
{
	public class XMLReturnBinder:AbstractReturnBinderAttribute
	{
		public override void Bind(System.Web.Mvc.IController controller, System.Web.Mvc.ControllerContext controllerContext, Type returnType, object returnValue)
		{
			
			if(returnValue!=null)
			{
				XmlSerializer xs = new XmlSerializer(returnType);
				controllerContext.HttpContext.Response.ContentType = "text/xml";
				xs.Serialize(controllerContext.HttpContext.Response.Output,returnValue);
			}
		}
	}
}
