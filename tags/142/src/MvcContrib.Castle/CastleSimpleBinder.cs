using System;
using System.Web.Mvc;
using Castle.Components.Binder;
using MvcContrib.MetaData;

namespace MvcContrib.Castle
{
	/// <summary>
	/// Simple IParameterBinder implementation that uses Castle's DefaultConverter.
	/// </summary>
	public class CastleSimpleBinder : IModelBinder
	{
		
		/// <summary>
		/// Looks for a parameter with the specified name in the Request and the RouteData and converts it to the specified type.
		/// </summary>
		/// <param name="modelType">Type to which the value should be converted.</param>
		/// <param name="modelName">Name of the parameter to search for</param>
		/// <param name="controllerContext">Controller Context</param>
		/// <param name="modelState"></param>
		/// <returns>The converted object, or the default value for the type.</returns>
		public object GetValue(ControllerContext controllerContext, string modelName, Type modelType, ModelStateDictionary modelState)
		{
			string value = controllerContext.HttpContext.Request[modelName];

			//Route data should be a higher priority than Request.
			if (controllerContext.RouteData.Values.ContainsKey(modelName)) {
				object routeValue = controllerContext.RouteData.Values[modelName];
				if (routeValue != null) {
					if (modelType.IsAssignableFrom(routeValue.GetType())) {
						return routeValue;
					}
					else {
						value = routeValue.ToString();
					}
				}
			}

			var converter = new DefaultConverter();
			object result = null;

			try {
				bool success;
				result = converter.Convert(modelType, value, out success);
			}
			catch (BindingException) {
			}

			//If the binding failed then value types should be set to their default value. 
			if (result == null && modelType.IsValueType) {
				return Activator.CreateInstance(modelType);
			}

			return result;
		}
	}
}