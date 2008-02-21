using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using MvcContrib.TestHelper.Sample.Controllers;
using MvcContrib.TestHelper.Sample.Models;
using MvcContrib.TestHelper;

//This class is to demo the use of the framework
//by testing the controller located in MvcTestingFramework.Sample.
//The actual unit tests for the framework are located in MvcTestingFramework.Test

namespace MvcContrib.TestHelper.Sample
{
    [TestFixture]
    public class StarsControllerTest
    {
        [Test]
        public void ListControllerSelectsListView()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            StarsController controller = builder.CreateController<StarsController>();
            controller.List();
            Assert.AreEqual("List", builder.RenderViewData.ViewName);
        }

        [Test]
        public void AddFormStarShouldRedirectToList()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            StarsController controller = builder.CreateController<StarsController>();
            controller.AddFormStar();
            Assert.AreEqual("List", builder.RedirectToActionData.ActionName);
        }

        [Test]
        public void AddFormStarShouldSaveFormToTempData()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            StarsController controller = builder.CreateController<StarsController>();
            builder.Form["NewStarName"] = "alpha c";
            controller.AddFormStar();
            Assert.AreEqual("alpha c", controller.TempData["NewStarName"]);
        }

        [Test]
        public void AddSessionStarShouldSaveFormToSession()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            StarsController controller = builder.CreateController<StarsController>();
            builder.Form["NewStarName"] = "alpha c";
            controller.AddSessionStar();
            Assert.AreEqual("alpha c", controller.HttpContext.Session["NewStarName"]);
        }

        [Test]
        public void RouteTableShouldSelectProperRoute()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            MvcTestingFramework.Sample.Global.SetupRoutes(); //setup the routes the same as our app does
            StarsController controller = builder.CreateController<StarsController>();
            controller.ListWithLinks();
            List<Star> stars = (List<Star>)builder.RenderViewData.ViewData;
            Assert.AreEqual("/Stars/Nearby/1", stars[0].NearbyLink);
        }

        [Test]
        public void NearbyShouldRedirectToListWithLinks() //ok, really it should do something more useful, but you get the point
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            StarsController controller = builder.CreateController<StarsController>();
            controller.Nearby();
            Assert.AreEqual("ListWithLinks", builder.RedirectToActionData.ActionName);
        }
    }
}
