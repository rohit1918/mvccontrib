using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.MetaData;

namespace MvcContrib
{
	/// <summary>
	/// Custom action invoker for use with the convention controller. 
	/// The ConventionControllerActionInvoker uses the Controller Meta Data in order to locate filters and actions.
	/// </summary>
	public class ConventionControllerActionInvoker : ControllerActionInvoker
	{
		private ControllerMetaData _metaData;
		private IControllerDescriptor _controllerDescriptor;

		/// <summary>
		/// The action currently being executed.
		/// </summary>
		public ActionMetaData SelectedAction { get; private set; }

		/// <summary>
		/// Information about the controller.
		/// </summary>
		public ControllerMetaData MetaData
		{
			get
			{
				if(_metaData == null)
				{
					_metaData = ControllerDescriptor.GetMetaData(ControllerContext.Controller);
				}
				return _metaData;
			}
		}

		/// <summary>
		/// Descriptor used to obtain metadata about the current controller. By default, this will be a CachedControllerDescriptor.
		/// </summary>
		public IControllerDescriptor ControllerDescriptor
		{
			get
			{
				if(_controllerDescriptor == null)
				{
					_controllerDescriptor = new CachedControllerDescriptor();
				}
				return _controllerDescriptor;
			}
			set { _controllerDescriptor = value; }
		}

		protected override MethodInfo FindActionMethod(string actionName) 
		{
			if (string.IsNullOrEmpty(actionName)) 
			{
				throw new ArgumentNullException("actionName");
			}

			var action = FindActionMetaData(actionName);
			
			if(action != null)
			{
				SelectedAction = action;
				return action.MethodInfo;
			}

			return null;
		}

		/// <summary>
		/// Finds the ActionMetaData with the specified action name.
		/// </summary>
		/// <param name="actionName">Name of the action to locate.</param>
		/// <returns>ActionMetaData or null if no action can be found with the specified name.</returns>
		public virtual ActionMetaData FindActionMetaData(string actionName)
		{
			var action = MetaData.GetAction(actionName, ControllerContext);

			if(action == null && MetaData.DefaultAction != null)
			{
				action = MetaData.DefaultAction;
			}

			return action;
		}

		protected override object GetParameterValue(ParameterInfo parameterInfo) 
		{
			//The DefaultModelBinder does not play nicely with value types.
			//For example, if a parameter named "foo" is of type int, and that parameter is not in the RouteData/Request, then the DefaultModelBinder will throw.
			//So, if the DefaultBinder is the default (ie a DefaultModelBinder instance) then we replace it temporarily with a SimpleParameterBinder which will instead return 0.
			//However, if the user has replaced the DefaultBinder with one of their own (eg the CastleSimpleBinder) then we let that do the work.
			if(ModelBinders.DefaultBinder.GetType() == typeof(DefaultModelBinder))
			{
				var binder = new SimpleParameterBinder();
				return binder.GetValue(ControllerContext, parameterInfo.Name, parameterInfo.ParameterType, this.ControllerContext.Controller.ViewData.ModelState);
			}
			return base.GetParameterValue(parameterInfo);
		}
	}
}
