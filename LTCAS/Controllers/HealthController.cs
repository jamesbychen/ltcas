using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTCAS.Controllers
{
    public class HealthController : Controller
    {
        // GET: Health  健康評估紀錄
        public ActionResult Index()
        {
            return View();
        }
    }
}