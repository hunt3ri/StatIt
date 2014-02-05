using StatIt.Engine.Distimo.Services;
using StatIt.Engine.Distimo.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StatIt.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDistimoService DistimoService;

        public HomeController(IDistimoService distimoService)
        {
            DistimoService = distimoService;
        }
        //
        // GET: /Home/
        public ActionResult Index()
        {
           
           // var eventData = DistimoService.GetEvents();

           // var iain = revenueData;
            
            return View();
        }

        public JsonResult GetRevenues()
        {
            var queryString = Uri.UnescapeDataString(Request.QueryString.ToString());

            var revenueData = DistimoService.GetRevenues(queryString);

            var jsonData = Json(revenueData, JsonRequestBehavior.AllowGet);

            return jsonData;
        }

        public JsonResult GetDownloads()
        {
            var downloadData = DistimoService.GetDownloads();

            var jsonData = Json(downloadData, JsonRequestBehavior.AllowGet);

            return jsonData;
            
        }
    }
}