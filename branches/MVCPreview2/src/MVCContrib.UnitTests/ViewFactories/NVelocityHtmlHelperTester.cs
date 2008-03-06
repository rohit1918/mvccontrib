using System.Collections;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Castle;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ViewFactories
{
	[TestFixture, Category("NVelocityViewEngine")]
	public class NVelocityHtmlHelperTester
	{
		private MockRepository _mocks;
		private NVelocityHtmlHelper _htmlHelper;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			HttpContextBase httpContext = _mocks.DynamicMock<HttpContextBase>();
			HttpResponseBase httpResponse = _mocks.DynamicMock<HttpResponseBase>();
			HttpSessionStateBase httpSessionState = _mocks.DynamicMock<HttpSessionStateBase>();
			HttpServerUtilityBase httpServer = _mocks.DynamicMock<HttpServerUtilityBase>();
			SetupResult.For(httpContext.Session).Return(httpSessionState);
			SetupResult.For(httpContext.Response).Return(httpResponse);
			SetupResult.For(httpContext.Server).Return(httpServer);
			SetupResult.For(httpServer.HtmlEncode(null)).IgnoreArguments().Return(string.Empty);
			RequestContext requestContext = new RequestContext(httpContext, new RouteData());
			IController controller = _mocks.DynamicMock<IController>();
			ControllerContext controllerContext = new ControllerContext(requestContext, controller);
			_mocks.ReplayAll();
			ViewContext viewContext =
				new ViewContext(controllerContext, new Hashtable(), new TempDataDictionary(controllerContext.HttpContext));

			_htmlHelper = new NVelocityHtmlHelper(viewContext);
		}

		[Test]
		public void Submit_Defaults_To_No_HtmlName()
		{
			Assert.AreEqual(_htmlHelper.SubmitButton(string.Empty, "test"), _htmlHelper.Submit("test"));
		}

		[Test]
		public void Submit_Uses_Id_For_HtmlName()
		{
			Hashtable htmlAttributes = new Hashtable();
			htmlAttributes["id"] = "id";

			Assert.AreEqual(_htmlHelper.SubmitButton("id", "test"), _htmlHelper.Submit("test", htmlAttributes));
		}

		[Test]
		public void TextBox_Passes_Through_Attributes()
		{
			Hashtable htmlAttributes = new Hashtable();
			htmlAttributes["attr"] = "value";

			Assert.AreEqual(_htmlHelper.TextBox("htmlName", string.Empty, new { attr = "value" }), _htmlHelper.TextBox("htmlName", htmlAttributes));
		}

		[Test]
		public void TextBox_Passes_Through_Attributes_With_Value()
		{
			Hashtable htmlAttributes = new Hashtable();
			htmlAttributes["attr"] = "value";

			Assert.AreEqual(_htmlHelper.TextBox("htmlName", "value", new { attr = "value" }), _htmlHelper.TextBox("htmlName", "value", htmlAttributes));
		}

		[Test]
		public void Mailto_Uses_Subject_Body_Cc_And_Bcc_Attributes()
		{
			Hashtable htmlAttributes = new Hashtable();
			htmlAttributes["subject"] = "subject";
			htmlAttributes["body"] = "body";
			htmlAttributes["cc"] = "cc";

			Assert.AreEqual(_htmlHelper.MailTo("emailAddress", "linkText", "subject", string.Empty, "cc", "body"), _htmlHelper.MailTo("emailAddress", "linkText", htmlAttributes));
		}
	}
}
