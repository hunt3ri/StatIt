using StatIt.Engine.Distimo.Services;
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
            
            return View();
        }

        public JsonResult GetDownloads()
        {
            var downloadData = DistimoService.GetDownloads();

            var jsonData = Json(downloadData, JsonRequestBehavior.AllowGet);

            return jsonData;
            
        }
    }
}