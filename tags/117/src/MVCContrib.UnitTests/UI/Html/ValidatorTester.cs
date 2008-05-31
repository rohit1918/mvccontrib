using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MvcContrib.UI.Tags.Validators;
using NUnit.Framework.SyntaxHelpers;
using System.Text.RegularExpressions;
using System.Globalization;
using Rhino.Mocks;
using System.Web;
using System.Collections.Specialized;

namespace MvcContrib.UnitTests.UI.Html
{
	public class ValidatorTester
	{
		[TestFixture]
		public class Base_Validator
		{
			[Test]
			public void All_Validators_Are_Valid()
			{
				RequiredValidator validator1 = new RequiredValidator("myid1", "refid1", "Error!");
				RequiredValidator validator2 = new RequiredValidator("myid2", "refid2", "Error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid1", "1234");
				formValues.Add("refid2", "5678");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(BaseValidator.Validate(request, new IValidator[] { validator1, validator2 }));
					Assert.IsTrue(validator1.IsValid);
					Assert.IsTrue(validator2.IsValid);
				}
			}

			[Test]
			public void Single_Validator_Not_Valid()
			{
				RequiredValidator validator1 = new RequiredValidator("myid1", "refid1", "Error!");
				RequiredValidator validator2 = new RequiredValidator("myid2", "refid2", "Error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid1", "1234");
				formValues.Add("refid2", "");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(BaseValidator.Validate(request, new IValidator[] { validator1, validator2 }));
					Assert.IsTrue(validator1.IsValid);
					Assert.IsFalse(validator2.IsValid);
				}
			}

			[Test]
			public void Multiple_Validators_Not_Valid()
			{
				RequiredValidator validator1 = new RequiredValidator("myid1", "refid1", "Error!");
				RequiredValidator validator2 = new RequiredValidator("myid2", "refid2", "Error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid1", "");
				formValues.Add("refid2", "");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(BaseValidator.Validate(request, new IValidator[] { validator1, validator2 }));
					Assert.IsFalse(validator1.IsValid);
					Assert.IsFalse(validator2.IsValid);
				}
			}
		}

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

			[Test]
			public void Validation_On_Value_Present()
			{
				RequiredValidator validator = new RequiredValidator("myid", "refid", "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid", "value");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator.Validate(request));
					Assert.IsTrue(validator.IsValid);
				}
			}

			[Test]
			public void Validation_On_No_Value_Present()
			{
				RequiredValidator validator = new RequiredValidator("myid", "refid", "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid", "");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator.Validate(request));
					Assert.IsFalse(validator.IsValid);
				}
			}

			[Test]
			public void Validation_On_No_Form_Value_Present()
			{
				RequiredValidator validator = new RequiredValidator("myid", "refid", "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;

				using (mocks.Record())
				{
					request = mocks.DynamicHttpRequestBase();
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator.Validate(request));
				}
			}

			[Test]
			public void Rendering_Validator_When_IsValid_False_Initially_Displays()
			{
				RequiredValidator validator = new RequiredValidator("myid", "refid", "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid", "");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator.Validate(request));
					Assert.That(validator.ToString(), Is.EqualTo("<span id=\"myid\" style=\"color:red;\">error!</span>"));
				}
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

			[Test]
			public void Validation_On_Value_Present()
			{
				RegularExpressionValidator validator = new RegularExpressionValidator("myid", "refid", "^\\d*$", "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid", "1234");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator.Validate(request));
					Assert.IsTrue(validator.IsValid);
				}
			}

			[Test]
			public void Validation_On_No_Value_Present()
			{
				RegularExpressionValidator validator = new RegularExpressionValidator("myid", "refid", "^\\d*$", "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid", "value");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator.Validate(request));
					Assert.IsFalse(validator.IsValid);
				}
			}

			[Test]
			public void Validation_On_No_Form_Value_Present()
			{
				RegularExpressionValidator validator = new RegularExpressionValidator("myid", "refid", "^\\d*$", "error!");

				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;

				using (mocks.Record())
				{
					request = mocks.DynamicHttpRequestBase();
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator.Validate(request));
				}
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

			[Test]
			public void Validation_On_Date_Value_Present()
			{
				CompareValidator validator1 = new CompareValidator("myid", "refid1", "compareRefId1", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				CompareValidator validator2 = new CompareValidator("myid", "refid2", "compareRefId2", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThan, "error!");
				CompareValidator validator3 = new CompareValidator("myid", "refid3", "compareRefId3", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				CompareValidator validator4 = new CompareValidator("myid", "refid4", "compareRefId4", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				CompareValidator validator5 = new CompareValidator("myid", "refid5", "compareRefId5", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.LessThan, "error!");
				CompareValidator validator6 = new CompareValidator("myid", "refid6", "compareRefId6", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				CompareValidator validator7 = new CompareValidator("myid", "refid7", "compareRefId7", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				CompareValidator validator8 = new CompareValidator("myid", "refid8", "compareRefId8", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.NotEqual, "error!");
				CompareValidator validator9 = new CompareValidator("myid", "refid9", "compareRefId9", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.DataTypeCheck, "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid1", "1/5/2008");
				formValues.Add("compareRefId1", "1/5/2008");
				formValues.Add("refid2", "1/6/2008");
				formValues.Add("compareRefId2", "1/5/2008");
				formValues.Add("refid3", "1/6/2008");
				formValues.Add("compareRefId3", "1/5/2008");
				formValues.Add("refid4", "1/5/2008");
				formValues.Add("compareRefId4", "1/5/2008");
				formValues.Add("refid5", "1/4/2008");
				formValues.Add("compareRefId5", "1/5/2008");
				formValues.Add("refid6", "1/4/2008");
				formValues.Add("compareRefId6", "1/5/2008");
				formValues.Add("refid7", "1/5/2008");
				formValues.Add("compareRefId7", "1/5/2008");
				formValues.Add("refid8", "1/4/2008");
				formValues.Add("compareRefId8", "1/5/2008");
				formValues.Add("refid9", "1/4/2008");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator1.Validate(request));
					Assert.IsTrue(validator1.IsValid);
					Assert.IsTrue(validator2.Validate(request));
					Assert.IsTrue(validator2.IsValid);
					Assert.IsTrue(validator3.Validate(request));
					Assert.IsTrue(validator3.IsValid);
					Assert.IsTrue(validator4.Validate(request));
					Assert.IsTrue(validator4.IsValid);
					Assert.IsTrue(validator5.Validate(request));
					Assert.IsTrue(validator5.IsValid);
					Assert.IsTrue(validator6.Validate(request));
					Assert.IsTrue(validator6.IsValid);
					Assert.IsTrue(validator7.Validate(request));
					Assert.IsTrue(validator7.IsValid);
				}
			}

			[Test]
			public void Validation_On_Currency_Value_Present()
			{
				CompareValidator validator1 = new CompareValidator("myid", "refid1", "compareRefId1", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				CompareValidator validator2 = new CompareValidator("myid", "refid2", "compareRefId2", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThan, "error!");
				CompareValidator validator3 = new CompareValidator("myid", "refid3", "compareRefId3", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				CompareValidator validator4 = new CompareValidator("myid", "refid4", "compareRefId4", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				CompareValidator validator5 = new CompareValidator("myid", "refid5", "compareRefId5", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.LessThan, "error!");
				CompareValidator validator6 = new CompareValidator("myid", "refid6", "compareRefId6", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				CompareValidator validator7 = new CompareValidator("myid", "refid7", "compareRefId7", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				CompareValidator validator8 = new CompareValidator("myid", "refid8", "compareRefId8", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.NotEqual, "error!");
				CompareValidator validator9 = new CompareValidator("myid", "refid9", "compareRefId9", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.DataTypeCheck, "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid1", "1234.12");
				formValues.Add("compareRefId1", "1234.12");
				formValues.Add("refid2", "2345.12");
				formValues.Add("compareRefId2", "1234.12");
				formValues.Add("refid3", "2345.12");
				formValues.Add("compareRefId3", "1234.12");
				formValues.Add("refid4", "1234.12");
				formValues.Add("compareRefId4", "1234.12");
				formValues.Add("refid5", "123.12");
				formValues.Add("compareRefId5", "1234.12");
				formValues.Add("refid6", "123.12");
				formValues.Add("compareRefId6", "1234.12");
				formValues.Add("refid7", "1234.12");
				formValues.Add("compareRefId7", "1234.12");
				formValues.Add("refid8", "1234.12");
				formValues.Add("compareRefId8", "2345.12");
				formValues.Add("refid9", "1234.12");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator1.Validate(request));
					Assert.IsTrue(validator1.IsValid);
					Assert.IsTrue(validator2.Validate(request));
					Assert.IsTrue(validator2.IsValid);
					Assert.IsTrue(validator3.Validate(request));
					Assert.IsTrue(validator3.IsValid);
					Assert.IsTrue(validator4.Validate(request));
					Assert.IsTrue(validator4.IsValid);
					Assert.IsTrue(validator5.Validate(request));
					Assert.IsTrue(validator5.IsValid);
					Assert.IsTrue(validator6.Validate(request));
					Assert.IsTrue(validator6.IsValid);
					Assert.IsTrue(validator7.Validate(request));
					Assert.IsTrue(validator7.IsValid);
				}
			}

			[Test]
			public void Validation_On_Double_Value_Present()
			{
				CompareValidator validator1 = new CompareValidator("myid", "refid1", "compareRefId1", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				CompareValidator validator2 = new CompareValidator("myid", "refid2", "compareRefId2", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThan, "error!");
				CompareValidator validator3 = new CompareValidator("myid", "refid3", "compareRefId3", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				CompareValidator validator4 = new CompareValidator("myid", "refid4", "compareRefId4", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				CompareValidator validator5 = new CompareValidator("myid", "refid5", "compareRefId5", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.LessThan, "error!");
				CompareValidator validator6 = new CompareValidator("myid", "refid6", "compareRefId6", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				CompareValidator validator7 = new CompareValidator("myid", "refid7", "compareRefId7", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				CompareValidator validator8 = new CompareValidator("myid", "refid8", "compareRefId8", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.NotEqual, "error!");
				CompareValidator validator9 = new CompareValidator("myid", "refid9", "compareRefId9", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.DataTypeCheck, "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid1", "1234.12");
				formValues.Add("compareRefId1", "1234.12");
				formValues.Add("refid2", "2345.12");
				formValues.Add("compareRefId2", "1234.12");
				formValues.Add("refid3", "2345.12");
				formValues.Add("compareRefId3", "1234.12");
				formValues.Add("refid4", "1234.12");
				formValues.Add("compareRefId4", "1234.12");
				formValues.Add("refid5", "123.12");
				formValues.Add("compareRefId5", "1234.12");
				formValues.Add("refid6", "123.12");
				formValues.Add("compareRefId6", "1234.12");
				formValues.Add("refid7", "1234.12");
				formValues.Add("compareRefId7", "1234.12");
				formValues.Add("refid8", "1234.12");
				formValues.Add("compareRefId8", "2345.12");
				formValues.Add("refid9", "1234.12");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator1.Validate(request));
					Assert.IsTrue(validator1.IsValid);
					Assert.IsTrue(validator2.Validate(request));
					Assert.IsTrue(validator2.IsValid);
					Assert.IsTrue(validator3.Validate(request));
					Assert.IsTrue(validator3.IsValid);
					Assert.IsTrue(validator4.Validate(request));
					Assert.IsTrue(validator4.IsValid);
					Assert.IsTrue(validator5.Validate(request));
					Assert.IsTrue(validator5.IsValid);
					Assert.IsTrue(validator6.Validate(request));
					Assert.IsTrue(validator6.IsValid);
					Assert.IsTrue(validator7.Validate(request));
					Assert.IsTrue(validator7.IsValid);
				}
			}

			[Test]
			public void Validation_On_Integer_Value_Present()
			{
				CompareValidator validator1 = new CompareValidator("myid", "refid1", "compareRefId1", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				CompareValidator validator2 = new CompareValidator("myid", "refid2", "compareRefId2", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThan, "error!");
				CompareValidator validator3 = new CompareValidator("myid", "refid3", "compareRefId3", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				CompareValidator validator4 = new CompareValidator("myid", "refid4", "compareRefId4", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				CompareValidator validator5 = new CompareValidator("myid", "refid5", "compareRefId5", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.LessThan, "error!");
				CompareValidator validator6 = new CompareValidator("myid", "refid6", "compareRefId6", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				CompareValidator validator7 = new CompareValidator("myid", "refid7", "compareRefId7", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				CompareValidator validator8 = new CompareValidator("myid", "refid8", "compareRefId8", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.NotEqual, "error!");
				CompareValidator validator9 = new CompareValidator("myid", "refid9", "compareRefId9", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.DataTypeCheck, "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid1", "1234");
				formValues.Add("compareRefId1", "1234");
				formValues.Add("refid2", "2345");
				formValues.Add("compareRefId2", "1234");
				formValues.Add("refid3", "2345");
				formValues.Add("compareRefId3", "1234");
				formValues.Add("refid4", "1234");
				formValues.Add("compareRefId4", "1234");
				formValues.Add("refid5", "123");
				formValues.Add("compareRefId5", "1234");
				formValues.Add("refid6", "123");
				formValues.Add("compareRefId6", "1234");
				formValues.Add("refid7", "1234");
				formValues.Add("compareRefId7", "1234");
				formValues.Add("refid8", "1234");
				formValues.Add("compareRefId8", "2345");
				formValues.Add("refid9", "1234");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator1.Validate(request));
					Assert.IsTrue(validator1.IsValid);
					Assert.IsTrue(validator2.Validate(request));
					Assert.IsTrue(validator2.IsValid);
					Assert.IsTrue(validator3.Validate(request));
					Assert.IsTrue(validator3.IsValid);
					Assert.IsTrue(validator4.Validate(request));
					Assert.IsTrue(validator4.IsValid);
					Assert.IsTrue(validator5.Validate(request));
					Assert.IsTrue(validator5.IsValid);
					Assert.IsTrue(validator6.Validate(request));
					Assert.IsTrue(validator6.IsValid);
					Assert.IsTrue(validator7.Validate(request));
					Assert.IsTrue(validator7.IsValid);
				}
			}

			[Test]
			public void Validation_On_String_Value_Present()
			{
				CompareValidator validator1 = new CompareValidator("myid", "refid1", "compareRefId1", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				CompareValidator validator2 = new CompareValidator("myid", "refid2", "compareRefId2", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThan, "error!");
				CompareValidator validator3 = new CompareValidator("myid", "refid3", "compareRefId3", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				CompareValidator validator4 = new CompareValidator("myid", "refid4", "compareRefId4", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				CompareValidator validator5 = new CompareValidator("myid", "refid5", "compareRefId5", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.LessThan, "error!");
				CompareValidator validator6 = new CompareValidator("myid", "refid6", "compareRefId6", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				CompareValidator validator7 = new CompareValidator("myid", "refid7", "compareRefId7", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				CompareValidator validator8 = new CompareValidator("myid", "refid8", "compareRefId8", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.NotEqual, "error!");
				CompareValidator validator9 = new CompareValidator("myid", "refid9", "compareRefId9", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.DataTypeCheck, "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid1", "c");
				formValues.Add("compareRefId1", "c");
				formValues.Add("refid2", "d");
				formValues.Add("compareRefId2", "c");
				formValues.Add("refid3", "d");
				formValues.Add("compareRefId3", "c");
				formValues.Add("refid4", "c");
				formValues.Add("compareRefId4", "c");
				formValues.Add("refid5", "b");
				formValues.Add("compareRefId5", "c");
				formValues.Add("refid6", "b");
				formValues.Add("compareRefId6", "c");
				formValues.Add("refid7", "c");
				formValues.Add("compareRefId7", "c");
				formValues.Add("refid8", "b");
				formValues.Add("compareRefId8", "c");
				formValues.Add("refid9", "c");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator1.Validate(request));
					Assert.IsTrue(validator1.IsValid);
					Assert.IsTrue(validator2.Validate(request));
					Assert.IsTrue(validator2.IsValid);
					Assert.IsTrue(validator3.Validate(request));
					Assert.IsTrue(validator3.IsValid);
					Assert.IsTrue(validator4.Validate(request));
					Assert.IsTrue(validator4.IsValid);
					Assert.IsTrue(validator5.Validate(request));
					Assert.IsTrue(validator5.IsValid);
					Assert.IsTrue(validator6.Validate(request));
					Assert.IsTrue(validator6.IsValid);
					Assert.IsTrue(validator7.Validate(request));
					Assert.IsTrue(validator7.IsValid);
				}
			}

			[Test]
			public void Validation_InvalidType()
			{
				CompareValidator validator1 = new CompareValidator("myid", "refid1", "compareRefId1", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				CompareValidator validator2 = new CompareValidator("myid", "refid2", "compareRefId2", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid1", "abcd");
				formValues.Add("compareRefId1", "1234.12");
				formValues.Add("refid1", "1234.12");
				formValues.Add("compareRefId1", "abcd");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator1.Validate(request));
					Assert.IsFalse(validator2.Validate(request));
				}
			}

			[Test]
			public void Validation_On_No_Value_Present()
			{
				CompareValidator validator1 = new CompareValidator("myid", "refid1", "compareRefId1", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				CompareValidator validator2 = new CompareValidator("myid", "refid2", "compareRefId2", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("compareRefId1", "1234.12");
				formValues.Add("refid2", "1234.12");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator1.Validate(request));
					Assert.IsFalse(validator1.IsValid);
					Assert.IsFalse(validator2.Validate(request));
					Assert.IsFalse(validator2.IsValid);
				}
			}

			[Test]
			public void Validation_On_No_Form_Value_Present()
			{
				CompareValidator validator1 = new CompareValidator("myid", "refid1", "compareRefId1", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				CompareValidator validator2 = new CompareValidator("myid", "refid2", "compareRefId2", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");

				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;

				using (mocks.Record())
				{
					request = mocks.DynamicHttpRequestBase();
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator1.Validate(request));
					Assert.IsFalse(validator1.IsValid);
					Assert.IsFalse(validator2.Validate(request));
					Assert.IsFalse(validator2.IsValid);
				}
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

			[Test]
			public void Validation_On_String_Type()
			{
				RangeValidator validator = new RangeValidator("myid", "refid", "a", "c", System.Web.UI.WebControls.ValidationDataType.String, "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid", "b");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator.Validate(request));
					Assert.IsTrue(validator.IsValid);
				}
			}

			[Test]
			public void Validation_On_Double_Type()
			{
				RangeValidator validator = new RangeValidator("myid", "refid", "1234.12", "2345.12", System.Web.UI.WebControls.ValidationDataType.Double, "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid", "2000.00");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator.Validate(request));
					Assert.IsTrue(validator.IsValid);
				}
			}

			[Test]
			public void Validation_On_Integer_Type()
			{
				RangeValidator validator = new RangeValidator("myid", "refid", "1", "10", System.Web.UI.WebControls.ValidationDataType.Integer, "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid", "5");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator.Validate(request));
					Assert.IsTrue(validator.IsValid);
				}
			}

			[Test]
			public void Validation_InvalidType()
			{
				RangeValidator validator = new RangeValidator("myid", "refid1", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid1", "abcd");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator.Validate(request));
					Assert.IsFalse(validator.IsValid);
				}
			}

			[Test]
			public void Validation_On_No_Value_Present()
			{
				RangeValidator validator = new RangeValidator("myid", "refid1", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "error!");
				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;
				NameValueCollection formValues = new NameValueCollection();
				formValues.Add("refid1", "");

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator.Validate(request));
					Assert.IsFalse(validator.IsValid);
				}
			}

			[Test]
			public void Validation_On_No_Form_Value_Present()
			{
				RangeValidator validator = new RangeValidator("myid", "refid1", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "error!");

				MockRepository mocks = new MockRepository();
				HttpRequestBase request = null;

				using (mocks.Record())
				{
					request = mocks.DynamicHttpRequestBase();
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator.Validate(request));
					Assert.IsFalse(validator.IsValid);
				}
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
