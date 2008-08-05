using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.ActionResults;

namespace MvcContrib
{
	/// <summary>
	/// Base controller class that implements additional features over Controller.
	/// Includes support for Rescues, Parameter Binders and Default Actions.
	/// </summary>
	public class ConventionController : Controller
	{
		protected override void Execute(ControllerContext controllerContext)
		{
			ActionInvoker = CreateActionInvoker(controllerContext);
			base.Execute(controllerContext);
		}

		protected virtual ControllerActionInvoker CreateActionInvoker(ControllerContext controllerContext)
		{
			return new ConventionControllerActionInvoker(controllerContext);
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
		/// Creates a <see cref="BinaryResult"/> object for writing the specified binary content to the HTTP Response's output stream.
		/// </summary>
		/// <param name="content">The binary content.</param>
		/// <param name="contentType">MIME type of the content</param>
		/// <param name="asAttachment">Whether to specify the stream as an attachments. (When TRUE most bowsers will prompt the user to save the response instead of rendering in the browser.)</param>
		/// <param name="filename">The filename to specify in the response.  (Most browsers will default to this value when saving as an attachment.) </param>
		/// <returns>A BinaryResult object</returns>
		protected virtual BinaryResult Binary(byte[] content, string contentType, bool asAttachment, string filename)
		{
			return new BinaryResult(content, contentType, asAttachment, filename);
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
