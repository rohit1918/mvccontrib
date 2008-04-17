using System;
using System.Web.Mvc;
using MvcContrib.MetaData;

namespace MvcContrib
{
	/// <summary>
	/// Base controller class that implements additional features over Controller.
	/// Includes support for Rescues, Parameter Binders and Default Actions.
	/// </summary>
	public class ConventionController : Controller
	{
		private ControllerMetaData _metaData;
		private IControllerDescriptor _controllerDescriptor;

		/// <summary>
		/// The action currently being executed.
		/// </summary>
		public ActionMetaData SelectedAction { get; protected internal set; }

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
		/// Information about the controller.
		/// </summary>
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

		/// <summary>
		/// Descriptor used to obtain metadata about the current controller. By default, this will be a CachedControllerDescriptor.
		/// </summary>
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
