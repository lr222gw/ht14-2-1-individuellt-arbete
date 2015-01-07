using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weather.Domain;
using Weather.Domain.Repositories;
using Weather.Domain.Webservices;

namespace Weather.MVC.Controllers
{
    public class HomeController : Controller
    {
        SuperService service = new SuperService();
        
        public ActionResult Index()
        {
            //YrWebservice yo = new YrWebservice();
            //var h = yo.preGetForecastFromLaNLo("58.34576", "15.13853");
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index_Post(string searchvalue)
        {

            var h = service.search(searchvalue);
            
            return View(h);
        }

        public ActionResult Forecast(string geonameID)
        {
            if (geonameID == null)
            {
                return View("Index");
            }
            var theLocation = service.getLocationFromGeoID(geonameID);
            service.getImageForLocationForecasts(theLocation);
            return View(theLocation);
        }


    }
}