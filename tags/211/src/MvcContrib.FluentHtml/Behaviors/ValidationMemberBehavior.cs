using System;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Behaviors
{
	public class ValidationMemberBehavior : IMemberBehavior
	{
		private const string defaultValidationCssClass = "input-validation-error";
		private readonly Func<ModelStateDictionary> modelStateDictionaryFunc;
		private readonly string validationErrorCssClass;

		public ValidationMemberBehavior(Func<ModelStateDictionary> modelStateDictionaryFunc)
			: this(modelStateDictionaryFunc, defaultValidationCssClass) { }

		public ValidationMemberBehavior(Func<ModelStateDictionary> modelStateDictionaryFunc, string validationErrorCssClass)
		{
			this.modelStateDictionaryFunc = modelStateDictionaryFunc;
			this.validationErrorCssClass = validationErrorCssClass;
		}

		public void Execute(IMemberElement element)
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
				if (valueMethod != null)
				{
					valueMethod.Invoke(element, new [] { state.AttemptedValue });
				}
			}
		}
	}
}