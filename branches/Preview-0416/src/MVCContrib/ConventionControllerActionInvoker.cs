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

			//The controller implements IActionFilter. 
			//Make sure its the first one in the list so that OnActionExecuting/OnActionExecuted get called.
			var filters = new List<IActionFilter> { Controller };
			filters.AddRange(actionMetaData.Filters.Cast<IActionFilter>());
			
			try
			{
				ActionExecutedContext postContext = InvokeActionMethodWithFilters(actionMetaData.MethodInfo, values ?? new Dictionary<string, object>(), filters);
				InvokeActionResultWithFilters(postContext.Result ?? new EmptyResult(), filters);	
			}
			catch(Exception exception)
			{
				//Give the rescues an opportunity to handle the error, otherwise rethrow.
				if(!InvokeRescues(actionMetaData, exception))
				{
					throw;
				}
			}
			
			//TODO: Sort filters to match the order ControllerActionInvoker uses.

			return true;
		}

		
		protected override ActionResult InvokeActionMethod(System.Reflection.MethodInfo methodInfo, IDictionary<string, object> parameters)
		{
			//The parameter list should be constructed here to ensure that any pre-action filters have executed before the paramter binders are invoked.
			parameters = GetParameterValues(methodInfo, parameters);
			return base.InvokeActionMethod(methodInfo, parameters);
		}

		/// <summary>
		/// Executes each rescue attribute for the action.  
		/// </summary>
		/// <param name="action">The current action</param>
		/// <param name="exception">The exception that was thrown</param>
		/// <returns>True if a rescue executed. False if no rescues executed.</returns>
		protected virtual bool InvokeRescues(ActionMetaData action, Exception exception)
		{
			foreach(var rescue in action.Rescues)
			{
				if(rescue.PerformRescue(exception, Controller))
				{
					return true;
				}
			}

			return false;
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

	}
}