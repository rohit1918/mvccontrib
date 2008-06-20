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
	}
}
