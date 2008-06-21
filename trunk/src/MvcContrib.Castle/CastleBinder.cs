using System;
using System.Web.Mvc;
using Castle.Components.Binder;
using MvcContrib.MetaData;

namespace MvcContrib.Castle
{
	/// <summary>
	/// Parameter binder that uses the Castle DataBinder to bind action parameters.
	/// Example:
	/// <![CDATA[
	/// public ActionResult Save([CastleBind] Customer customer) {
	///  //...
	/// }
	/// ]]>
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false), Serializable]
	public class CastleBindAttribute : Attribute, IParameterBinder
	{
		/// <summary>
		/// Properties to exclude from binding
		/// </summary>
		public string Exclude { get; set; }

		/// <summary>
		/// Prefix. If null, will use the paramter name as the prefix.
		/// </summary>
		protected string Prefix { get; private set; }

		/// <summary>
        /// Creates a new CastleBind attribute with the specified parameter prefix. 
        /// </summary>
        /// <param name="prefix">Prefix to use when extracting from the Request.Form.</param>
        public CastleBindAttribute(string prefix)
        {
            Prefix = prefix;
        }

        /// <summary>
        /// Creates a new CastleBind attribute. The name of the parameter will be used as the request prefix.
        /// </summary>
		public CastleBindAttribute()
        {
        }

        /// <summary>
        /// Performs the binding.
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="paramName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual object Bind(Type targetType, string paramName, ControllerContext context)
        {
			IDataBinder binder = LocateBinder(context);
            object instance = binder.BindObject(targetType, Prefix ?? paramName, Exclude, null, new TreeBuilder().BuildSourceNode(context.HttpContext.Request.Form));
            return instance;
        }

		/// <summary>
		/// Finds the binder to use. If the controller implements ICastleBindingContainer then its binder is used. Otherwise, a new DataBinder is created.
		/// </summary>
		/// <returns></returns>
		protected virtual IDataBinder LocateBinder(ControllerContext context)
		{
			var bindingContainer = context.Controller as ICastleBindingContainer;

			if(bindingContainer != null)
			{
				if(bindingContainer.Binder == null)
				{
					bindingContainer.Binder = CreateBinder();
				}
				return bindingContainer.Binder;
			}

			return CreateBinder();
		}

        /// <summary>
        /// Creates the binder to use.
        /// </summary>
        /// <returns>IDataBinder</returns>
        protected virtual IDataBinder CreateBinder()
        {
            return new DataBinder();
        }
	}
}