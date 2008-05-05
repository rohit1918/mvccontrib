using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MvcContrib.UI.Tags.Validators
{
	public class CustomValidator : BaseValidator
	{
		public CustomValidator(string id, string referenceId, string clientValidationFunction, string text)
			: base(id, referenceId, text)
		{
			this.ClientValidationFunction = clientValidationFunction;
		}

		public CustomValidator(string id, string referenceId, string clientValidationFunction, string text, IDictionary attributes)
			: base(id, referenceId, text, attributes)
		{
			this.ClientValidationFunction = clientValidationFunction;
		}

		public CustomValidator(string id, string referenceId, string clientValidationFunction, string text, string validationGroup)
			: base(id, referenceId, text, validationGroup)
		{
			this.ClientValidationFunction = clientValidationFunction;
		}

		public CustomValidator(string id, string referenceId, string clientValidationFunction, string text, string validationGroup, IDictionary attributes)
			: base(id, referenceId, text, validationGroup, attributes)
		{
			this.ClientValidationFunction = clientValidationFunction;
		}

		public string ClientValidationFunction
		{
			get
			{
				return this.NullExpandoGet("clientvalidationfunction");
			}

			set
			{
				this.NullExpandoSet("clientvalidationfunction", value);
			}
		}

		public override string ValidationFunction
		{
			get
			{
				return "CustomValidatorEvaluateIsValid";
			}
		}
	}
}
