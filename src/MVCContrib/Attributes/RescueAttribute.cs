using System;
using System.Collections.Generic;
using System.Web.Mvc;
namespace MvcContrib.Attributes
{
	/// <summary>
	/// Filter attribute for handling errors.
	/// When an error occurs, the RescueAttribute will render the specified view.
	/// The rescue attribute can be limited to certain exception types.
	/// <example>
	/// <![CDATA[
	/// [Rescue("MyErrorView", typeof(InvalidOperationException)]
	/// [Rescue("DatabaseError", typeof(SqlException)]
	/// public class HomeController  {
	/// 
	/// }
	/// ]]>
	/// </example>
	/// </summary>
	public class RescueAttribute : ActionFilterAttribute
	{
		private readonly string _view;
		private readonly Type[] _exceptionsTypes;

		/// <summary>
		/// Creates a new instance of the RescueAttribute class.
		/// </summary>
		/// <param name="view">The name of the view to render when an exception is thrown.</param>
		public RescueAttribute(string view)
			: this(view, typeof(Exception))
		{
		}

		/// <summary>
		/// Creates a new instance of the RescueAttribute class.
		/// </summary>
		/// <param name="view">The name of the view to render when an exception is thrown.</param>
		/// <param name="exceptionTypes">The types of exception that the attribute should apply to.</param>
		public RescueAttribute(string view, params Type[] exceptionTypes)
		{
			if (string.IsNullOrEmpty(view))
			{
				throw new ArgumentException("view is required", "view");
			}

			_view = view;

			if (exceptionTypes != null)
			{
				_exceptionsTypes = exceptionTypes;
			}
		}

		/// <summary>
		/// After the action has been executed, the Rescue will be invoked if the filterContext has an Exception.
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.Exception == null || filterContext.ExceptionHandled)
			{
				return;
			}

			var exception = filterContext.Exception;
			var controller = filterContext.Controller as Controller;

			if(controller == null)
			{
				throw new NotSupportedException("The RescueAttribute can only be used with controllers that inherit from System.Web.Mvc.Controller.");
			}

			Type baseExceptionType = exception.GetBaseException().GetType();

			//ThreadAbortException could have occurred due to a direct call to HttpResponse.Redirect.
			//This is perfectly valid, so we don't want to invoke the rescue.
			if (baseExceptionType == typeof(System.Threading.ThreadAbortException))
			{
				return;
			}

			//If we're using a ConventionController, ensure that the OnPreRescue method is called.
			var conventionController = filterContext.Controller as ConventionController;

			foreach (Type exceptionType in _exceptionsTypes)
			{
				if (exceptionType.IsAssignableFrom(baseExceptionType))
				{
					//If the current controller is a ConventionController, additional parsing of the exception can occur in OnPreRescue.
					if(conventionController != null)
					{
						conventionController.OnPreRescue(exception);
					}

					filterContext.ExceptionHandled = true;
					controller.ViewEngine.RenderView(CreateViewContext(exception, controller));
					return;
				}
			}
		}

		/// <summary>
		/// Creates the ViewContext to be used when rendering the rescue.
		/// </summary>
		/// <param name="exception">The exception which will become the ViewData.</param>
		/// <param name="controller">The current controller.</param>
		/// <returns>A ViewContext object.</returns>
		protected virtual ViewContext CreateViewContext(Exception exception, Controller controller)
		{
			return new ViewContext(controller.ControllerContext, ViewName, null, exception, controller.TempData);
		}

		/// <summary>
		/// The view to render.
		/// </summary>
		public string ViewName
		{
			get { return "Rescues/" + _view; }
		}
	}
}
