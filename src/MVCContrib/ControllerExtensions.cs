using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Microsoft.Web.Mvc.Internal;

namespace MvcContrib
{
	///<summary>
	/// Static class containing extension methods for controllers
	///</summary>
	public static class ControllerExtensions
	{
		/// <summary>
		/// Redirects to an action using expression-based syntax
		/// </summary>
		/// <typeparam name="T">The type of the controller</typeparam>
		/// <param name="controller">The controller on which to call the action</param>
		/// <param name="action">The action to redirect to on the controller</param>
		/// <returns>A <see cref="RedirectToRouteResult"/> pointing to the specified action on the specified controller</returns>
		public static RedirectToRouteResult RedirectToAction<T>(this T controller, Expression<Action<T>> action) where T : Controller
		{
			return new RedirectToRouteResult(ExpressionHelper.GetRouteValuesFromExpression(action));
		}
	}
}