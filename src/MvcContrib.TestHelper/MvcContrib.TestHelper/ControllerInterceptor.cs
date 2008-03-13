using System;
using System.Web.Routing;
using Castle.Core.Interceptor;

namespace MvcContrib.TestHelper
{
	public partial class TestControllerBuilder //remember c++ "friend" classes?
	{
		#region Nested type: ControllerInterceptor

		/// <summary>
		/// Used internally for intercepting controller calls to RenderView and RedirectToAction
		/// </summary>
		private class ControllerInterceptor : IInterceptor
		{
			private readonly TestControllerBuilder _parentHandler;

			/// <summary>
			/// Initializes a new instance of the <see cref="ControllerInterceptor"/> class.
			/// </summary>
			/// <param name="parentHandler">The TestControllerBuilder creating this ControllerInterceptor.</param>
			public ControllerInterceptor(TestControllerBuilder parentHandler)
			{
				_parentHandler = parentHandler;
			}

			#region IInterceptor Members

			/// <summary>
			/// Intercepts the specified invocation. Called by DynamicProxy when any virtual method on the proxied controller is called.
			/// </summary>
			/// <param name="invocation">The invocation.</param>
			public void Intercept(IInvocation invocation)
			{
				if(invocation.Method.Name == "RenderView" && invocation.Arguments.Length == 3)
				{
					var viewName = (string)invocation.Arguments[0];
					var masterName = (string)invocation.Arguments[1];
					object viewData = invocation.Arguments[2];
					_parentHandler.RenderViewData = new RenderViewData
					{
						ViewName = viewName,
						MasterName = masterName,
						ViewData = viewData
					};

					return;
				}
				if(invocation.Method.Name == "RedirectToAction" && invocation.Arguments.Length == 1)
				{
					HandleRedirectToAction(invocation.Arguments[0] as RouteValueDictionary);
					return;
				}
				invocation.Proceed();
			}

			#endregion

			/// <summary>
			/// Handles the RedirectToAction call of a controller
			/// </summary>
			/// <param name="values">The RedirectData or RedirectDataWithController or Anon object with Action/Controller values</param>
			protected void HandleRedirectToAction(RouteValueDictionary values)
			{
				_parentHandler.RedirectToActionData = new RedirectToActionData
				{
					ActionName = Convert.ToString(values.ContainsKey("action") ? values["action"] : ""),
					ControllerName = Convert.ToString(values.ContainsKey("controller") ? values["controller"] : "")
				};
			}
		}

		#endregion
	}
}