using System;
using System.Globalization;
using System.Web.Mvc;

namespace MvcContrib.MetaData
{
	//TODO: Preview 5 Investigate whether this is still necessary or whether DefaultModelBinder can take care of it.
	[Serializable]
	public class SimpleParameterBinder : IModelBinder
	{
		public object GetValue(ControllerContext controllerContext, string modelName, Type modelType, ModelStateDictionary modelState)
		{
			string value = controllerContext.HttpContext.Request[modelName];

			if (controllerContext.RouteData.Values.ContainsKey(modelName)
				&& controllerContext.RouteData.Values[modelName] != null) {
				object routeValue = controllerContext.RouteData.Values[modelName];
				if (modelType.IsAssignableFrom(routeValue.GetType())) {
					return routeValue;
				}
				else {
					value = routeValue.ToString();
				}
			}

			var convertible = new DefaultConvertible(value);
			return convertible.ToType(modelType, CultureInfo.CurrentCulture);
		}

        #region IModelBinder Members

        public ModelBinderResult BindModel(ModelBindingContext bindingContext)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
