﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;
using System.Web;

namespace MvcContrib.UI.Tags.Validators
{
	public class CompareValidator : BaseCompareValidator
	{
		public CompareValidator(string id, string referenceId, string compareReferenceId, ValidationDataType type, ValidationCompareOperator operatorType, string text)
			: base(id, referenceId, text, type)
		{
			this.OperatorType = operatorType;
			this.CompareReferenceId = compareReferenceId;
		}

		public CompareValidator(string id, string referenceId, string compareReferenceId, ValidationDataType type, ValidationCompareOperator operatorType, string text, IDictionary attributes)
			: base(id, referenceId, text, type, attributes)
		{
			this.OperatorType = operatorType;
			this.CompareReferenceId = compareReferenceId;
		}

		public CompareValidator(string id, string referenceId, string compareReferenceId, ValidationDataType type, ValidationCompareOperator operatorType, string text, string validationGroup)
			: base(id, referenceId, text, type, validationGroup)
		{
			this.OperatorType = operatorType;
			this.CompareReferenceId = compareReferenceId;
		}

		public CompareValidator(string id, string referenceId, string compareReferenceId, ValidationDataType type, ValidationCompareOperator operatorType, string text, string validationGroup, IDictionary attributes)
			: base(id, referenceId, text, type, validationGroup, attributes)
		{
			this.OperatorType = operatorType;
			this.CompareReferenceId = compareReferenceId;
		}

		public ValidationCompareOperator OperatorType
		{
			get
			{
				string value = this.NullExpandoGet("operator");
				return (ValidationCompareOperator)PropertyConverter.EnumFromString(typeof(ValidationCompareOperator), value);
			}

			set
			{
				this.NullExpandoSet("operator", PropertyConverter.EnumToString(typeof(ValidationCompareOperator), value));
			}
		}

		public string CompareReferenceId
		{
			get
			{
				return this.NullExpandoGet("controltocompare");
			}

			set
			{
				this.NullExpandoSet("controltocompare", value);
				this.NullExpandoSet("controlhookup", value);
			}
		}

		public override string ValidationFunction
		{
			get
			{
				return "CompareValidatorEvaluateIsValid";
			}
		}

		public override bool Validate(HttpRequestBase request)
		{
			string formValue1 = request.Form[this.ReferenceId];
			string formValue2 = request.Form[this.CompareReferenceId];

			if (formValue1 == null || (formValue2 == null && this.OperatorType != ValidationCompareOperator.DataTypeCheck))
			{
				this.IsValid = false;
				return false;
			}

			this.IsValid = this.CompareValues(formValue1, formValue2, this.OperatorType);

			return this.IsValid;
		}
	}
}
