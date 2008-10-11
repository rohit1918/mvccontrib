using System;
using System.Globalization;
using System.Web.Mvc;

namespace MvcContrib.Binders
{
	///<summary>
	/// Binder that creates SubControllers that are needed for an action method
	///</summary>
	public class SubControllerBinder : DefaultModelBinder
	{
        
		protected  object ConvertType(CultureInfo culture, object value, Type destinationType)
		{
			if (typeof (ISubController).IsAssignableFrom(destinationType))
			{
				object instance = CreateSubController(destinationType);
				if (instance == null)
				{
					throw new InvalidOperationException(destinationType + " not created properly.");
				}

				return instance;
			}
		    return null;
			//return base.ConvertType(culture, value, destinationType);
		}

		///<summary>
		/// Creates the subcontroller given its type.  Override this method to wire into an IoC container
		///</summary>
		///<param name="destinationType">The type of subcontroller</param>
		///<returns>an object instance</returns>
		public virtual object CreateSubController(Type destinationType)
		{
			return Activator.CreateInstance(destinationType, true);
		}
	}
}