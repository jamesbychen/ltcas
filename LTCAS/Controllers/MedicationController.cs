using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTCAS.Controllers
{
    public class MedicationController : Controller
    {
        // GET: Medication  用藥記錄
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}