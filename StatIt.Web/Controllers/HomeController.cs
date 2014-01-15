using StatIt.Engine.Distimo.Services.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StatIt.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            var test = new DistimoService();
            test.GetDownloads();
            return View();
        }
    }
}