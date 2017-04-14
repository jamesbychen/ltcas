using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTCAS.Controllers
{
    public class DiagnosisController : Controller
    {
        // GET: Diagnosis   看診記錄
        public ActionResult Index()
        {
            ViewBag.Message = "看診記錄";
            return PartialView();
        }
    }
}