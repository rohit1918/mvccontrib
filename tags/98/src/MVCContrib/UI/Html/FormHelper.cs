using System.Collections;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.UI.Tags;
using System;
using System.Web;
using MvcContrib.Services;
using System.Web.Handlers;
using System.Text;
using MvcContrib.UI.Tags.Validators;
using System.Collections.Generic;
using System.Linq;

namespace MvcContrib.UI.Html
{
	public class FormHelper : BaseHelper, IFormHelper
	{
		public const string CACHE_KEY = "__MvcContribFormHelper__";
		private const string VALIDATOR_CACHE_KEY = "__MvcContrib_Validators__";
		private const string VALIDATOR_REGISTERED_CACHE_KEY = "__MvcContrib_Validations_Registered__";
		private const string VALIDATOR_INITIALIZED_CACHE_KEY = "__MvcContrib_Validators_Initialized__";
		private static string _webValidationUrl;
		private static object _webValidationUrlLock = new object();

		public static IFormHelper GetInstance(ViewContext context)
		{
			if(context.HttpContext.Items.Contains(CACHE_KEY))
				return (IFormHelper)context.HttpContext.Items[CACHE_KEY];

			IFormHelper helper;

			if (DependencyResolver.Resolver != null)
				helper = DependencyResolver.Resolver.GetImplementationOf<IFormHelper>();
			else
				helper = new FormHelper();

			helper.ViewContext = context;
			context.HttpContext.Items.Add(CACHE_KEY, helper);
			return helper;
		}

		public virtual string TextField(string name)
		{
			return TextField(name, Hash.Empty);
		}

		public virtual string TextField(string name, IDictionary attributes)
		{
			TextBox options = new TextBox(attributes);
			options.Name = name;
			return TextField(options);
		}

		public virtual string TextField(TextBox options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (options.Value == null)
				options.Value = ObtainFromViewData(options.Name);

			return options.ToString();
		}

		public virtual string PasswordField(string name)
		{
			return PasswordField(name, Hash.Empty);
		}

		public virtual string PasswordField(string name, IDictionary attributes)
		{
			Password options = new Password(attributes);
			options.Name = name;
			return PasswordField(options);
		}

		public virtual string PasswordField(Password options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (options.Value == null)
				options.Value = ObtainFromViewData(options.Name);

			return options.ToString();
		}

		public virtual string HiddenField(string name)
		{
			return HiddenField(name, Hash.Empty);
		}

		public virtual string HiddenField(string name, IDictionary attributes)
		{
			HiddenField options = new HiddenField(attributes);
			options.Name = name;
			return HiddenField(options);
		}

		public virtual string HiddenField(HiddenField options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (options.Value == null)
				options.Value = ObtainFromViewData(options.Name);

			return options.ToString();
		}

		public virtual string CheckBoxField(string name)
		{
			return CheckBoxField(name, Hash.Empty);
		}

		public virtual string CheckBoxField(string name, IDictionary attributes)
		{
			CheckBoxField options = new CheckBoxField(attributes);
			options.Name = name;
			return CheckBoxField(options);
		}

		public virtual string CheckBoxField(CheckBoxField options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (options.Value == null)
				options.Value = "true";

			if(options.Checked == null)
			{
				object value = ObtainFromViewData(options.Name);
				bool isChecked = value != null && value is bool && (bool)value;

				options.Checked = isChecked;
			}

			HiddenField hidden = new HiddenField();
			hidden.Value = "false";
			if(!string.IsNullOrEmpty(options.Id))
				hidden.Id = options.Id + "H";
			hidden.Name = options.Name;

			return options.ToString() + hidden.ToString();
		}

		public virtual string TextArea(string name)
		{
			return TextArea(name, Hash.Empty);
		}

		public virtual string TextArea(string name, IDictionary attributes)
		{
			TextArea options = new TextArea(attributes);
			options.Name = name;
			return TextArea(options);
		}

		public virtual string TextArea(TextArea options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (string.IsNullOrEmpty(options.InnerText))
			{
				object value = ObtainFromViewData(options.Name);
				options.InnerText = value != null ? value.ToString() : null;
			}
			return options.ToString();
		}

		public virtual string Submit()
		{
			return Submit("Submit");
		}

		public virtual string Submit(string value)
		{
			SubmitButton button = new SubmitButton();
			button.Value = value;
			return Submit(button);
		}

		public virtual string Submit(string value, IDictionary attributes)
		{
			SubmitButton button = new SubmitButton(attributes);

			if (button.Value == null)
				button.Value = value;

			return Submit(button);
		}

		public virtual string Submit(SubmitButton options)
		{
			return options.ToString();
		}

		public virtual string ImageButton(string src, string alt)
		{
			InputImage options = new InputImage(src);
			options.Alt = alt;
			return options.ToString();
		}

		public virtual string ImageButton(string src, string alt, IDictionary attributes)
		{
			InputImage options = new InputImage(src, attributes);
			options.Alt = alt;
			return options.ToString();
		}

		public virtual string ImageButton(InputImage options)
		{
			return options.ToString();
		}

		public virtual string Select(string name, object dataSource, string textField, string valueField)
		{
			return Select(name, dataSource, textField, valueField, Hash.Empty);
		}

		public virtual string Select(string name, object dataSource, string textField, string valueField, IDictionary attributes)
		{
			string firstOption = ObtainAndRemove(attributes, "firstOption");
			string firstOptionValue = ObtainAndRemove(attributes, "firstOptionValue");
			string selectedValue = ObtainAndRemove(attributes, "selectedValue");

			Select select = new Select(attributes);
			select.Name = name;
			select.TextField = textField;
			select.ValueField = valueField;
			select.FirstOption = firstOption;
			select.FirstOptionValue = firstOptionValue;
			select.SelectedValue = selectedValue;
			return Select(dataSource, select);
		}

		public virtual string Select(object dataSource, Select options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (options.SelectedValue == null)
			{
				object value = ObtainFromViewData(options.Name);
				options.SelectedValue = value != null ? value.ToString() : null;
			}

			ProcessDataSource(dataSource, options.TextField, options.ValueField, delegate(int count, object text, object value)
			{
				options.AddOption(value.ToString(), text.ToString());
			});

			return options.ToString();
		}

		public virtual string RadioField(string name, object value)
		{
			return RadioField(name, value, Hash.Empty);
		}

		public virtual string RadioField(string name, object value, IDictionary attributes)
		{
			RadioField options = new RadioField(attributes);
			options.Name = name;
			options.Value = value;
			return RadioField(options);
		}

		public virtual string RadioField(RadioField options)
		{
			if(options.Value != null)
			{
				object dataValue = ObtainFromViewData(options.Name);
				if(dataValue != null && dataValue.ToString().Equals(options.Value))
				{
					options.Checked = true;
				}
			}

			if (string.IsNullOrEmpty(options.Id))
			{
				string id = options.Name;
				if(options.Value != null)
				{
					id += "-" + options.Value.ToString().Replace(" ", string.Empty);
				}
				options.Id = id;
			}

			return options.ToString();
		}

		public virtual CheckBoxList CheckBoxList(string name, object dataSource, string textField, string valueField)
		{
			return CheckBoxList(name, dataSource, textField, valueField, Hash.Empty);
		}

		public virtual CheckBoxList CheckBoxList(string name, object dataSource, string textField, string valueField, IDictionary attributes)
		{
			CheckBoxList list = new CheckBoxList(attributes);
			list.Name = name;
			list.TextField = textField;
			list.ValueField = valueField;
			return CheckBoxList(dataSource, list);
		}

		public virtual CheckBoxList CheckBoxList(object dataSource, CheckBoxList options)
		{
			ProcessDataSource(dataSource, options.TextField, options.ValueField, delegate(int count, object text, object value)
			{
				CheckBoxField field = new CheckBoxField();
				field.Name = options.Name;
				field.Label = text.ToString();
				field.Id = field.Name + "-" + count;
				field.Value = value;

				options.Add(field);                                                               		
			});

			return options;
		}

		public virtual RadioList RadioList(string name, object dataSource, string textField, string valueField)
		{
			return RadioList(name, dataSource, textField, valueField, Hash.Empty);
		}

		public virtual RadioList RadioList(string name, object dataSource, string textField, string valueField, IDictionary attributes)
		{
			RadioList list = new RadioList(attributes);
			list.Name = name;
			list.TextField = textField;
			list.ValueField = valueField;
			return RadioList(dataSource, list);
		}

		public virtual RadioList RadioList(object dataSource, RadioList options)
		{
			ProcessDataSource(dataSource, options.TextField, options.ValueField, delegate(int count, object text, object value) 
			{
				RadioField field = new RadioField();
				field.Name = options.Name;
				field.Label = text.ToString();
				field.Id = field.Name + "-" + count;
				field.Value = value;

				options.Add(field);
			});
			return options;
		}

		protected void ProcessDataSource(object dataSource, string textField, string valueField, Action<int, object, object> forEachItemInDataSource)
		{
			IEnumerable ds = dataSource as IEnumerable;
			PropertyInfo textProperty = null;
			PropertyInfo valueProperty = null;

			if (ds != null && textField != null && valueField != null)
			{
				IEnumerator enumerator = ds.GetEnumerator();

				if (enumerator.MoveNext())
				{
					Type type = enumerator.Current.GetType();
					textProperty = type.GetProperty(textField);
					valueProperty = type.GetProperty(valueField);
				}
			}

			if (textProperty != null && valueProperty != null)
			{
				int count = 0;

				foreach (object item in ds)
				{
					object value = valueProperty.GetValue(item, null);
					object text = textProperty.GetValue(item, null);

					forEachItemInDataSource(count++, text, value);
				}
			}

		}

		public void For<T>(string viewDataKey, string url, IDictionary attributes, Action<SmartForm<T>> block)
		{
			object raw = ObtainFromViewData(viewDataKey);

			T item;

			if(raw == null)
			{
				item = default(T);
			}
			else
			{
				//TODO: Introduce type checks.
				item = (T)raw;
			}

			SmartForm<T> form = new SmartForm<T>(viewDataKey, url, block, this, item, attributes);
			ViewContext.HttpContext.Response.Output.Write(form.ToString());
		}

		public void For<T>(T dataItem, string url, Action<SmartForm<T>> block)
		{
			For(dataItem, url, Hash.Empty, block);
		}

		public void For<T>(T dataItem, string url, IDictionary attributes, Action<SmartForm<T>> block)
		{
			SmartForm<T> form = new SmartForm<T>(url, block, this, dataItem, attributes);
			ViewContext.HttpContext.Response.Output.Write(form.ToString());
		}


		protected virtual object ObtainFromViewData(string id)
		{
			if (Binder != null)
			{
				return Binder.ExtractValue(id, ViewContext);
			}
			else
			{
				return null;
			}
		}

		protected string ObtainAndRemove(IDictionary dictionary, string key)
		{
			if(!dictionary.Contains(key))
				return null;

			string item = dictionary[key].ToString();
			dictionary.Remove(key);
			return item;
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
						_webValidationUrl = (string)webResourceUrlMethod.Invoke(null, new object[] { systemWebAssembly, "WebUIValidation.js", false });
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

			List<BaseValidator> validators = this.ViewContext.HttpContext.Items[VALIDATOR_CACHE_KEY] as List<BaseValidator>;

			if (validators != null)
			{
				foreach (BaseValidator baseValidator in validators)
				{
					if (!firstValidator)
					{
						outputValidatorArray.Append(", ");
					}

					outputValidatorArray.Append("document.getElementById(\"");
					outputValidatorArray.Append(baseValidator.Id.Replace('.', '-'));
					outputValidatorArray.Append("\")");
					firstValidator = false;

					baseValidator.RenderClientHookup(outputValidators);
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

		private void ValidateAndAddValidator(BaseValidator newValidator)
		{
			if (this.ViewContext.HttpContext.Items[VALIDATOR_REGISTERED_CACHE_KEY] == null)
			{
				throw new InvalidOperationException("You must register the validation scripts before adding a validator.");
			}

			List<BaseValidator> validators = this.ViewContext.HttpContext.Items[VALIDATOR_CACHE_KEY] as List<BaseValidator>;

			if (validators == null)
			{
				validators = new List<BaseValidator>();
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
	}
}
