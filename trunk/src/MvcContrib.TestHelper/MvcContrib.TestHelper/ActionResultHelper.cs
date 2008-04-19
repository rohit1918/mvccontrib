using System;
using System.Web.Mvc;

namespace MvcContrib.TestHelper
{
	/// <summary>
	/// Contains extension methods for testing action results.
	/// </summary>
	public static class ActionResultHelper
	{
		/// <summary>
		/// Asserts that the ActionResult is of the specified type.
		/// </summary>
		/// <typeparam name="T">Type of action result to convert to.</typeparam>
		/// <param name="result">Action Result to convert.</param>
		/// <returns></returns>
		public static T AssertResultIs<T>(this ActionResult result) where T : ActionResult
		{
			var converted = result as T;

			if(converted == null)
			{
				throw new ActionResultAssertionException(string.Format("Expected result to be of type {0}. It is actually of type {1}.", typeof(T).Name, result.GetType().Name));
			}

			return converted;
		}

		/// <summary>
		/// Asserts that the action result is a RenderViewResult.
		/// </summary>
		/// <param name="result">The result to convert.</param>
		/// <returns></returns>
		public static RenderViewResult AssertViewRendered(this ActionResult result)
		{
			return result.AssertResultIs<RenderViewResult>();
		}

		/// <summary>
		/// Asserts that the action result is a HttpRedirectResult.
		/// </summary>
		/// <param name="result">The result to convert.</param>
		/// <returns></returns>
		public static HttpRedirectResult AssertHttpRedirect(this ActionResult result)
		{
			return result.AssertResultIs<HttpRedirectResult>();
		}

		/// <summary>
		/// Asserts that the action result is an ActionRedirectResult.
		/// </summary>
		/// <param name="result">The result to convert.</param>
		/// <returns></returns>
		public static ActionRedirectResult AssertActionRedirect(this ActionResult result)
		{
			return result.AssertResultIs<ActionRedirectResult>();
		}

		/// <summary>
		/// Asserts that an ActionRedirectResult is for the specified controller.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="controller">The name of the controller.</param>
		/// <returns></returns>
		public static ActionRedirectResult ToController(this ActionRedirectResult result, string controller)
		{
			return result.WithParameter("controller", controller);
		}

		/// <summary>
		/// Asserts that an ActionRedirectReslt is for the specified controller.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="action">The name of the action.</param>
		/// <returns></returns>
		public static ActionRedirectResult ToAction(this ActionRedirectResult result, string action)
		{
			return result.WithParameter("action", action);

		}

		/// <summary>
		/// Asserts that an ActionRedirectResult contains a specified value in its RouteValueCollection.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="paramName">The name of the parameter to check for.</param>
		/// <param name="value">The expected value.</param>
		/// <returns></returns>
		public static ActionRedirectResult WithParameter(this ActionRedirectResult result, string paramName, object value)
		{
			if(!result.Values.ContainsKey(paramName))
			{
				throw new ActionResultAssertionException(string.Format("Could not find a parameter named '{0}' in the result's Values collection.", paramName));
			}

			var paramValue = result.Values[paramName];

			if(!paramValue.Equals(value))
			{
				throw new ActionResultAssertionException(string.Format("When looking for a parameter named '{0}', expected '{1}' but was '{2}'.", paramName, value, paramValue));
			}

			return result;
		}

		/// <summary>
		/// Asserts that a RenderViewResult is rendering the specified view.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="viewName">The name of the view.</param>
		/// <returns></returns>
		public static RenderViewResult ForView(this RenderViewResult result, string viewName)
		{
			if(result.ViewName != viewName)
			{
				throw new ActionResultAssertionException(string.Format("Expected view name '{0}', actual was '{1}'", viewName, result.ViewName));
			}
			return result;
		}

		/// <summary>
		/// Asserts that a HttpRedirectResult is redirecting to the specified URL.
		/// </summary>
		/// <param name="result">The result to check</param>
		/// <param name="url">The URL that the result should be redirecting to.</param>
		/// <returns></returns>
		public static HttpRedirectResult ToUrl(this HttpRedirectResult result, string url)
		{
			if(result.Url != url)
			{
				throw new ActionResultAssertionException(string.Format("Expected redirect to '{0}', actual was '{1}'", url, result.Url));
			}
			return result;
		}
	}
}