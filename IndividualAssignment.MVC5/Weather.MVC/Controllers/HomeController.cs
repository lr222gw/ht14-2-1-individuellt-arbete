using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weather.Domain;
using Weather.Domain.Webservices;

namespace Weather.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            YrWebservice yo = new YrWebservice();
            yo.preGetForecastFromLaNLo("58.34576", "15.13853");
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
    }
}