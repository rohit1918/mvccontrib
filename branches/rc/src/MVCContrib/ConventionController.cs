using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.ActionResults;

namespace MvcContrib
{
	/// <summary>
	/// Base controller class that implements additional features over Controller.
	/// </summary>
	public abstract class ConventionController : Controller
	{
		/// <summary>
		/// Creates a new instance of the ConventionController
		/// </summary>
		protected ConventionController() : this(new ConventionControllerActionInvoker())
		{
		}

		/// <summary>
		/// Creates a new instance of the ConventionController using the specified ActionInvoker.
		/// </summary>
		/// <param name="invokerToUse">The action invoker to use.</param>
		protected ConventionController(IActionInvoker invokerToUse)
		{
			ActionInvoker = invokerToUse;
		}

		/// <summary>
		/// Creates a <see cref="XmlResult"/> object for serializing the specified object as XML for writing to the HTTP Response's output stream.
		/// </summary>
		/// <param name="toSerialize">The object to serialize</param>
		/// <returns>An XmlResult object</returns>
		protected virtual XmlResult Xml(object toSerialize)
		{
			return new XmlResult(toSerialize);
		}

		/// <summary>
		/// Redirects to an action on the same or another controller using expression-based syntax
		/// </summary>
		/// <typeparam name="T">The type of the controller on which to call the action</typeparam>
		/// <param name="action">An expression which identifies the action to redirect to on the controller of type <typeparamref name="T"/></param>
		/// <returns>A <see cref="RedirectToRouteResult"/> pointing to the action specified by the <paramref name="action"/> expression</returns>
		protected virtual RedirectToRouteResult RedirectToAction<T>(Expression<Action<T>> action) where T : Controller
		{
			return ((Controller)this).RedirectToAction(action);
		}
	}
}
