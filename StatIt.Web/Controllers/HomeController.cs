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
        //private readonly IDistimoService DistimoService;
        private readonly IRevenuesService RevenuesFactory;

        public HomeController(IRevenuesService revenuesFactory)
        {
            RevenuesFactory = revenuesFactory;
        }
        //
        // GET: /Home/
        public ActionResult Index()
        {
           
           // var eventData = DistimoService.GetEvents();

           // var iain = revenueData;
            
            return View();
        }

        public JsonResult GetRevenues(string AppId, string DateStart, string DateEnd)
        {

            var dateStart = DateTime.ParseExact(DateStart, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            var dateEnd = DateTime.ParseExact(DateEnd, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            //var revenueData = DistimoService.GetRevenues(AppId, dateStart, dateEnd);
            var revenueData = RevenuesFactory.GetRevenues(AppId, dateStart, dateEnd);

            var jsonData = Json(revenueData, JsonRequestBehavior.AllowGet);

            return jsonData;
        }

        //public JsonResult GetDownloads()
        //{
        //    var downloadData = DistimoService.GetDownloads();

        //    var jsonData = Json(downloadData, JsonRequestBehavior.AllowGet);

        //    return jsonData;
            
        //}
    }
}