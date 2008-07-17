using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;

namespace MvcContrib.UI.Tags.Validators
{
	public class RangeValidator : BaseCompareValidator
	{
		public RangeValidator(string id, string referenceId, string minimumValue, string maximumValue, ValidationDataType type, string text)
			: base(id, referenceId, text, type)
		{
			this.ValidateInput(minimumValue, maximumValue, type);
			this.MinimumValue = minimumValue;
			this.MaximumValue = maximumValue;
		}

		public RangeValidator(string id, string referenceId, string minimumValue, string maximumValue, ValidationDataType type, string text, IDictionary attributes)
			: base(id, referenceId, text, type, attributes)
		{
			this.ValidateInput(minimumValue, maximumValue, type);
			this.MinimumValue = minimumValue;
			this.MaximumValue = maximumValue;
		}

		public RangeValidator(string id, string referenceId, string minimumValue, string maximumValue, ValidationDataType type, string text, string validationGroup)
			: base(id, referenceId, text, type, validationGroup)
		{
			this.ValidateInput(minimumValue, maximumValue, type);
			this.MinimumValue = minimumValue;
			this.MaximumValue = maximumValue;
		}

		public RangeValidator(string id, string referenceId, string minimumValue, string maximumValue, ValidationDataType type, string text, string validationGroup, IDictionary attributes)
			: base(id, referenceId, text, type, validationGroup, attributes)
		{
			this.ValidateInput(minimumValue, maximumValue, type);
			this.MinimumValue = minimumValue;
			this.MaximumValue = maximumValue;
		}

		public string MinimumValue
		{
			get
			{
				return this.NullExpandoGet("minimumvalue");
			}

			set
			{
				this.NullExpandoSet("minimumvalue", value);
			}
		}

		public string MaximumValue
		{
			get
			{
				return this.NullExpandoGet("maximumvalue");
			}

			set
			{
				this.NullExpandoSet("maximumvalue", value);
			}
		}

		public override string ValidationFunction
		{
			get
			{
				return "RangeValidatorEvaluateIsValid";
			}
		}

		private void ValidateInput(string minimumValue, string maximumValue, ValidationDataType type)
		{
			if (type == ValidationDataType.Date || type == ValidationDataType.Currency)
			{
				throw new ArgumentException("The RangeValidator currently does not support Date or Currency types.");
			}

			if (string.IsNullOrEmpty(minimumValue) || !System.Web.UI.WebControls.BaseCompareValidator.CanConvert(minimumValue, type, true))
			{
				throw new ArgumentException("Cannot convert value to selected validation type.", "minimumValue");
			}

			if (string.IsNullOrEmpty(maximumValue) || !System.Web.UI.WebControls.BaseCompareValidator.CanConvert(maximumValue, type, true))
			{
				throw new ArgumentException("Cannot convert value to selected validation type.", "maximumValue");
			}
		}

		public override bool Validate(System.Web.HttpRequestBase request)
		{
			string formValue = request.Form[this.ReferenceId];

			if (formValue == null)
			{
				this.IsValid = false;
				return false;
			}

			this.IsValid = (this.CompareValues(formValue, this.MinimumValue, ValidationCompareOperator.GreaterThanEqual) && this.CompareValues(formValue, this.MaximumValue, ValidationCompareOperator.LessThanEqual));
			return this.IsValid;
		}
	}
}
