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
		/// <returns>The converted object, or the default value for the type.</returns>
	    public ModelBinderResult BindModel(ModelBindingContext bindingContext)
		{
			string value = bindingContext.HttpContext.Request[bindingContext.ModelName];

			//Route data should be a higher priority than Request.
			if (bindingContext.RouteData.Values.ContainsKey(bindingContext.ModelName)) {
				object routeValue = bindingContext.RouteData.Values[bindingContext.ModelName];
				if (routeValue != null) {
					if (bindingContext.ModelType.IsAssignableFrom(routeValue.GetType())) {
						return new ModelBinderResult(routeValue);
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
				result = converter.Convert(bindingContext.ModelType, value, out success);
			}
			catch (BindingException) {
			}

			//If the binding failed then value types should be set to their default value. 
			if (result == null && bindingContext.ModelType.IsValueType) {
				return new ModelBinderResult(Activator.CreateInstance(bindingContext.ModelType));
			}

			return new ModelBinderResult(result);
		}
	}
}