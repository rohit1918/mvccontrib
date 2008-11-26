using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcContrib.Filters
{
	/// <summary>
	/// When a RedirectToRouteResult is returned from an action, anything in the ViewData.ModelState dictionary will be copied into TempData.
	/// When a ViewResult is returned from an action, any ModelState entries that were previously copied to TempData will be copied back to the ModelState dictionary.
	/// </summary>
	public class ModelStateToTempDataAttribute : ActionFilterAttribute
	{
		public const string TempDataKey = "__MvcContrib_ValidationFailures__";

		/// <summary>
		/// When a RedirectToRouteResult is returned from an action, anything in the ViewData.ModelState dictionary will be copied into TempData.
		/// When a ViewResult is returned from an action, any ModelState entries that were previously copied to TempData will be copied back to the ModelState dictionary.
		/// </summary>
		/// <param name="filterContext"></param>
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			var modelState = filterContext.Controller.ViewData.ModelState;

			var controller = filterContext.Controller;

			if(filterContext.Result is ViewResult)
			{
				//If there are failures in tempdata, copy them to the modelstate
				CopyTempDataToModelState(controller.ViewData.ModelState, controller.TempData);
				return;
			}

			//If we're redirecting and there are errors, put them in tempdata instead (so they can later be copied back to modelstate)
			if(filterContext.Result is RedirectToRouteResult && !modelState.IsValid)
			{
				CopyModelStateToTempData(controller.ViewData.ModelState, controller.TempData);
			}
		}

		private void CopyTempDataToModelState(ModelStateDictionary modelState, TempDataDictionary tempData)
		{
			if(!tempData.ContainsKey(TempDataKey)) return;

			var fromTempData = tempData[TempDataKey] as Dictionary<string, ModelStateSerializable>;
			if(fromTempData == null) return;

			foreach(var pair in fromTempData)
			{
				modelState.Add(pair.Key, pair.Value.ToModelState());
			}
		}

		private static void CopyModelStateToTempData(ModelStateDictionary modelState, TempDataDictionary tempData)
		{
			var dict = new Dictionary<string, ModelStateSerializable>();

			foreach(var statePair in modelState)
			{
				dict.Add(statePair.Key, new ModelStateSerializable(statePair.Value));
			}

			tempData[TempDataKey] = dict;
		}

		/// <summary>
		/// Temporarily stores ModelStat entries so they can be serialized to SessionState.
		/// </summary>
		[Serializable]
		public class ModelStateSerializable
		{
			/// <summary>
			/// The errors from the ModelCollection
			/// </summary>
			public IEnumerable<string> Errors { get; private set; }

			/// <summary>
			/// The attempted value
			/// </summary>
			public string AttemptedValue { get; private set; }

			/// <summary>
			/// Creates a new instance of the ModelStateSerializable class.
			/// </summary>
			/// <param name="value">The ModelState entry to wrap</param>
			public ModelStateSerializable(ModelState value)
			{
				AttemptedValue = value.AttemptedValue;
				Errors = value.Errors
					.Select(x => x.ErrorMessage)
					.ToList();
			}

			/// <summary>
			/// Converts back to a ModelState instance.
			/// </summary>
			/// <returns>A reconstructed ModelState instance</returns>
			public ModelState ToModelState()
			{
				var modelState = new ModelState {AttemptedValue = AttemptedValue};
				foreach(var error in Errors)
				{
					modelState.Errors.Add(error);
				}
				return modelState;
			}
		}
	}
}