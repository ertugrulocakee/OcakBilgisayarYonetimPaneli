using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCTeknolojikMarketYonetimPaneli.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Page403()
        {

            Response.StatusCode = 403;
            Response.TrySkipIisCustomErrors = true;

            return View();

        }

        public ActionResult Page404()
        {

            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            return View();

        }

        public ActionResult Page500()
        {

            Response.StatusCode = 500;
            Response.TrySkipIisCustomErrors = true;

            return View();

        }

    }
}