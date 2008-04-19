using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MvcContrib.UI.Tags.Validators;
using NUnit.Framework.SyntaxHelpers;
using System.Text.RegularExpressions;
using System.Globalization;

namespace MvcContrib.UnitTests.UI.Html
{
	public class ValidatorTester
	{
		[TestFixture]
		public class Required_Validator_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				RequiredValidator validator = new RequiredValidator("myId", "myReference", "Error!");
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.ValidationFunction, Is.EqualTo("RequiredFieldValidatorEvaluateIsValid"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				RequiredValidator validator = new RequiredValidator("myId", "myReference", "Error!");
				validator.InitialValue = "test";
				Assert.That(validator.InitialValue, Is.EqualTo("test"));
				validator.ErrorMessage = "error";
				Assert.That(validator.ErrorMessage, Is.EqualTo("error"));
				validator.ValidationGroup = "group";
				Assert.That(validator.ValidationGroup, Is.EqualTo("group"));
				validator.ReferenceId = "reference";
				Assert.That(validator.ReferenceId, Is.EqualTo("reference"));
			}

			[Test]
			public void Empty_property_returns_null_for_expando_attribute()
			{
				RequiredValidator validator = new RequiredValidator("myId", "myReference", "Error!");
				Assert.IsNull(validator.ValidationGroup);
			}

			[Test]
			public void When_Creating_element_with_validation_group_it_sticks()
			{
				RequiredValidator validator = new RequiredValidator("myId", "myReference", "Error!", "test");
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
			}

			[Test]
			public void When_creating_element_with_dictionary_it_sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				RequiredValidator validator = new RequiredValidator("myId", "myReference", "Error!", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void When_creating_element_with_validation_group_and_dictionary_it_sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				RequiredValidator validator = new RequiredValidator("myId", "myReference", "Error!", "test", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void Validate_Rendering()
			{
				RequiredValidator validator = new RequiredValidator("myId", "myReference", "Error!");
				Assert.That(validator.ToString(), Is.EqualTo("<span id=\"myId\" style=\"display:none;color:red;\">Error!</span>"));
			}

			[Test]
			public void Validate_Rendering_client_hookup()
			{
				RequiredValidator validator = new RequiredValidator("myId", "myReference", "Error!");
				Regex outputRegex = new Regex(@"var\smyId.*;.*myId.controltovalidate\s=\s""myReference"";.*myId.evaluationfunction\s=\s""RequiredFieldValidatorEvaluateIsValid"";.*", RegexOptions.Singleline);
				StringBuilder output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));
			}
		}

		[TestFixture]
		public class Regular_Expression_Validator_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				RegularExpressionValidator validator = new RegularExpressionValidator("myId", "myReference", ".*", "Error!");
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.ValidationFunction, Is.EqualTo("RegularExpressionValidatorEvaluateIsValid"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				RegularExpressionValidator validator = new RegularExpressionValidator("myId", "myReference", ".*", "Error!");
				Assert.That(validator.ValidationExpression, Is.EqualTo(".*"));
				validator.ErrorMessage = "error";
				Assert.That(validator.ErrorMessage, Is.EqualTo("error"));
				validator.ValidationGroup = "group";
				Assert.That(validator.ValidationGroup, Is.EqualTo("group"));
				validator.ReferenceId = "reference";
				Assert.That(validator.ReferenceId, Is.EqualTo("reference"));
			}

			[Test]
			public void When_Creating_element_with_validation_group_it_sticks()
			{
				RegularExpressionValidator validator = new RegularExpressionValidator("myId", "myReference", ".*", "Error!", "test");
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
			}

			[Test]
			public void When_creating_element_with_dictionary_it_sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				RegularExpressionValidator validator = new RegularExpressionValidator("myId", "myReference", ".*", "Error!", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ValidationExpression, Is.EqualTo(".*"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void When_creating_element_with_validation_group_and_dictionary_it_sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				RegularExpressionValidator validator = new RegularExpressionValidator("myId", "myReference", ".*", "Error!", "test", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ValidationExpression, Is.EqualTo(".*"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void Validate_Rendering()
			{
				RegularExpressionValidator validator = new RegularExpressionValidator("myId", "myReference", ".*", "Error!");
				Assert.That(validator.ToString(), Is.EqualTo("<span id=\"myId\" style=\"display:none;color:red;\">Error!</span>"));
			}

			[Test]
			public void Validate_Rendering_client_hookup()
			{
				RegularExpressionValidator validator = new RegularExpressionValidator("myId", "myReference", ".*", "Error!");
				Regex outputRegex = new Regex(@"var\smyId.*;.*myId.controltovalidate\s=\s""myReference"";.*myId.validationexpression\s=\s""\.\*"";.*myId.evaluationfunction\s=\s""RegularExpressionValidatorEvaluateIsValid"";.*", RegexOptions.Singleline);
				StringBuilder output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));
			}
		}

		[TestFixture]
		public class Compare_Validator_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				CompareValidator validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.ValidationFunction, Is.EqualTo("CompareValidatorEvaluateIsValid"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				CompareValidator validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				Assert.That(validator.CompareReferenceId, Is.EqualTo("compareReference"));
				Assert.That(validator.Type, Is.EqualTo(System.Web.UI.WebControls.ValidationDataType.String));
				Assert.That(validator.OperatorType, Is.EqualTo(System.Web.UI.WebControls.ValidationCompareOperator.Equal));
				validator.ErrorMessage = "error";
				Assert.That(validator.ErrorMessage, Is.EqualTo("error"));
				validator.ValidationGroup = "group";
				Assert.That(validator.ValidationGroup, Is.EqualTo("group"));
				validator.ReferenceId = "reference";
				Assert.That(validator.ReferenceId, Is.EqualTo("reference"));
			}

			[Test]
			public void When_Creating_element_with_validation_group_it_sticks()
			{
				CompareValidator validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!", "test");
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
			}

			[Test]
			public void When_creating_element_with_dictionary_it_sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				CompareValidator validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void When_creating_element_with_validation_group_and_dictionary_it_sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				CompareValidator validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!", "test", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void Validate_Rendering()
			{
				CompareValidator validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				Assert.That(validator.ToString(), Is.EqualTo("<span id=\"myId\" style=\"display:none;color:red;\">Error!</span>"));
			}

			[Test]
			public void Validate_Rendering_client_hookup()
			{
				CompareValidator validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				Regex outputRegex = new Regex(@"var\smyId.*;.*myId.controltovalidate\s=\s""myReference"";.*myId.operator\s=\s""Equal"";.*myId.controltocompare\s=\s""compareReference"";.*myId.controlhookup\s=\s""compareReference"".*myId.evaluationfunction\s=\s""CompareValidatorEvaluateIsValid"";.*", RegexOptions.Singleline);
				StringBuilder output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));
			}

			[Test]
			public void Validate_Rendering_double_client_hookup()
			{
				CultureInfo priorInfo = CultureInfo.CurrentCulture;
				System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

				CompareValidator validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				Regex outputRegex = new Regex(@"var\smyId.*;.*myId.controltovalidate\s=\s""myReference"";.*myId.operator\s=\s""Equal"";.*myId.controltocompare\s=\s""compareReference"";.*myId.controlhookup\s=\s""compareReference"".*myId.type\s=\s""Double"";.*myId.decimalchar\s=\s""\."";.*myId.evaluationfunction\s=\s""CompareValidatorEvaluateIsValid"";.*", RegexOptions.Singleline);
				StringBuilder output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));

				System.Threading.Thread.CurrentThread.CurrentCulture = priorInfo;
			}

			[Test]
			public void Validate_Rendering_currency_client_hookup()
			{
				CultureInfo priorInfo = CultureInfo.CurrentCulture;
				System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

				CompareValidator validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				Regex outputRegex = new Regex(@"var\smyId.*;.*myId.controltovalidate\s=\s""myReference"";.*myId.operator\s=\s""Equal"";.*myId.controltocompare\s=\s""compareReference"";.*myId.controlhookup\s=\s""compareReference"".*myId.type\s=\s""Currency"";.*myId.decimalchar\s=\s""\."";.*myId.groupchar\s=\s"","";.*myId.digits\s=\s""2"";.*myId.groupsize\s=\s""3"";.*myId.evaluationfunction\s=\s""CompareValidatorEvaluateIsValid"";.*", RegexOptions.Singleline);
				StringBuilder output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));

				System.Threading.Thread.CurrentThread.CurrentCulture = priorInfo;
			}

			[Test]
			public void Validate_Rendering_currency_empty_group_separator_client_hookup()
			{
				CultureInfo priorInfo = CultureInfo.CurrentCulture;
				System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", true);
				CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator = "\x00a0";

				CompareValidator validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				Regex outputRegex = new Regex(@".*myId.groupchar\s=\s"" "";.*", RegexOptions.Singleline);
				StringBuilder output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));

				System.Threading.Thread.CurrentThread.CurrentCulture = priorInfo;
			}

			[Test]
			public void Validate_Rendering_currency_variable_group_size_client_hookup()
			{
				CultureInfo priorInfo = CultureInfo.CurrentCulture;
				System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", true);
				CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSizes = new int[] { 2, 3 };

				CompareValidator validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				Regex outputRegex = new Regex(@".*myId.groupsize.*", RegexOptions.Singleline);
				StringBuilder output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsFalse(outputRegex.IsMatch(output.ToString()));

				System.Threading.Thread.CurrentThread.CurrentCulture = priorInfo;
			}

			[Test]
			public void Validate_Rendering_date_client_hookup()
			{
				CultureInfo priorInfo = CultureInfo.CurrentCulture;
				System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

				CompareValidator validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				Regex outputRegex = new Regex(@"var\smyId.*;.*myId.controltovalidate\s=\s""myReference"";.*myId.operator\s=\s""Equal"";.*myId.controltocompare\s=\s""compareReference"";.*myId.controlhookup\s=\s""compareReference"".*myId.type\s=\s""Date"";.*myId.dateorder\s=\s""mdy"";.*myId.cutoffyear\s=\s""2029"";.*myId.century\s=\s""2000"";.*myId.evaluationfunction\s=\s""CompareValidatorEvaluateIsValid"";.*", RegexOptions.Singleline);
				StringBuilder output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));

				System.Threading.Thread.CurrentThread.CurrentCulture = priorInfo;
			}

			[Test]
			public void Validate_Rendering_date_YMD_format_client_hookup()
			{
				CultureInfo priorInfo = CultureInfo.CurrentCulture;
				System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", true);
				CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy/M/d";

				CompareValidator validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				Regex outputRegex = new Regex(@".*myId.dateorder\s=\s""ymd"";.*", RegexOptions.Singleline);
				StringBuilder output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));

				System.Threading.Thread.CurrentThread.CurrentCulture = priorInfo;
			}

			[Test]
			public void Validate_Rendering_date_DMY_format_client_hookup()
			{
				CultureInfo priorInfo = CultureInfo.CurrentCulture;
				System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", true);
				CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern = "d/M/yyyy";

				CompareValidator validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				Regex outputRegex = new Regex(@".*myId.dateorder\s=\s""dmy"";.*", RegexOptions.Singleline);
				StringBuilder output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));

				System.Threading.Thread.CurrentThread.CurrentCulture = priorInfo;
			}
		}

		[TestFixture]
		public class Range_Validator_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				RangeValidator validator = new RangeValidator("myId", "myReference", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "Error!");
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.ValidationFunction, Is.EqualTo("RangeValidatorEvaluateIsValid"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				RangeValidator validator = new RangeValidator("myId", "myReference", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "Error!");
				Assert.That(validator.MinimumValue, Is.EqualTo("1"));
				Assert.That(validator.MaximumValue, Is.EqualTo("10"));
				validator.ErrorMessage = "error";
				Assert.That(validator.ErrorMessage, Is.EqualTo("error"));
				validator.ValidationGroup = "group";
				Assert.That(validator.ValidationGroup, Is.EqualTo("group"));
				validator.ReferenceId = "reference";
				Assert.That(validator.ReferenceId, Is.EqualTo("reference"));
			}

			[Test]
			public void When_Creating_element_with_validation_group_it_sticks()
			{
				RangeValidator validator = new RangeValidator("myId", "myReference", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "Error!", "test");
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
			}

			[Test]
			public void When_creating_element_with_dictionary_it_sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				RangeValidator validator = new RangeValidator("myId", "myReference", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "Error!", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.MinimumValue, Is.EqualTo("1"));
				Assert.That(validator.MaximumValue, Is.EqualTo("10"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void When_creating_element_with_validation_group_and_dictionary_it_sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				RangeValidator validator = new RangeValidator("myId", "myReference", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "Error!", "test", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.MinimumValue, Is.EqualTo("1"));
				Assert.That(validator.MaximumValue, Is.EqualTo("10"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void Validate_Rendering()
			{
				RangeValidator validator = new RangeValidator("myId", "myReference", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "Error!");
				Assert.That(validator.ToString(), Is.EqualTo("<span id=\"myId\" style=\"display:none;color:red;\">Error!</span>"));
			}

			[Test]
			public void Validate_Rendering_client_hookup()
			{
				RangeValidator validator = new RangeValidator("myId", "myReference", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "Error!");
				Regex outputRegex = new Regex(@"var\smyId.*;.*myId.controltovalidate\s=\s""myReference"";.*myId.minimumvalue\s=\s""1"";.*myId.maximumvalue\s=\s""10"";.*myId.type\s=\s""Double"";.*myId.evaluationfunction\s=\s""RangeValidatorEvaluateIsValid"";.*", RegexOptions.Singleline);
				StringBuilder output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));
			}

			[Test, ExpectedException(typeof(ArgumentException))]
			public void When_validation_type_is_date()
			{
				RangeValidator validator = new RangeValidator("myId", "myReference", "1/1/2008", "3/1/2008", System.Web.UI.WebControls.ValidationDataType.Date, "Error!");
			}

			[Test, ExpectedException(typeof(ArgumentException))]
			public void When_validation_type_is_currency()
			{
				RangeValidator validator = new RangeValidator("myId", "myReference", "$1.00", "$3.00", System.Web.UI.WebControls.ValidationDataType.Currency, "Error!");
			}

			[Test, ExpectedException(typeof(ArgumentException))]
			public void When_minimum_value_is_not_valid_type()
			{
				RangeValidator validator = new RangeValidator("myId", "myReference", "abcd", "1234", System.Web.UI.WebControls.ValidationDataType.Double, "Error!");
			}

			[Test, ExpectedException(typeof(ArgumentException))]
			public void When_maximum_value_is_not_valid_type()
			{
				RangeValidator validator = new RangeValidator("myId", "myReference", "1234", "abcd", System.Web.UI.WebControls.ValidationDataType.Double, "Error!");
			}

			[Test, ExpectedException(typeof(ArgumentException))]
			public void When_minimum_value_is_not_specified()
			{
				RangeValidator validator = new RangeValidator("myId", "myReference", "", "1234", System.Web.UI.WebControls.ValidationDataType.Double, "Error!");
			}

			[Test, ExpectedException(typeof(ArgumentException))]
			public void When_maximum_value_is_not_specified()
			{
				RangeValidator validator = new RangeValidator("myId", "myReference", "1234", "", System.Web.UI.WebControls.ValidationDataType.Double, "Error!");
			}
		}

		[TestFixture]
		public class Custom_Validator_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				CustomValidator validator = new CustomValidator("myId", "myReference", "MyFunction", "Error!");
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.ValidationFunction, Is.EqualTo("CustomValidatorEvaluateIsValid"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				CustomValidator validator = new CustomValidator("myId", "myReference", "MyFunction", "Error!");
				Assert.That(validator.ClientValidationFunction, Is.EqualTo("MyFunction"));
				validator.ErrorMessage = "error";
				Assert.That(validator.ErrorMessage, Is.EqualTo("error"));
				validator.ValidationGroup = "group";
				Assert.That(validator.ValidationGroup, Is.EqualTo("group"));
				validator.ReferenceId = "reference";
				Assert.That(validator.ReferenceId, Is.EqualTo("reference"));
			}

			[Test]
			public void When_Creating_element_with_validation_group_it_sticks()
			{
				CustomValidator validator = new CustomValidator("myId", "myReference", "MyFunction", "Error!", "test");
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ClientValidationFunction, Is.EqualTo("MyFunction"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
			}

			[Test]
			public void When_creating_element_with_dictionary_it_sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				CustomValidator validator = new CustomValidator("myId", "myReference", "MyFunction", "Error!", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ClientValidationFunction, Is.EqualTo("MyFunction"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void When_creating_element_with_validation_group_and_dictionary_it_sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				CustomValidator validator = new CustomValidator("myId", "myReference", "MyFunction", "Error!", "test", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ClientValidationFunction, Is.EqualTo("MyFunction"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void Validate_Rendering()
			{
				CustomValidator validator = new CustomValidator("myId", "myReference", "MyFunction", "Error!");
				Assert.That(validator.ToString(), Is.EqualTo("<span id=\"myId\" style=\"display:none;color:red;\">Error!</span>"));
			}

			[Test]
			public void Validate_Rendering_client_hookup()
			{
				CustomValidator validator = new CustomValidator("myId", "myReference", "MyFunction", "Error!");
				Regex outputRegex = new Regex(@"var\smyId.*;.*myId.controltovalidate\s=\s""myReference"";.*myId.clientvalidationfunction\s=\s""MyFunction"";.*myId.evaluationfunction\s=\s""CustomValidatorEvaluateIsValid"";.*", RegexOptions.Singleline);
				StringBuilder output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));
			}
		}
	}
}
