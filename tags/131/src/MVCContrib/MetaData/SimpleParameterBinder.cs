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
			string value = context.HttpContext.Request[paramName];

			if( context.RouteData.Values.ContainsKey(paramName) 
				&& context.RouteData.Values[paramName] != null )
			{
				object routeValue = context.RouteData.Values[paramName];
				if(targetType.IsAssignableFrom(routeValue.GetType()))
				{
					return routeValue;
				}
				else
				{
					value = routeValue.ToString();
				}
			}

			var convertible = new DefaultConvertible(value);
			return convertible.ToType(targetType, CultureInfo.CurrentCulture);
		}
	}
}
