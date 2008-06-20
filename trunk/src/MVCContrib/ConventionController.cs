using System;
using System.Web.Mvc;
using MvcContrib.ActionResults;
using MvcContrib.Interfaces;

namespace MvcContrib
{
	/// <summary>
	/// Base controller class that implements additional features over Controller.
	/// Includes support for Rescues, Parameter Binders and Default Actions.
	/// </summary>
	public class ConventionController : Controller, IRescuable
	{
		protected override void Execute(ControllerContext controllerContext)
		{
			ActionInvoker = new ConventionControllerActionInvoker(controllerContext);
			base.Execute(controllerContext);
		}

		/// <summary>
		/// Occurs before a Rescue is invoked.
		/// </summary>
		/// <param name="thrownException">The exception that was thrown</param>
		[NonAction]
		public virtual void OnPreRescue(Exception thrownException)
		{
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
	}
}
