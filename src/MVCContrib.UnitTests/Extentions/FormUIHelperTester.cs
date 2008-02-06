using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Web.Mvc;
using MvcContrib.Extentions;
using Rhino.Mocks;

namespace MVCContrib.UnitTests.Extentions
{
    [TestFixture, Category("HtmlHelper Extentions")]
    public class FormUIHelperTester
    {
        private MockRepository _mocks;
        private HtmlHelper _htmlHelper;

        [SetUp]
        public void SetUp()
        {
            _mocks = new MockRepository();
            IHttpContext httpContext = _mocks.DynamicMock<IHttpContext>();
            IHttpResponse httpResponse = _mocks.DynamicMock<IHttpResponse>();
            IHttpSessionState httpSessionState = _mocks.DynamicMock<IHttpSessionState>();
            SetupResult.For(httpContext.Session).Return(httpSessionState);
            SetupResult.For(httpContext.Response).Return(httpResponse);
            RequestContext requestContext = new RequestContext(httpContext, new RouteData());
            IController controller = _mocks.DynamicMock<IController>();
            ControllerContext controllerContext = new ControllerContext(requestContext, controller);
            _mocks.ReplayAll();
            ViewContext viewContext = new ViewContext(controllerContext, new Hashtable(), new TempDataDictionary(controllerContext.HttpContext));

            _htmlHelper = new HtmlHelper(viewContext);
        }
        [Test]
        public void Make_Input_TextBox()
        {
            string testresult = @"<input type=""text"" id=""theID"" name=""theID"" value=""new value"" />";
            string test = _htmlHelper.FormText("theID", "new value");
            Assert.AreEqual(testresult, test);
        }
        [Test]
        public void Make_Input_Password()
        {
            string testresult = @"<input type=""password"" id=""theID"" name=""theID"" value=""new value"" />";
            string test = _htmlHelper.FormPassword("theID", "new value");
            Assert.AreEqual(testresult, test);
        }
        [Test]
        public void Make_Input_File()
        {
            string testresult = @"<input type=""file"" id=""theID"" name=""theID"" />";
            string test = _htmlHelper.FormFile("theID");
            Assert.AreEqual(testresult, test);
        }
        [Test]
        public void Make_Input_CheckBox()
        {
            string testresult = @"<input type=""checkbox"" id=""theID"" name=""theID"" value=""new value"" checked=""checked"" />";
            string test = _htmlHelper.FormCheckBox("theID", "new value", true);
            Assert.AreEqual(testresult, test);
        }
        [Test]
        public void Make_Input_Radio()
        {
            string testresult = @"<input type=""radio"" id=""theID"" name=""theID"" value=""new value"" checked=""checked"" />";
            string test = _htmlHelper.FormRadio("theID", "new value", true);
            Assert.AreEqual(testresult, test);
        }
        [Test]
        public void Make_Select()
        {
            //ToDo: Write test;
        }
        [Test]
        public void Make_TextArea()
        {
            //string testresult = @"<textarea id=""theID"" name=""theID"">The Value</textarea>";
        }
        [Test]
        public void Make_Input_Buttons()
        {
            //ToDo: Write test;
        }
    }
}