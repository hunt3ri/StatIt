using StatIt.Engine.Distimo.Services;
using StatIt.Engine.Distimo.Services.Models;
using StatIt.Engine.Flurry.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StatIt.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRevenuesService RevenuesService;
        private readonly IFlurryService FlurryService;

        public HomeController(IRevenuesService revenuesService, IFlurryService flurryService)
        {
            RevenuesService = revenuesService;
            FlurryService = flurryService;
        }
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetDAU(string DateStart, string DateEnd)
        {
            var dateStart = DateTime.ParseExact(DateStart, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            var dateEnd = DateTime.ParseExact(DateEnd, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);


            var dauData = FlurryService.GetActiveUsers(dateStart, dateEnd);

            var jsonData = Json(dauData, JsonRequestBehavior.AllowGet);

            return jsonData;
        }

        public JsonResult GetIAPRevenues(string AppId, string DateStart, string DateEnd)
        {
            var dateStart = DateTime.ParseExact(DateStart, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            var dateEnd = DateTime.ParseExact(DateEnd, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            var iapData = RevenuesService.GetIAPRevenues(AppId, dateStart, dateEnd);

            var jsonData = Json(iapData, JsonRequestBehavior.AllowGet);
            return jsonData;
        }

        public JsonResult GetRevenues(string AppId, string DateStart, string DateEnd)
        {

            var dateStart = DateTime.ParseExact(DateStart, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            var dateEnd = DateTime.ParseExact(DateEnd, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            var revenueData = RevenuesService.GetRevenues(AppId, dateStart, dateEnd);

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