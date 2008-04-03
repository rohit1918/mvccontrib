using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;
using System.Globalization;

namespace MvcContrib.UI.Tags.Validators
{
	public abstract class BaseCompareValidator : BaseValidator
	{
		private ValidationDataType _type = ValidationDataType.String;

		public BaseCompareValidator(string id, string referenceId, string text, ValidationDataType type) : base(id, referenceId, text)
		{
			this.Type = type;
		}

		public BaseCompareValidator(string id, string referenceId, string text, ValidationDataType type, IDictionary attributes) : base(id, referenceId, text, attributes)
		{
			this.Type = type;
		}

		public BaseCompareValidator(string id, string referenceId, string text, ValidationDataType type, string validationGroup) : base(id, referenceId, text, validationGroup)
		{
			this.Type = type;
		}

		public BaseCompareValidator(string id, string referenceId, string text, ValidationDataType type, string validationGroup, IDictionary attributes) : base(id, referenceId, text, validationGroup, attributes)
		{
			this.Type = type;
		}

		public ValidationDataType Type
		{
			get
			{
				return this._type;
			}

			set
			{
				this._type = value;
			}
		}

		public override void RenderClientHookup(StringBuilder output)
		{
			if (this._type != ValidationDataType.String)
			{
				this.NullExpandoSet("type", PropertyConverter.EnumToString(typeof(ValidationDataType), this._type));
				NumberFormatInfo numberFormat = NumberFormatInfo.CurrentInfo;

				switch (this._type)
				{
					case ValidationDataType.Double:
						this.NullExpandoSet("decimalchar", numberFormat.NumberDecimalSeparator);
						break;
					case ValidationDataType.Currency:
						this.NullExpandoSet("decimalchar", numberFormat.CurrencyDecimalSeparator);
						string currencyGroupSeparator = numberFormat.CurrencyGroupSeparator;
						if (currencyGroupSeparator[0] == '\x00a0')
						{
							currencyGroupSeparator = " ";
						}

						this.NullExpandoSet("groupchar", currencyGroupSeparator);
						this.NullExpandoSet("digits", numberFormat.CurrencyDecimalDigits.ToString(NumberFormatInfo.InvariantInfo));

						int groupSize = GetCurrencyGroupSize(numberFormat);
						if (groupSize > 0)
						{
							this.NullExpandoSet("groupsize", groupSize.ToString(NumberFormatInfo.InvariantInfo));
						}

						break;
					case ValidationDataType.Date:
						this.NullExpandoSet("dateorder", GetDateElementOrder());
						this.NullExpandoSet("cutoffyear", DateTimeFormatInfo.CurrentInfo.Calendar.TwoDigitYearMax.ToString(NumberFormatInfo.InvariantInfo));
						int year = DateTime.Today.Year;
						this.NullExpandoSet("century", (year - (year % 100)).ToString(NumberFormatInfo.InvariantInfo));
						break;
				}
			}

			base.RenderClientHookup(output);
		}

		private static int GetCurrencyGroupSize(NumberFormatInfo info)
		{
			int[] currencyGroupSizes = info.CurrencyGroupSizes;
			if ((currencyGroupSizes != null) && (currencyGroupSizes.Length == 1))
			{
				return currencyGroupSizes[0];
			}

			return -1;
		}

		private static string GetDateElementOrder()
		{
			string shortDatePattern = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
			if (shortDatePattern.IndexOf('y') < shortDatePattern.IndexOf('M'))
			{
				return "ymd";
			}

			if (shortDatePattern.IndexOf('M') < shortDatePattern.IndexOf('d'))
			{
				return "mdy";
			}

			return "dmy";
		}
	}
}
