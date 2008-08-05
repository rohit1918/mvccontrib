using System;
using System.Globalization;
using System.Web.Mvc;

namespace MvcContrib.MetaData
{
	[Serializable]
	public class SimpleParameterBinder : IParameterBinder
	{
		public object Bind(Type targetType, string paramName, ControllerContext context)
		{
			string sValue = context.HttpContext.Request[paramName];

			if( context.RouteData.Values.ContainsKey(paramName) 
				&& context.RouteData.Values[paramName] != null )
			{
				sValue = context.RouteData.Values[paramName].ToString();
			}

			var convertible = new DefaultConvertible(sValue);
			return convertible.ToType(targetType, CultureInfo.CurrentCulture);
		}
	}
}
