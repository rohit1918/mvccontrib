using System;
using System.Collections.Generic;
using System.Linq;
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
		/// <summary>
		/// The current ConventionController being executed.
		/// </summary>
		public ConventionController Controller { get; protected set; }


		/// <summary>
		/// Creates a new instance of the ConventionControllerActionInvoker class.
		/// </summary>
		/// <param name="controllerContext">The controller context for use with the current request.</param>
		public ConventionControllerActionInvoker(ControllerContext controllerContext) : base(controllerContext)
		{
			Controller = controllerContext.Controller as ConventionController;

			if(Controller == null)
			{
				throw new Exception("The ConventionControllerActionInvoker can only be used with controllers that inherit from ConventionController.");
			}
		}

		/// <summary>
		/// Invokes the action with the specified action name. 
		/// </summary>
		/// <param name="actionName">The name of the action to invoke.</param>
		/// <param name="values">Custom parameters to pass to the action. Note: ConventionControllerInvoker ignores custom parameters.</param>
		/// <returns>A boolean that represents whether the action was successfully invoked or not.</returns>
		public override bool InvokeAction(string actionName, System.Collections.Generic.IDictionary<string, object> values)
		{
			if(string.IsNullOrEmpty(actionName))
			{
				throw new ArgumentNullException("actionName", "actionName is required.");
			}

			var actionMetaData = FindActionMetaData(actionName);

			if (actionMetaData == null)
			{
				return false;
			}

			Controller.SelectedAction = actionMetaData;

			IDictionary<string, object> parameters = GetParameterValues(actionMetaData.MethodInfo, values);
			
			//The controller implements IActionFilter. 
			//Make sure its the first one in the list so that OnActionExecuting/OnActionExecuted get called.
			var filters = new List<IActionFilter> { Controller };
			filters.AddRange(actionMetaData.Filters.Cast<IActionFilter>());
			

			ActionExecutedContext postContext = InvokeActionMethodWithFilters(actionMetaData.MethodInfo, parameters, filters);
			InvokeActionResultWithFilters(postContext.Result ?? new EmptyResult(), filters);

			//TODO: Sort filters to match the order ControllerActionInvoker uses.
			//TODO: Rescues

			return true;
		}



		//The base implementation of GetParameterValues inspects the MethodInfo directly by looping over all of its parameters. 
		//We don't need to do this as the ControllerDescriptor has already gathered information about the parameters when it was instantiated.
		protected override IDictionary<string, object> GetParameterValues(System.Reflection.MethodInfo methodInfo, IDictionary<string, object> values)
		{
			var actionMetaData = Controller.SelectedAction;
			var parameters = new Dictionary<string, object>();

			foreach(var parameter in actionMetaData.Parameters)
			{
				parameters.Add(parameter.ParameterInfo.Name, parameter.Bind(ControllerContext));
			}

			return parameters;
		}

		/// <summary>
		/// Finds the ActionMetaData with the specified action name.
		/// </summary>
		/// <param name="actionName">Name of the action to locate.</param>
		/// <returns>ActionMetaData or null if no action can be found with the specified name.</returns>
		public virtual ActionMetaData FindActionMetaData(string actionName)
		{
			var actions = Controller.MetaData.GetActions(actionName);

			if (actions == null || actions.Count == 0)
			{
				//No matching action found - see if there is a "catch all" action.
				if (Controller.MetaData.DefaultAction != null)
				{
					return Controller.MetaData.DefaultAction;
				}
				else
				{
					return null;
				}
			}
			
			if (actions.Count > 1)
			{
				throw new InvalidOperationException(string.Format("More than one action with name '{0}' found", actionName));
			}
			
			return actions[0];
		}

		protected override System.Reflection.MethodInfo FindActionMethod(string actionName, System.Collections.Generic.IDictionary<string, object> values)
		{
			var meta = FindActionMetaData(actionName);
			return meta == null ? null : meta.MethodInfo;
		}


	}
}