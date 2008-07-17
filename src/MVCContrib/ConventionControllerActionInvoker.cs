using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.Interfaces;
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


		/// <summary>
		/// Creates a new instance of the ConventionControllerActionInvoker class.
		/// </summary>
		/// <param name="controllerContext">The controller context for use with the current request.</param>
		public ConventionControllerActionInvoker(ControllerContext controllerContext) : base(controllerContext)
		{
		}

		/// <summary>
		/// Invokes the action with the specified action name. 
		/// </summary>
		/// <param name="actionName">The name of the action to invoke.</param>
		/// <param name="values">Custom parameters to pass to the action. Note: ConventionControllerInvoker ignores custom parameters.</param>
		/// <returns>A boolean that represents whether the action was successfully invoked or not.</returns>
		public override bool InvokeAction(string actionName, IDictionary<string, object> values)
		{
			if(string.IsNullOrEmpty(actionName))
			{
				throw new ArgumentNullException("actionName", "actionName is required.");
			}

			var actionMetaData = FindActionMetaData(actionName);

			if(actionMetaData == null)
			{
				return false;
			}

			SelectedAction = actionMetaData;
            
			//var filters = GetAllActionFilters(actionMetaData.MethodInfo);
            
            FilterInfo filters = base.GetFiltersForActionMethod(actionMetaData.MethodInfo);
            

			try
			{
				ActionExecutedContext postContext = InvokeActionMethodWithFilters(actionMetaData.MethodInfo,
				                                                                  values ?? new Dictionary<string, object>(),
				                                                                  filters.ActionFilters);
				InvokeActionResultWithFilters(postContext.Result ?? new EmptyResult(), filters.ResultFilters);
			}
			catch(Exception exception)
			{
				//Give the rescues an opportunity to handle the error, otherwise rethrow.
				if(!InvokeRescues(actionMetaData, exception))
				{
					throw;
				}
			}

			return true;
		}

		protected override ActionResult InvokeActionMethod(MethodInfo methodInfo, IDictionary<string, object> parameters)
		{
			//The parameter list should be constructed here to ensure that any pre-action filters have executed before the parameter binders are invoked.
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
			if((ControllerContext.Controller is IRescue) &&
			   ((IRescue)ControllerContext.Controller).PerformRescue(exception, ControllerContext))
				return true;

			foreach(var rescue in action.Rescues)
			{
				if(rescue.PerformRescue(exception, ControllerContext))
				{
					return true;
				}
			}
			return false;
		}

		//The base implementation of GetParameterValues inspects the MethodInfo directly by looping over all of its parameters. 
		//We don't need to do this as the ControllerDescriptor has already gathered information about the parameters when it was instantiated.
		protected override IDictionary<string, object> GetParameterValues(MethodInfo methodInfo,
		                                                                  IDictionary<string, object> values)
		{
			var actionMetaData = SelectedAction;
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
			var actions = MetaData.GetActions(actionName);

			if(actions == null || actions.Count == 0)
			{
				//No matching action found - see if there is a "catch all" action.
				if(MetaData.DefaultAction != null)
				{
					return MetaData.DefaultAction;
				}
				else
				{
					return null;
				}
			}

			if(actions.Count > 1)
			{
				throw new InvalidOperationException(string.Format("More than one action with name '{0}' found", actionName));
			}

			return actions[0];
		}
	}
}