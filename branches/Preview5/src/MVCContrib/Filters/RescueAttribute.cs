/*
using System;
using System.Threading;
using System.Web.Mvc;
using MvcContrib.Services;

namespace MvcContrib.Filters
{
	/// <summary>
	/// Filter attribute for handling errors.
	/// When an error occurs, the RescueAttribute will first search for a view that matches the exception name,
	/// then render the specified view if no matching exception view is found.
	/// The rescue attribute can be limited to certain exception types.
	/// <example>
	/// <![CDATA[
	/// [Rescue("MyErrorView", typeof(InvalidOperationException)]
	/// [Rescue("DatabaseError", typeof(SqlException)]
	/// public class HomeController  {
	/// 
	///     public ActionResult Action()
	///     {
	///        throw new SqlException();
	///       //will look for SqlException.aspx, then DatabaseError.aspx
	///     
	///        throw new InvalidOperationException();
	///       //will look for InvalidOperationException.aspx then MyErrorView.aspx 
	///     }
	/// 
	/// }
	/// 
	/// [Rescue("DefaultRescue")] 
	/// public ActionResult ControllerAction  {
	///     throw new CookieException();
	/// 
	///     //this will look for CookieException.aspx
	///     //then call DefaultRescue.aspx if not found
	/// }
	/// ]]>
	/// 
	/// 
	/// </example>
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public class RescueAttribute : FilterAttribute, IExceptionFilter
	{
		private string _view;
		private readonly Type[] _exceptionsTypes;

		/// <summary>
		/// Creates a new instance of the RescueAttribute class.
		/// </summary>
		/// <param name="view">The name of the view to render when an exception is thrown if no matching view is found.</param>
		public RescueAttribute(string view)
			: this(view, typeof(Exception))
		{
		}

		/// <summary>
		/// Creates a new instance of the RescueAttribute class.
		/// </summary>
		/// <param name="view">The name of the view to render when an exception is thrown if no matching view is found.</param>
		/// <param name="exceptionTypes">The types of exception that this attribute will be restricted in catching.</param>
		public RescueAttribute(string view, params Type[] exceptionTypes)
		{
			if(string.IsNullOrEmpty(view))
			{
				throw new ArgumentException("view is required", "view");
			}

			_view = view;

			if(exceptionTypes != null)
			{
				_exceptionsTypes = exceptionTypes;
			}

			AutoLocate = true;
		}


		/// <summary>
		/// After the action has been executed, the Rescue will be invoked if the filterContext has an Exception.
		/// </summary>
		/// <param name="filterContext">The current filter context.</param>
		public virtual void OnException(ExceptionContext filterContext) 
		{
			Type baseExceptionType = filterContext.Exception.GetBaseException().GetType();

			if (IsThreadAbortException(baseExceptionType))
			{
				filterContext.ExceptionHandled = true;
				return;
			}

			foreach (var expectedExceptionType in ExceptionsTypes) {
				if (expectedExceptionType.IsAssignableFrom(baseExceptionType)) {
					if (AutoLocate) {
						if (ViewExists(baseExceptionType, filterContext)) {
							ViewName = baseExceptionType.Name;
							ActivateSpecificRescue(filterContext.Exception, filterContext);
							filterContext.ExceptionHandled = true;
							return;
						}

						if (ViewExists(expectedExceptionType, filterContext)) {
							ViewName = expectedExceptionType.Name;
							ActivateSpecificRescue(filterContext.Exception, filterContext);
							filterContext.ExceptionHandled = true;
							return;
						}
					}
					ActivateSpecificRescue(filterContext.Exception, filterContext);
					filterContext.ExceptionHandled = true;
					return;
				}
			}
		}

		/// <summary>
		/// Determines if the view exists. Override in an inherited class to implement a custom view finding scheme. 
		/// </summary>
		/// <param name="exceptionType">The type of exception that was thrown.</param>
		/// <param name="controllerContext">The current controllercontext.</param> 
		/// <returns>True if the view is found, otherwise false.</returns>
		protected virtual bool ViewExists(Type exceptionType, ControllerContext controllerContext)
		{
			IViewLocator locator = new WebFormViewLocator();
			try
			{
				locator.GetViewLocation(controllerContext, "Rescues/" + exceptionType.Name);
				return true;
			}
			catch //the GetViewLocation throws an exception if it isn't found.
			{
				return false;
			}
		}

		protected virtual bool IsThreadAbortException(Type baseExceptionType)
		{
			//ThreadAbortException could have occurred due to a direct call to HttpResponse.Redirect.
			//This is perfectly valid, so we don't want to invoke the rescue.
			if(baseExceptionType == typeof(ThreadAbortException))
			{
				return true;
			}
			return false;
		}

		protected virtual void ActivateSpecificRescue(Exception exception, ControllerContext controllerContext)
		{
			IViewEngine viewEngine = null;
			if(controllerContext.Controller is Controller)
				viewEngine = ((Controller)controllerContext.Controller).ViewEngine;
			else if(DependencyResolver.Resolver != null)
				viewEngine = DependencyResolver.Resolver.GetImplementationOf<IViewEngine>();

			if(viewEngine == null)
				viewEngine = new WebFormViewEngine();

			viewEngine.RenderView(CreateViewContext(exception, controllerContext));
		}


		/// <summary>
		/// Creates the ViewContext to be used when rendering the rescue.
		/// </summary>
		/// <param name="exception">The exception which will become the ViewData.</param>
		/// <param name="controllerContext">The current controllercpontext.</param>
		/// <returns>A ViewContext object.</returns>
		protected virtual ViewContext CreateViewContext(Exception exception, ControllerContext controllerContext)
		{
			TempDataDictionary tempData;
			if(controllerContext.Controller is Controller)
				tempData = ((Controller)controllerContext.Controller).TempData;
			else
			{
				tempData = new TempDataDictionary();
				//tempData = new TempDataDictionary(controllerContext.HttpContext);
			}
			return new ViewContext(controllerContext, ViewName, null, new ViewDataDictionary(exception), tempData);
		}

		/// <summary>
		/// The view to render.
		/// </summary>
		public string ViewName
		{
			get { return "Rescues/" + _view; }
			protected set { _view = value; }
		}

		/// <summary>
		/// The exception types used by this rescue.
		/// </summary>
		public Type[] ExceptionsTypes
		{
			get { return _exceptionsTypes; }
		}

		/// <summary>
		/// When false, only the specified rescue will be called.
		/// When true, rescues with a name matching the exception will be called.
		/// </summary>
		public bool AutoLocate { get; set; }
	}
}
*/
