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
			var httpContext = _mocks.DynamicMock<HttpContextBase>();
			var httpResponse = _mocks.DynamicMock<HttpResponseBase>();
			var httpSessionState = _mocks.DynamicMock<HttpSessionStateBase>();
			var httpServer = _mocks.DynamicMock<HttpServerUtilityBase>();
			SetupResult.For(httpContext.Session).Return(httpSessionState);
			SetupResult.For(httpContext.Response).Return(httpResponse);
			SetupResult.For(httpContext.Server).Return(httpServer);
			SetupResult.For(httpServer.HtmlEncode(null)).IgnoreArguments().Return(string.Empty);
			var requestContext = new RequestContext(httpContext, new RouteData());
			var controller = _mocks.DynamicMock<ControllerBase>();
			var controllerContext = new ControllerContext(requestContext, controller);
			_mocks.ReplayAll();
			var viewContext =
				new ViewContext(controllerContext, "index", new ViewDataDictionary(), new TempDataDictionary());

			_htmlHelper = new NVelocityHtmlHelper(viewContext, new ViewPage());
		}

		[Test,Ignore]
		public void Submit_Defaults_To_No_HtmlName()
		{
//			Assert.AreEqual(_htmlHelper.SubmitButton(string.Empty, "test"), _htmlHelper.Submit("test"));
		}

		[Test,Ignore]
		public void Submit_Uses_Id_For_HtmlName()
		{
			var htmlAttributes = new Hashtable();
			htmlAttributes["id"] = "id";

			//Assert.AreEqual(_htmlHelper.SubmitButton("id", "test"), _htmlHelper.Submit("test", htmlAttributes));
		}

		[Test]
		public void TextBox_Passes_Through_Attributes()
		{
			var htmlAttributes = new Hashtable();
			htmlAttributes["attr"] = "value";

			Assert.AreEqual(_htmlHelper.TextBox("htmlName", string.Empty, new { attr = "value" }), _htmlHelper.TextBox("htmlName", htmlAttributes));
		}

		[Test]
		public void TextBox_Passes_Through_Attributes_With_Value()
		{
			var htmlAttributes = new Hashtable();
			htmlAttributes["attr"] = "value";

			Assert.AreEqual(_htmlHelper.TextBox("htmlName", "value", new { attr = "value" }), _htmlHelper.TextBox("htmlName", "value", htmlAttributes));
		}

		[Test,Ignore]
		public void Mailto_Uses_Subject_Body_Cc_And_Bcc_Attributes()
		{
			var htmlAttributes = new Hashtable();
			htmlAttributes["subject"] = "subject";
			htmlAttributes["body"] = "body";
			htmlAttributes["cc"] = "cc";

            //string expected = _htmlHelper.Mailto("emailAddress", "linkText", "subject", "body", "cc", string.Empty, null);
            //string actual = _htmlHelper.Mailto("emailAddress", "linkText", htmlAttributes);

            //Assert.AreEqual(expected, actual);
		}
	}
}
