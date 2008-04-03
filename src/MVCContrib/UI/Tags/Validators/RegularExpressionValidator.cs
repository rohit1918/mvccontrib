﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MvcContrib.UI.Tags.Validators
{
	public class RegularExpressionValidator : BaseValidator
	{
		public RegularExpressionValidator(string id, string referenceId, string validationExpression, string text)
			: base(id, referenceId, text)
		{
			this.ValidationExpression = validationExpression;
		}

		public RegularExpressionValidator(string id, string referenceId, string validationExpression, string text, IDictionary attributes)
			: base(id, referenceId, text, attributes)
		{
			this.ValidationExpression = validationExpression;
		}

		public RegularExpressionValidator(string id, string referenceId, string validationExpression, string text, string validationGroup)
			: base(id, referenceId, text, validationGroup)
		{
			this.ValidationExpression = validationExpression;
		}

		public RegularExpressionValidator(string id, string referenceId, string validationExpression, string text, string validationGroup, IDictionary attributes)
			: base(id, referenceId, text, validationGroup, attributes)
		{
			this.ValidationExpression = validationExpression;
		}

		public string ValidationExpression
		{
			get
			{
				return this.NullExpandoGet("validationexpression");
			}

			set
			{
				this.NullExpandoSet("validationexpression", value);
			}
		}

		public override string ValidationFunction
		{
			get 
			{
				return "RegularExpressionValidatorEvaluateIsValid";
			}
		}
	}
}
