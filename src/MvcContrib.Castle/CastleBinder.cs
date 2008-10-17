using System;
using System.Web.Mvc;
using Castle.Components.Binder;
using MvcContrib.Attributes;
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
	public class CastleBindAttribute : AbstractParameterBinderAttribute
	{
		/// <summary>
		/// Properties to exclude from binding
		/// </summary>
		public string Exclude { get; set; }

		/// <summary>
        /// Creates a new CastleBind attribute with the specified parameter prefix. 
        /// </summary>
        /// <param name="prefix">Prefix to use when extracting from the Request.Form.</param>
        public CastleBindAttribute(string prefix) : base(prefix)
        {
        }

        /// <summary>
        /// Creates a new CastleBind attribute. The name of the parameter will be used as the request prefix.
        /// </summary>
		public CastleBindAttribute() : base(null)
        {
        }

		/// <summary>
		/// Binds the model object using a castle IDataBinder
		/// </summary>
		/// <param name="bindingContext">The current binding context</param>
		/// <returns>A ModelBinderResult containing the bound object</returns>
		public override ModelBinderResult BindModel(ModelBindingContext bindingContext) 
		{
			IDataBinder binder = LocateBinder(bindingContext);
			string modelName = Prefix ?? bindingContext.ModelName;
			object instance = binder.BindObject(bindingContext.ModelType, modelName, Exclude, null, new TreeBuilder().BuildSourceNode(bindingContext.HttpContext.Request.Form));
			return new ModelBinderResult(instance);
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