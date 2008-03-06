using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.Attributes;
using MvcContrib.Filters;
using MvcContrib.MetaData;
using MvcContrib.Services;

namespace MvcContrib
{
	public class ConventionController : Controller
	{
		public ActionMetaData SelectedAction { get; protected set; }

		protected override bool InvokeAction(string actionName, System.Web.Routing.RouteValueDictionary values)
		{
			if (string.IsNullOrEmpty(actionName))
			{
				throw new ArgumentException("actionName is required", "actionName");
			}

			IList<ActionMetaData> actions = MetaData.GetActions(actionName);

			ActionMetaData selectedAction;

			if (actions == null || actions.Count == 0)
			{
				//No matching action found - see if there is a "catch all" action.
				if (MetaData.DefaultAction != null)
				{
					selectedAction = MetaData.DefaultAction;
				}
				else
				{
					return false;
				}
			}
			else if (actions.Count > 1)
			{
				throw new InvalidOperationException(string.Format("More than one action with name '{0}' found", actionName));
			}
			else
			{
				selectedAction = actions[0];
			}

			SelectedAction = selectedAction;

			FilterContext filterContext = new FilterContext(ControllerContext, selectedAction.MethodInfo);
			FilterExecutingContext executingContext = new FilterExecutingContext(filterContext);

			OnActionExecuting(executingContext);

			if (!executingContext.Cancel)
			{
				try
				{
					if (!ExecutePreActionFilters(selectedAction, executingContext))
					{
						return false;
					}

					InvokeActionMethod(selectedAction);

					//TODO: ExecutePostActionFilters
				}
				catch (Exception exc)
				{
					FilterExecutedContext executedContext = new FilterExecutedContext(filterContext, exc);
					OnActionExecuted(executedContext);

					if(!executedContext.ExceptionHandled)
					{
						throw;
					}
				}
				finally
				{
					OnActionExecuted(new FilterExecutedContext(filterContext, null));
				}
			}

			return true;
		}

		protected override void OnActionExecuted(FilterExecutedContext filterContext)
		{
			if(filterContext.Exception != null)
			{
				bool handled = OnError(SelectedAction, filterContext.Exception);
				filterContext.ExceptionHandled = handled;
			}

			base.OnActionExecuted(filterContext);
		}

		protected virtual bool ExecutePreActionFilters(ActionMetaData action, FilterExecutingContext context)
		{
			foreach(var filter in action.Filters)
			{
				filter.OnActionExecuting(context);
				if(context.Cancel)
				{
					return false;
				}
			}

			return true;
		}

		private bool _isRedirected = false;
		protected override void RedirectToAction(System.Web.Routing.RouteValueDictionary values)
		{
			_isRedirected = true;
			base.RedirectToAction(values);
		}

		protected virtual bool OnError(ActionMetaData action, Exception exception)
		{
			Type baseExceptionType = exception.GetBaseException().GetType();

			if (baseExceptionType == typeof(System.Threading.ThreadAbortException) && _isRedirected)
			{
				return true;
			}

			foreach (RescueAttribute rescue in action.Rescues)
			{
				foreach (Type exceptionType in rescue.ExceptionTypes)
				{
					if (exceptionType.IsAssignableFrom(baseExceptionType))
					{
						OnPreRescue(exception);

						if (!string.IsNullOrEmpty(rescue.View))
						{
							string rescueView = string.Concat("Rescues/", rescue.View);

							RenderView(rescueView, exception);

							return true;
						}
					}
				}
			}

			return false;
		}

		protected virtual void OnPreRescue(Exception thrownException)
		{

		}

		protected virtual void InvokeActionMethod(ActionMetaData action)
		{
			object[] actionParameters = new object[action.Parameters.Count];

			int index = 0;
			foreach (ActionParameterMetaData actionParameter in action.Parameters)
			{
				actionParameters[index++] = actionParameter.Bind(ControllerContext);
			}

			object returnValue = action.InvokeMethod(this, actionParameters);
			if (action.ReturnBinderDescriptor != null)
			{
				IReturnBinder binder = action.ReturnBinderDescriptor.ReturnTypeBinder;

				// Runs return binder and keep going
				binder.Bind(this, ControllerContext, action.ReturnBinderDescriptor.ReturnType, returnValue);
			}
		}

		private ControllerMetaData _metaData;

		public ControllerMetaData MetaData
		{
			get
			{
				if (_metaData == null)
				{
					_metaData = ControllerDescriptor.GetMetaData(this);
				}
				return _metaData;
			}
		}

		private IControllerDescriptor _controllerDescriptor;

		public IControllerDescriptor ControllerDescriptor
		{
			get
			{
				if (_controllerDescriptor == null)
				{
					_controllerDescriptor = new CachedControllerDescriptor();
				}
				return _controllerDescriptor;
			}
			set { _controllerDescriptor = value; }
		}
	}
}
