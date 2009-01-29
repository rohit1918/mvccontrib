using System.Web.Mvc;
using MvcContrib.UI.Tags.Validators;
using System.Collections.Generic;
using System.Collections;
namespace MvcContrib.UI.Html
{
	public interface IValidationHelper
	{
		ViewContext ViewContext { get; set; }
		string ValidatorRegistrationScripts();
		string ValidatorInitializationScripts();
		IDictionary<string, object> FormValidation();
		IDictionary<string, object> FormValidation(string validationGroup);
		string RequiredValidator(string name, string referenceName, string text);
		string RequiredValidator(string name, string referenceName, string text, IDictionary attributes);
		string RequiredValidator(string name, string referenceName, string text, string validationGroup);
		string RequiredValidator(string name, string referenceName, string text, string validationGroup, IDictionary attributes);
		string RequiredValidator(string name, string referenceName, string text, string validationGroup, string initialValue);
		string RequiredValidator(string name, string referenceName, string text, string validationGroup, string initialValue, IDictionary attributes);
		string RegularExpressionValidator(string name, string referenceName, string validationExpression, string text);
		string RegularExpressionValidator(string name, string referenceName, string validationExpression, string text, IDictionary attributes);
		string RegularExpressionValidator(string name, string referenceName, string validationExpression, string text, string validationGroup);
		string RegularExpressionValidator(string name, string referenceName, string validationExpression, string text, string validationGroup, IDictionary attributes);
		string CompareValidator(string name, string referenceName, string compareReferenceName, System.Web.UI.WebControls.ValidationDataType type, System.Web.UI.WebControls.ValidationCompareOperator operatorType, string text);
		string CompareValidator(string name, string referenceName, string compareReferenceName, System.Web.UI.WebControls.ValidationDataType type, System.Web.UI.WebControls.ValidationCompareOperator operatorType, string text, IDictionary attributes);
		string CompareValidator(string name, string referenceName, string compareReferenceName, System.Web.UI.WebControls.ValidationDataType type, System.Web.UI.WebControls.ValidationCompareOperator operatorType, string text, string validationGroup);
		string CompareValidator(string name, string referenceName, string compareReferenceName, System.Web.UI.WebControls.ValidationDataType type, System.Web.UI.WebControls.ValidationCompareOperator operatorType, string text, string validationGroup, IDictionary attributes);
		string RangeValidator(string name, string referenceName, string minimumValue, string maximumValue, System.Web.UI.WebControls.ValidationDataType type, string text);
		string RangeValidator(string name, string referenceName, string minimumValue, string maximumValue, System.Web.UI.WebControls.ValidationDataType type, string text, IDictionary attributes);
		string RangeValidator(string name, string referenceName, string minimumValue, string maximumValue, System.Web.UI.WebControls.ValidationDataType type, string text, string validationGroup);
		string RangeValidator(string name, string referenceName, string minimumValue, string maximumValue, System.Web.UI.WebControls.ValidationDataType type, string text, string validationGroup, IDictionary attributes);
		string CustomValidator(string name, string referenceName, string clientValidationFunction, string text);
		string CustomValidator(string name, string referenceName, string clientValidationFunction, string text, IDictionary attributes);
		string CustomValidator(string name, string referenceName, string clientValidationFunction, string text, string validationGroup);
		string CustomValidator(string name, string referenceName, string clientValidationFunction, string text, string validationGroup, IDictionary attributes);
		string ElementValidation(ICollection<IValidator> validators);
		string ElementValidation(ICollection<IValidator> validators, string referenceName);
	}
}