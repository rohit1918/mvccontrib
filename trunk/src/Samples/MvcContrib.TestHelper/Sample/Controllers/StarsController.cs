using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using MvcContrib.TestHelper.Sample.Models;
using System.Collections.Generic;

namespace MvcContrib.TestHelper.Sample.Controllers
{
    public class StarsController : Controller
    {
        [ControllerAction]
        public void List()
        {
            List<Star> stars = StarDatabase.GetStars();
            RenderView("List", stars);
        }

        [ControllerAction]
        public void ListWithLinks()
        {
            List<Star> stars = StarDatabase.GetStarsAndLinks(ControllerContext);
            RenderView("ListWithLinks", stars);
        }

        [ControllerAction]
        public void AddFormStar()
        {
            string name = Request.Form["NewStarName"];
            this.TempData["NewStarName"] = name;
            RedirectToAction("List");
        }

        [ControllerAction]
        public void AddSessionStar()
        {
            string name = Request.Form["NewStarName"];
            this.HttpContext.Session["NewStarName"] = name;
            RedirectToAction("List");
        }

        [ControllerAction]
        public void Nearby()
        {
            //Placeholder link for demonstration of link checking
            RedirectToAction("ListWithLinks");
        }
    }
}
