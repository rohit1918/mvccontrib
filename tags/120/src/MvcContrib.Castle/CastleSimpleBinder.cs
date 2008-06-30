using System;
using System.Web.Mvc;
using Castle.Components.Binder;
using MvcContrib.MetaData;

namespace MvcContrib.Castle
{
	/// <summary>
	/// Simple IParameterBinder implementation that uses Castle's DefaultConverter.
	/// </summary>
	public class CastleSimpleBinder : IParameterBinder
	{
		/// <summary>
		/// Looks for a parameter with the specified name in the Request and the RouteData and converts it to the specified type.
		/// </summary>
		/// <param name="targetType">Type to which the value should be converted.</param>
		/// <param name="paramName">Name of the parameter to search for</param>
		/// <param name="context">Controller Context</param>
		/// <returns>The converted object, or the default value for the type.</returns>
		public object Bind(Type targetType, string paramName, ControllerContext context)
		{
			string value = context.HttpContext.Request[paramName];

			//Route data should be a higher priority than Request.
			if(context.RouteData.Values.ContainsKey(paramName))
			{
				object routeValue = context.RouteData.Values[paramName];
				if(routeValue != null)
				{
					value = routeValue.ToString();
				}
			}

			var converter = new DefaultConverter();
			object result = null;

			try
			{
				bool success;
				result = converter.Convert(targetType, value, out success);
			}
			catch(BindingException)
			{
			}

			//If the binding failed then value types should be set to their default value. 
			if(result == null && targetType.IsValueType)
			{
				return Activator.CreateInstance(targetType);
			}

			return result;
		}
	}
}