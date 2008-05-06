using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Handlers;
using System.Web.Mvc;
using MvcContrib.Services;
using System.Collections.Generic;
using System.Collections;
using MvcContrib.UI.Tags.Validators;

namespace MvcContrib.UI.Html
{
	public class ValidationHelper : BaseHelper, IValidationHelper
	{
		public const string CACHE_KEY = "__MvcContribValidationHelper__";
		private const string VALIDATOR_CACHE_KEY = "__MvcContrib_Validators__";
		private const string VALIDATOR_REGISTERED_CACHE_KEY = "__MvcContrib_Validations_Registered__";
		private const string VALIDATOR_INITIALIZED_CACHE_KEY = "__MvcContrib_Validators_Initialized__";
		private static string _webValidationUrl;
		private static object _webValidationUrlLock = new object();

		public static IValidationHelper GetInstance(ViewContext context)
		{
			if (context.HttpContext.Items.Contains(CACHE_KEY))
				return (IValidationHelper)context.HttpContext.Items[CACHE_KEY];

			IValidationHelper helper;

			if (DependencyResolver.Resolver != null)
				helper = DependencyResolver.Resolver.GetImplementationOf<IValidationHelper>();
			else
				helper = new ValidationHelper();

			helper.ViewContext = context;
			context.HttpContext.Items.Add(CACHE_KEY, helper);
			return helper;
		}

		public string ValidatorRegistrationScripts()
		{
			if (this.ViewContext.HttpContext.Items[VALIDATOR_REGISTERED_CACHE_KEY] != null)
			{
				throw new InvalidOperationException("You cannot register the validation scripts more than 1 time.");
			}

			StringBuilder output = new StringBuilder();
			if (string.IsNullOrEmpty(_webValidationUrl))
			{
				lock (_webValidationUrlLock)
				{
					if (string.IsNullOrEmpty(_webValidationUrl))
					{
						Type loaderType = typeof(AssemblyResourceLoader);
						Assembly systemWebAssembly = Assembly.GetAssembly(loaderType);
						MethodInfo webResourceUrlMethod = loaderType.GetMethod("GetWebResourceUrlInternal", BindingFlags.NonPublic | BindingFlags.Static);
						_webValidationUrl = this.ViewContext.HttpContext.Request.ApplicationPath + (string)webResourceUrlMethod.Invoke(null, new object[] { systemWebAssembly, "WebUIValidation.js", false });
					}
				}
			}

			output.Append("<script src=\"");
			output.Append(_webValidationUrl);
			output.Append("\" type=\"text/javascript\"></script>");
			output.AppendLine();

			output.AppendLine("<script type=\"text/javascript\">");
			output.AppendLine("//<![CDATA[");
			output.AppendLine("function WebForm_OnSubmit() {");
			output.AppendLine("if (typeof(ValidatorOnSubmit) == \"function\" && ValidatorOnSubmit('') == false) return false;");
			output.AppendLine("return true;");
			output.AppendLine("}");
			output.AppendLine("function WebForm_OnSubmitGroup(group) {");
			output.AppendLine("if (typeof(ValidatorOnSubmit) == \"function\" && ValidatorOnSubmit(group) == false) return false;");
			output.AppendLine("return true;");
			output.AppendLine("}");
			output.AppendLine("//]]>");
			output.AppendLine("</script>");

			this.ViewContext.HttpContext.Items[VALIDATOR_REGISTERED_CACHE_KEY] = true;

			return output.ToString();
		}

		public string ValidatorInitializationScripts()
		{
			if (this.ViewContext.HttpContext.Items[VALIDATOR_REGISTERED_CACHE_KEY] == null)
			{
				throw new InvalidOperationException("You must register the validation scripts before initializing.");
			}

			if (this.ViewContext.HttpContext.Items[VALIDATOR_INITIALIZED_CACHE_KEY] != null)
			{
				throw new InvalidOperationException("You cannot render the validation scripts more than 1 time.");
			}

			StringBuilder outputValidatorArray = new StringBuilder();
			StringBuilder outputValidators = new StringBuilder();
			bool firstValidator = true;

			outputValidatorArray.AppendLine("<script type=\"text/javascript\">");
			outputValidatorArray.AppendLine("//<![CDATA[");
			outputValidatorArray.Append("var Page_Validators = new Array(");

			outputValidators.AppendLine("<script type=\"text/javascript\">");
			outputValidators.AppendLine("//<![CDATA[");

			List<IValidator> validators = this.ViewContext.HttpContext.Items[VALIDATOR_CACHE_KEY] as List<IValidator>;

			if (validators != null)
			{
				foreach (IValidator validator in validators)
				{
					if (!firstValidator)
					{
						outputValidatorArray.Append(", ");
					}

					outputValidatorArray.Append("document.getElementById(\"");
					outputValidatorArray.Append(validator.Id.Replace('.', '-'));
					outputValidatorArray.Append("\")");
					firstValidator = false;

					validator.RenderClientHookup(outputValidators);
				}
			}

			outputValidators.AppendLine("//]]>");
			outputValidators.AppendLine("<!--");
			outputValidators.AppendLine("var Page_ValidationActive = false;");
			outputValidators.AppendLine("if (typeof(ValidatorOnLoad) == \"function\") {");
			outputValidators.AppendLine("\tValidatorOnLoad();");
			outputValidators.AppendLine("}");
			outputValidators.AppendLine();

			outputValidators.AppendLine("function ValidatorOnSubmit(group) {");
			outputValidators.AppendLine("\tif (Page_ValidationActive) {");
			outputValidators.AppendLine("\t\treturn Page_ClientValidate(group);");
			outputValidators.AppendLine("\t}");
			outputValidators.AppendLine("\telse {");
			outputValidators.AppendLine("\t\treturn true;");
			outputValidators.AppendLine("\t}");
			outputValidators.AppendLine("}");
			outputValidators.AppendLine("// -->");

			outputValidators.AppendLine("</script>");

			outputValidatorArray.Append(");");
			outputValidatorArray.AppendLine();
			outputValidatorArray.AppendLine("//]]>");
			outputValidatorArray.AppendLine("</script>");

			this.ViewContext.HttpContext.Items[VALIDATOR_INITIALIZED_CACHE_KEY] = true;
			return outputValidatorArray.ToString() + "\r\n" + outputValidators.ToString();
		}

		private void ValidateAndAddValidator(IValidator newValidator)
		{
			if (this.ViewContext.HttpContext.Items[VALIDATOR_REGISTERED_CACHE_KEY] == null)
			{
				throw new InvalidOperationException("You must register the validation scripts before adding a validator.");
			}

			List<IValidator> validators = this.ViewContext.HttpContext.Items[VALIDATOR_CACHE_KEY] as List<IValidator>;

			if (validators == null)
			{
				validators = new List<IValidator>();
				this.ViewContext.HttpContext.Items[VALIDATOR_CACHE_KEY] = validators;
			}

			if (validators.SingleOrDefault(v => string.Compare(v.Id, newValidator.Id, StringComparison.OrdinalIgnoreCase) == 0) != null)
			{
				throw new ArgumentException("A validator by this name already exists.", "name");
			}

			validators.Add(newValidator);
		}

		public virtual string RequiredValidator(string name, string referenceName, string text)
		{
			RequiredValidator validator = new RequiredValidator(name, referenceName, text);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public virtual string RequiredValidator(string name, string referenceName, string text, IDictionary attributes)
		{
			RequiredValidator validator = new RequiredValidator(name, referenceName, text, attributes);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public virtual string RequiredValidator(string name, string referenceName, string text, string validationGroup)
		{
			RequiredValidator validator = new RequiredValidator(name, referenceName, text, validationGroup);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public virtual string RequiredValidator(string name, string referenceName, string text, string validationGroup, IDictionary attributes)
		{
			RequiredValidator validator = new RequiredValidator(name, referenceName, text, validationGroup, attributes);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public virtual string RequiredValidator(string name, string referenceName, string text, string validationGroup, string initialValue)
		{
			RequiredValidator validator = new RequiredValidator(name, referenceName, text, validationGroup);
			validator.InitialValue = initialValue;
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public virtual string RequiredValidator(string name, string referenceName, string text, string validationGroup, string initialValue, IDictionary attributes)
		{
			RequiredValidator validator = new RequiredValidator(name, referenceName, text, validationGroup, attributes);
			validator.InitialValue = initialValue;
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public IDictionary<string, object> FormValidation()
		{
			if (this.ViewContext.HttpContext.Items[VALIDATOR_REGISTERED_CACHE_KEY] == null)
			{
				throw new InvalidOperationException("You must register the validation scripts before setting up form validation.");
			}

			Dictionary<string, object> values = new Dictionary<string, object>();
			values.Add("onsubmit", "javascript:return WebForm_OnSubmit();");

			return values;
		}

		public IDictionary<string, object> FormValidation(string validationGroup)
		{
			if (this.ViewContext.HttpContext.Items[VALIDATOR_REGISTERED_CACHE_KEY] == null)
			{
				throw new InvalidOperationException("You must register the validation scripts before setting up form validation.");
			}

			if (string.IsNullOrEmpty(validationGroup))
			{
				return FormValidation();
			}

			Dictionary<string, object> values = new Dictionary<string, object>();
			values.Add("onsubmit", "javascript:return WebForm_OnSubmitGroup('" + validationGroup + "');");

			return values;
		}

		public string RegularExpressionValidator(string name, string referenceName, string validationExpression, string text)
		{
			RegularExpressionValidator validator = new RegularExpressionValidator(name, referenceName, validationExpression, text);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public string RegularExpressionValidator(string name, string referenceName, string validationExpression, string text, IDictionary attributes)
		{
			RegularExpressionValidator validator = new RegularExpressionValidator(name, referenceName, validationExpression, text, attributes);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public string RegularExpressionValidator(string name, string referenceName, string validationExpression, string text, string validationGroup)
		{
			RegularExpressionValidator validator = new RegularExpressionValidator(name, referenceName, validationExpression, text, validationGroup);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public string RegularExpressionValidator(string name, string referenceName, string validationExpression, string text, string validationGroup, IDictionary attributes)
		{
			RegularExpressionValidator validator = new RegularExpressionValidator(name, referenceName, validationExpression, text, validationGroup, attributes);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public string CompareValidator(string name, string referenceName, string compareReferenceName, System.Web.UI.WebControls.ValidationDataType type, System.Web.UI.WebControls.ValidationCompareOperator operatorType, string text)
		{
			CompareValidator validator = new CompareValidator(name, referenceName, compareReferenceName, type, operatorType, text);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public string CompareValidator(string name, string referenceName, string compareReferenceName, System.Web.UI.WebControls.ValidationDataType type, System.Web.UI.WebControls.ValidationCompareOperator operatorType, string text, IDictionary attributes)
		{
			CompareValidator validator = new CompareValidator(name, referenceName, compareReferenceName, type, operatorType, text, attributes);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public string CompareValidator(string name, string referenceName, string compareReferenceName, System.Web.UI.WebControls.ValidationDataType type, System.Web.UI.WebControls.ValidationCompareOperator operatorType, string text, string validationGroup)
		{
			CompareValidator validator = new CompareValidator(name, referenceName, compareReferenceName, type, operatorType, text, validationGroup);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public string CompareValidator(string name, string referenceName, string compareReferenceName, System.Web.UI.WebControls.ValidationDataType type, System.Web.UI.WebControls.ValidationCompareOperator operatorType, string text, string validationGroup, IDictionary attributes)
		{
			CompareValidator validator = new CompareValidator(name, referenceName, compareReferenceName, type, operatorType, text, validationGroup, attributes);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public string RangeValidator(string name, string referenceName, string minimumValue, string maximumValue, System.Web.UI.WebControls.ValidationDataType type, string text)
		{
			RangeValidator validator = new RangeValidator(name, referenceName, minimumValue, maximumValue, type, text);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public string RangeValidator(string name, string referenceName, string minimumValue, string maximumValue, System.Web.UI.WebControls.ValidationDataType type, string text, IDictionary attributes)
		{
			RangeValidator validator = new RangeValidator(name, referenceName, minimumValue, maximumValue, type, text, attributes);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public string RangeValidator(string name, string referenceName, string minimumValue, string maximumValue, System.Web.UI.WebControls.ValidationDataType type, string text, string validationGroup)
		{
			RangeValidator validator = new RangeValidator(name, referenceName, minimumValue, maximumValue, type, text, validationGroup);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public string RangeValidator(string name, string referenceName, string minimumValue, string maximumValue, System.Web.UI.WebControls.ValidationDataType type, string text, string validationGroup, IDictionary attributes)
		{
			RangeValidator validator = new RangeValidator(name, referenceName, minimumValue, maximumValue, type, text, validationGroup, attributes);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public string CustomValidator(string name, string referenceName, string clientValidationFunction, string text)
		{
			CustomValidator validator = new CustomValidator(name, referenceName, clientValidationFunction, text);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public string CustomValidator(string name, string referenceName, string clientValidationFunction, string text, IDictionary attributes)
		{
			CustomValidator validator = new CustomValidator(name, referenceName, clientValidationFunction, text, attributes);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public string CustomValidator(string name, string referenceName, string clientValidationFunction, string text, string validationGroup)
		{
			CustomValidator validator = new CustomValidator(name, referenceName, clientValidationFunction, text, validationGroup);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public string CustomValidator(string name, string referenceName, string clientValidationFunction, string text, string validationGroup, IDictionary attributes)
		{
			CustomValidator validator = new CustomValidator(name, referenceName, clientValidationFunction, text, validationGroup, attributes);
			this.ValidateAndAddValidator(validator);
			return validator.ToString();
		}

		public string ElementValidation(ICollection<IValidator> validators)
		{
			return ElementValidation(validators, null);
		}

		public string ElementValidation(ICollection<IValidator> validators, string referenceName)
		{
			StringBuilder output = new StringBuilder();
			foreach (IValidator validator in validators)
			{
				if (string.IsNullOrEmpty(referenceName) || validator.ReferenceId == referenceName)
				{
					this.ValidateAndAddValidator(validator);
					output.AppendLine(validator.ToString());
				}
			}

			return output.ToString();
		}
	}
}