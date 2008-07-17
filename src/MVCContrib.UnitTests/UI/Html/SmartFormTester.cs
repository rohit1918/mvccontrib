using System;
using System.Text;
using System.Web;
using System.Web.Routing;
using MvcContrib.UI.Html;
using MvcContrib.UI.Tags;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using System.Web.Mvc;
using System.Collections;

namespace MvcContrib.UnitTests.UI.Html
{
	[TestFixture]
	public class SmartFormTester
	{
		public class BaseSmartFormTester
		{
			protected IFormHelper _helper;
			protected ViewContext _context;
			protected MockRepository _mocks;

			[SetUp]
			public virtual void Setup()
			{
				_mocks = new MockRepository();
				HttpContextBase httpContext = _mocks.DynamicMock<HttpContextBase>();
				HttpResponseBase response = _mocks.DynamicMock<HttpResponseBase>();
				HttpSessionStateBase session = _mocks.DynamicMock<HttpSessionStateBase>();
				IController controller = _mocks.DynamicMock<IController>();

				SetupResult.For(httpContext.Response).Return(response);
				SetupResult.For(httpContext.Session).Return(session);
				SetupResult.For(response.Filter).PropertyBehavior();
				SetupResult.For(response.ContentEncoding).Return(Encoding.UTF8);

				_mocks.ReplayAll();
				_context = new ViewContext(httpContext,new RouteData(), controller, "index", "", new ViewDataDictionary(), new TempDataDictionary());
				_helper = _mocks.DynamicMock<IFormHelper>();
				SetupResult.For(_helper.ViewContext).Return(_context);
				_mocks.Replay(_helper);
			}
		}

		[TestFixture]
		public class When_SmartForm_Is_Instantiated : BaseSmartFormTester
		{
			[Test]
			public void Then_Item_should_match_ctor_argument()
			{
				object obj = new object();
				 
				SmartForm<object> form = new SmartForm<object>("test", delegate { }, _helper, obj, Hash.Empty);
				Assert.That(form.Item, Is.SameAs(obj));
			}

			[Test]
			public void Then_Method_should_default_to_post()
			{
				SmartForm<object> form = new SmartForm<object>("test", delegate { }, _helper, new object(), Hash.Empty);
				Assert.That(form.Method, Is.EqualTo(MvcContrib.UI.Tags.Form.FORM_METHOD.POST));
			}

			[Test]
			public void Then_Action_should_match_ctor_argument()
			{
				SmartForm<object> form = new SmartForm<object>("test", delegate { }, _helper, new object(), Hash.Empty);
				Assert.That(form.Action, Is.EqualTo("test"));
			}

			[Test]
			public void Then_FormHelper_should_match_ctor_argument()
			{
				SmartForm<object> form = new SmartForm<object>("test", delegate  { }, _helper, new object(), Hash.Empty);
				Assert.That(form.FormHelper, Is.SameAs(_helper));
			}
		}

		[TestFixture]
		public class When_ToString_is_called : BaseSmartFormTester
		{

			private void FakeWrite(string text)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(text);
				_context.HttpContext.Response.Filter.Write(bytes, 0, bytes.Length);
			}

			[Test]
			public void Then_Inner_contents_should_be_rendered_correctly()
			{
				SmartForm<object> form = new SmartForm<object>("test", f => FakeWrite("Test"), _helper, new object(),Hash.Empty);
				string expected = "<form method=\"post\" action=\"test\">Test</form>";
				Assert.That(form.ToString(), Is.EqualTo(expected));
			}
		}

		[TestFixture]
		public class When_Form_methods_are_called
		{
			private MockRepository _mocks;
			private IFormHelper _helper;
			private SmartForm<object> _form;

			[SetUp]
			public void Setup()
			{
				_mocks = new MockRepository();
				_helper = _mocks.DynamicMock<IFormHelper>();
				_form = new SmartForm<object>("test", delegate { }, _helper, new object(), Hash.Empty);
			}

			[Test]
			public void Then_they_should_be_delegated_to_the_underlying_FormHelper()
			{
				TextBox tb = new TextBox();
				IDictionary hash = Hash.Empty;
				Password pw = new Password();
				HiddenField hidden = new HiddenField();
				CheckBoxField checkbox = new CheckBoxField();
				TextArea textArea = new TextArea();
				SubmitButton submit = new SubmitButton(); 
				object fakeDataItem = new object();
				Select select = new Select();
				RadioField radio = new RadioField();

				using(_mocks.Record())
				{
					IDataBinder binder = _mocks.DynamicMock<IDataBinder>();
					SetupResult.For(binder.NestedBindingScope(null)).IgnoreArguments().Return(_mocks.DynamicMock<IDisposable>());
					SetupResult.For(_helper.Binder).Return(binder);

					Expect.Call(_helper.TextField("Name")).Return(null);
					Expect.Call(_helper.TextField(tb)).Return(null);
					Expect.Call(_helper.TextField("Name", hash)).Return(null);

					Expect.Call(_helper.PasswordField(pw)).Return(null);
					Expect.Call(_helper.PasswordField("Name", hash)).Return(null);
					Expect.Call(_helper.PasswordField("Name")).Return(null);

					Expect.Call(_helper.HiddenField("Name")).Return(null);
					Expect.Call(_helper.HiddenField("Name", hash)).Return(null);
					Expect.Call(_helper.HiddenField(hidden)).Return(null);

					Expect.Call(_helper.CheckBoxField("IsDeveloper")).Return(null);
					Expect.Call(_helper.CheckBoxField("IsDeveloper", hash)).Return(null);
					Expect.Call(_helper.CheckBoxField(checkbox)).Return(null);

					Expect.Call(_helper.TextArea("Name")).Return(null);
					Expect.Call(_helper.TextArea("Name", hash)).Return(null);
					Expect.Call(_helper.TextArea(textArea)).Return(null);

					Expect.Call(_helper.Submit()).Return(null);
					Expect.Call(_helper.Submit("Submit")).Return(null);
					Expect.Call(_helper.Submit("Submit", hash)).Return(null);
					Expect.Call(_helper.Submit(submit)).Return(null);

					Expect.Call(_helper.Select(fakeDataItem, select)).Return(null);
					Expect.Call(_helper.Select("Role", fakeDataItem, "Name", "Id")).Return(null);
					Expect.Call(_helper.Select("Role", fakeDataItem, "Name", "Id", hash)).Return(null);

					Expect.Call(_helper.RadioField("Role", null)).Return(null);
					Expect.Call(_helper.RadioField("Role", null, hash)).Return(null);
					Expect.Call(_helper.RadioField(radio)).Return(null);

				}
				using(_mocks.Playback())
				{
					_form.TextField("Name");
					_form.TextField(tb);
					_form.TextField("Name", hash);

					_form.PasswordField(pw);
					_form.PasswordField("Name", hash);
					_form.PasswordField("Name");

					_form.HiddenField("Name");
					_form.HiddenField("Name", hash);
					_form.HiddenField(hidden);

					_form.CheckBoxField("IsDeveloper");
					_form.CheckBoxField("IsDeveloper", hash);
					_form.CheckBoxField(checkbox);

					_form.TextArea("Name");
					_form.TextArea("Name", hash);
					_form.TextArea(textArea);

					_form.Submit();
					_form.Submit("Submit");
					_form.Submit("Submit", hash);
					_form.Submit(submit);

					_form.Select(fakeDataItem, select);
					_form.Select("Role", fakeDataItem, "Name", "Id");
					_form.Select("Role", fakeDataItem, "Name", "Id", hash);

					_form.RadioField("Role", null);
					_form.RadioField("Role", null, hash);
					_form.RadioField(radio);
				}
			}

			[Test]
			public void With_viewdata_key_then_they_Should_be_delegated_to_the_underlying_formhelper()
			{
				_form = new SmartForm<object>("person", "test", delegate { }, _helper, new object(), Hash.Empty);
				IDictionary hash = Hash.Empty;
				object fakeDataItem = new object();

				using (_mocks.Record())
				{
					IDataBinder binder = _mocks.DynamicMock<IDataBinder>();
					SetupResult.For(binder.NestedBindingScope(null)).IgnoreArguments().Return(_mocks.DynamicMock<IDisposable>());
					SetupResult.For(_helper.Binder).Return(binder);

					Expect.Call(_helper.TextField("person.Name")).Return(null);
					Expect.Call(_helper.TextField("person.Name", hash)).Return(null);

					Expect.Call(_helper.PasswordField("person.Name", hash)).Return(null);
					Expect.Call(_helper.PasswordField("person.Name")).Return(null);

					Expect.Call(_helper.HiddenField("person.Name")).Return(null);
					Expect.Call(_helper.HiddenField("person.Name", hash)).Return(null);

					Expect.Call(_helper.CheckBoxField("person.IsDeveloper")).Return(null);
					Expect.Call(_helper.CheckBoxField("person.IsDeveloper", hash)).Return(null);

					Expect.Call(_helper.TextArea("person.Name")).Return(null);
					Expect.Call(_helper.TextArea("person.Name", hash)).Return(null);

					Expect.Call(_helper.Submit()).Return(null);
					Expect.Call(_helper.Submit("Submit")).Return(null);
					Expect.Call(_helper.Submit("Submit", hash)).Return(null);

					Expect.Call(_helper.Select("person.Role", fakeDataItem, "Name", "Id")).Return(null);
					Expect.Call(_helper.Select("person.Role", fakeDataItem, "Name", "Id", hash)).Return(null);

					Expect.Call(_helper.RadioField("person.Role", null)).Return(null);
					Expect.Call(_helper.RadioField("person.Role", null, hash)).Return(null);

				}
				using (_mocks.Playback())
				{
					_form.TextField("Name");
					_form.TextField("Name", hash);

					_form.PasswordField("Name", hash);
					_form.PasswordField("Name");

					_form.HiddenField("Name");
					_form.HiddenField("Name", hash);

					_form.CheckBoxField("IsDeveloper");
					_form.CheckBoxField("IsDeveloper", hash);

					_form.TextArea("Name");
					_form.TextArea("Name", hash);

					_form.Submit();
					_form.Submit("Submit");
					_form.Submit("Submit", hash);

					_form.Select("Role", fakeDataItem, "Name", "Id");
					_form.Select("Role", fakeDataItem, "Name", "Id", hash);

					_form.RadioField("Role", null);
					_form.RadioField("Role", null, hash);
				}
			}
		}
	}
}
