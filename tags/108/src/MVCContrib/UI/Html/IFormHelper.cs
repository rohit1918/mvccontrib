using System;
using System.Collections;
using MvcContrib.UI.Tags;
using System.Web.Mvc;
using System.Collections.Generic;
using MvcContrib.UI.Tags.Validators;
namespace MvcContrib.UI.Html
{
	public interface IFormHelper
	{
		ViewContext ViewContext { get; set; }
		IDataBinder Binder { get; set; }
		string TextField(string name);
		string TextField(TextBox attributes);
		string TextField(string name, IDictionary attributes);
		string PasswordField(string name);
		string PasswordField(Password attributes);
		string PasswordField(string name, IDictionary attributes);
		string HiddenField(string name);
		string HiddenField(string name, IDictionary attributes);
		string HiddenField(HiddenField options);
		string CheckBoxField(string name);
		string CheckBoxField(string name, IDictionary attributes);
		string CheckBoxField(CheckBoxField options);
		string TextArea(string name);
		string TextArea(string name, IDictionary attributes);
		string TextArea(TextArea options);
		string Submit();
		string Submit(string value);
		string Submit(string value, IDictionary attributes);
		string Submit(SubmitButton options);
		string ImageButton(string src, string alt);
		string ImageButton(string src, string alt, IDictionary attributes);
		string ImageButton(InputImage options);
		string Select(string name, object dataSource, string textField, string valueField);
		string Select(string name, object dataSource, string textField, string valueField, IDictionary attributes);
		string Select(object dataSource, Select options);
		string Select<T>(string name) where T : struct;
		string Select<T>(string name, IDictionary attributes) where T : struct;
		string RadioField(string name, object value);
		string RadioField(string name, object value, IDictionary attributes);
		string RadioField(RadioField options);
		CheckBoxList CheckBoxList(string name, object dataSource, string textField, string valueField);
		CheckBoxList CheckBoxList(string name, object dataSource, string textField, string valueField, IDictionary commonAttributes);
		CheckBoxList CheckBoxList(object dataSource, CheckBoxList options);
		RadioList RadioList(string name, object dataSource, string textField, string valueField);
		RadioList RadioList(string name, object dataSource, string textField, string valueField, IDictionary commonAttributes);
		RadioList RadioList(object dataSource, RadioList options);
		void For<T>(T dataItem, string url, Action<SmartForm<T>> block);
		void For<T>(T dataItem, string url, IDictionary attributes, Action<SmartForm<T>> block);
		void For<T>(string viewDataKey, string url, IDictionary attributes, Action<SmartForm<T>> block);
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
