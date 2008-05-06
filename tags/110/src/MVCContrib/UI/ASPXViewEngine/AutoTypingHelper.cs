using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;

namespace MvcContrib.UI.ASPXViewEngine
{
	internal static class AutoTypingHelper
	{
		public static T PerformLooseTypecast<T>(object fromObject)
		{
			if ((fromObject == null) || (typeof(T).IsAssignableFrom(fromObject.GetType())))
				// The incoming object is already of the right type
				return (T)fromObject;
			else
			{
				// Convert the incoming object to a dictionary, if it isn't one already
				IDictionary suppliedProps = fromObject as IDictionary;
				if (suppliedProps == null)
					suppliedProps = fromObject.GetType().GetProperties()
									.ToDictionary(pi => pi.Name, pi => pi.GetValue(fromObject, null));

				// Construct a T object, taking values from suppliedProps where available
				T result = Activator.CreateInstance<T>();
				foreach (PropertyInfo allowedProp in typeof(T).GetProperties())
					if (suppliedProps.Contains(allowedProp.Name))
						allowedProp.SetValue(result, suppliedProps[allowedProp.Name], null);

				return result;
			}
		}
	}
}
