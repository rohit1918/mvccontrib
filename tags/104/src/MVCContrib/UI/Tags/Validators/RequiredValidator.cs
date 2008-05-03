using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MvcContrib.UI.Tags.Validators
{
	public class RequiredValidator : BaseValidator
	{
		public RequiredValidator(string id, string referenceId, string text) : base(id, referenceId, text)
		{
			this.InitialValue = string.Empty;
		}

		public RequiredValidator(string id, string referenceId, string text, IDictionary attributes)	: base(id, referenceId, text, string.Empty, attributes)
		{
			this.InitialValue = string.Empty;
		}

		public RequiredValidator(string id, string referenceId, string text, string validationGroup) : base(id, referenceId, text, validationGroup)
		{
			this.InitialValue = string.Empty;
		}

		public RequiredValidator(string id, string referenceId, string text, string validationGroup, IDictionary attributes) : base(id, referenceId, text, validationGroup, attributes)
		{
			this.InitialValue = string.Empty;
		}

		public override string ValidationFunction
		{
			get 
			{
				return "RequiredFieldValidatorEvaluateIsValid"; 
			}
		}

		public string InitialValue
		{
			get
			{
				return this.NullExpandoGet("initialvalue");
			}

			set
			{
				this.NullExpandoSet("initialvalue", value);
			}
		}
	}
}
