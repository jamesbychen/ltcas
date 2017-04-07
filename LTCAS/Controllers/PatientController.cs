using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTCAS.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient 住民管理
        public ActionResult Index()
        {
            return View();
        }
    }
}