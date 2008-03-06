using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using MvcContrib.TestHelper;

namespace MvcContrib.TestHelper
{
    public class TestController : Controller
    {
        
        public void RedirectWithAction()
        {
            RedirectToAction("ActionName1");
        }

        
        public void RedirectWithActionAndController()
        {
            RedirectToAction("ActionName2", "ControllerName2");
        }

        
        public void RedirectWithObject()
        {
            RedirectToAction(new { Action = "ActionName3", Controller = "ControllerName3" });
        }

        
        public void RenderViewWithViewName()
        {
            RenderView("View1");
        }

        
        public void RenderViewWithViewNameAndData()
        {
            RenderView("View2", new { Prop1 = 1, Prop2 = 2 });
        }

        
        public void RenderViewWithViewNameAndMaster()
        {
            RenderView("View3", "Master3");
        }

        
        public void RenderViewWithViewNameAndMasterAndData()
        {
            RenderView("View4", "Master4", new { Prop1 = 3, Prop2 = 4 });
        }

        public virtual int RandomOtherFunction()
        {
            return 12345;
        }
    }
}
