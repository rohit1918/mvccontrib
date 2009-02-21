using System;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Behaviors
{
	public class ValidationBehavior : IBehavior
	{
		private const string defaultValidationCssClass = "input-validation-error";
		private readonly Func<ModelStateDictionary> modelStateDictionaryFunc;
		private readonly string validationErrorCssClass;

		public ValidationBehavior(Func<ModelStateDictionary> modelStateDictionaryFunc)
			: this(modelStateDictionaryFunc, defaultValidationCssClass) { }

		public ValidationBehavior(Func<ModelStateDictionary> modelStateDictionaryFunc, string validationErrorCssClass)
		{
			this.modelStateDictionaryFunc = modelStateDictionaryFunc;
			this.validationErrorCssClass = validationErrorCssClass;
		}

		public void Execute(IElement element)
		{
			var name = element.GetAttr(HtmlAttribute.Name);
			if (name == null)
			{
				return;
			}
			ModelState state;
			if (modelStateDictionaryFunc().TryGetValue(name, out state) && state.Errors != null && state.Errors.Count > 0)
			{
				element.Builder.AddCssClass(validationErrorCssClass);
				var valueMethod = element.GetType().GetMethod("Value");
                if (valueMethod != null && state.Value != null)
				{
					valueMethod.Invoke(element, new [] { state.Value.AttemptedValue });
				}
			}
		}
	}
}