using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using MvcContrib.TestHelper;


//Note: these tests confirm that the TestControllerBuilder works properly
//for examples on how to use the TestControllerBuilder and other TestHelper classes,
//look in the \src\Samples\MvcContrib.TestHelper solution
namespace MvcContrib.TestHelper.Test
{
    [TestFixture]
    public class ControllerBuilderTests
    {
        [Test]
        public void CanCreateSpecificController()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            object c = builder.CreateController<TestHelperController>();
            Assert.IsInstanceOfType(typeof(TestHelperController), c);
        }

        [Test]
        public void CanSpecifyFormVariables()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            builder.Form["Variable"] = "Value";
            Controller c = builder.CreateController<TestHelperController>();
            Assert.AreEqual("Value", c.HttpContext.Request.Form["Variable"]);
        }

        [Test]
        public void CanSpecifyRouteData()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            RouteData rd = new RouteData();
            rd.Values["Variable"] = "Value";
            builder.RouteData = rd;

            TestHelperController testController = builder.CreateController<TestHelperController>();
            Assert.AreEqual("Value", testController.RouteData.Values["Variable"]);
        }

        [Test]
        public void CanSpecifyQueryString()
        {
            TestControllerBuilder handler = new TestControllerBuilder();
            handler.QueryString["Variable"] = "Value";
            TestHelperController testController = handler.CreateController<TestHelperController>();
            Assert.AreEqual("Value", testController.Request.QueryString["Variable"]);
        }

        [Test]
        public void RedirectRecordsData()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            TestHelperController testController = builder.CreateController<TestHelperController>();
            testController.RedirectWithAction();
            Assert.AreEqual("ActionName1", builder.RedirectToActionData.ActionName);
            testController.RedirectWithActionAndController();
            Assert.AreEqual("ActionName2", builder.RedirectToActionData.ActionName);
            Assert.AreEqual("ControllerName2", builder.RedirectToActionData.ControllerName);
            testController.RedirectWithObject();
            Assert.AreEqual("ActionName3", builder.RedirectToActionData.ActionName);
            Assert.AreEqual("ControllerName3", builder.RedirectToActionData.ControllerName);
        }

        [Test]
        public void RenderViewRecordsData()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            TestHelperController testController = builder.CreateController<TestHelperController>();
            testController.RenderViewWithViewName();
            Assert.AreEqual("View1", builder.RenderViewData.ViewName);
            testController.RenderViewWithViewNameAndData();
            Assert.AreEqual("View2", builder.RenderViewData.ViewName);
            Assert.AreEqual(new { Prop1 = 1, Prop2 = 2 }, builder.RenderViewData.ViewData);
            testController.RenderViewWithViewNameAndMaster();
            Assert.AreEqual("View3", builder.RenderViewData.ViewName);
            Assert.AreEqual("Master3", builder.RenderViewData.MasterName);
            testController.RenderViewWithViewNameAndMasterAndData();
            Assert.AreEqual("View4", builder.RenderViewData.ViewName);
            Assert.AreEqual("Master4", builder.RenderViewData.MasterName);
            Assert.AreEqual(new { Prop1 = 3, Prop2 = 4 }, builder.RenderViewData.ViewData);
        }

        [Test]
        public void CanCreateMultipleControllers()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            TestHelperController testController = builder.CreateController<TestHelperController>();
            TestHelperController testController2 = builder.CreateController<TestHelperController>();
            Assert.AreNotEqual(testController, testController2);
        }

        [Test]
        public void InterceptorShouldIgnoreNormalMethods()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            TestHelperController testController = builder.CreateController<TestHelperController>();
            int x = testController.RandomOtherFunction();
            Assert.AreEqual(12345, x);
        }
    }
}
