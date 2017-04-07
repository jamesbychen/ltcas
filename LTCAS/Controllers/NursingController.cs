using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTCAS.Controllers
{
    public class NursingController : Controller
    {
        // GET: Nursing 護理作業
        public ActionResult Index()
        {
            return View();
        }
    }
}