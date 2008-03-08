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
        
        public void List()
        {
            List<Star> stars = StarDatabase.GetStars();
            RenderView("List", stars);
        }

        
        public void ListWithLinks()
        {
            List<Star> stars = StarDatabase.GetStarsAndLinks(ControllerContext);
            RenderView("ListWithLinks", stars);
        }

        
        public void AddFormStar()
        {
            string name = Request.Form["NewStarName"];
            this.TempData["NewStarName"] = name;
            RedirectToAction("List");
        }

        
        public void AddSessionStar()
        {
            string name = Request.Form["NewStarName"];
            this.HttpContext.Session["NewStarName"] = name;
            RedirectToAction("List");
        }

        
        public void Nearby()
        {
            //Placeholder link for demonstration of link checking
            RedirectToAction("ListWithLinks");
        }
    }
}
