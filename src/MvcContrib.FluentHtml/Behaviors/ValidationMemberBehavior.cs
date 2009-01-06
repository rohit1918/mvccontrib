using System.Web.Mvc;
using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.FluentHtml.Behaviors
{
    public class ValidationMemberBehavior : IMemberBehavior
    {
        private const string defaultValidationCssClass = "input-validation-error";
        private readonly ModelStateDictionary modelStateDictionary;
        private readonly string validationErrorCssClass;

        public ValidationMemberBehavior(ModelStateDictionary modelStateDictionary)
            : this(modelStateDictionary, defaultValidationCssClass) { }

        public ValidationMemberBehavior(ModelStateDictionary modelStateDictionary, string validationErrorCssClass)
        {
            this.modelStateDictionary = modelStateDictionary;
            this.validationErrorCssClass = validationErrorCssClass;
        }

        public void Execute(IMemberElement element)
        {
            var name = element.GetAttr("name");
            if (name == null)
            {
                return;
            }
            ModelState state;
            if (modelStateDictionary.TryGetValue(name, out state) && state.Errors != null && state.Errors.Count > 0)
            {
                element.SetAttr("class", validationErrorCssClass);
            }
        }
    }
}