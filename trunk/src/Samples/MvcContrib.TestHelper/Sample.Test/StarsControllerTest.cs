using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
    	private StarsController _controller;
    	private TestControllerBuilder _builder;

    	[SetUp]
    	public void Setup()
    	{
    		_controller = new StarsController();	
			_builder = new TestControllerBuilder();
    		_builder.InitializeController(_controller);
		}

        [Test]
        public void ListControllerSelectsListView()
        {
            _builder.InitializeController(_controller);
            ActionResult result = _controller.List();
            Assert.AreEqual("List", ((RenderViewResult)result).ViewName);
        }

        [Test]
        public void AddFormStarShouldRedirectToList()
        {
            var result = _controller.AddFormStar() as ActionRedirectResult;
            Assert.AreEqual("List", result.Values["action"]);
        }

        [Test]
        public void AddFormStarShouldSaveFormToTempData()
        {
            _builder.Form["NewStarName"] = "alpha c";
            _controller.AddFormStar();
            Assert.AreEqual("alpha c", _controller.TempData["NewStarName"]);
        }

        [Test]
        public void AddSessionStarShouldSaveFormToSession()
        {
            _builder.Form["NewStarName"] = "alpha c";
            _controller.AddSessionStar();
            Assert.AreEqual("alpha c", _controller.HttpContext.Session["NewStarName"]);
        }

        [Test]
        public void NearbyShouldRedirectToListWithLinks() //ok, really it should do something more useful, but you get the point
        {
            var result = _controller.Nearby() as ActionRedirectResult;
            Assert.AreEqual("ListWithLinks", result.Values["action"]);
        }
    }
}
