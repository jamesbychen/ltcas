using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTCAS.Controllers
{
    public class PatientController : Controller
    {
        private PublicFunctionController pub = new PublicFunctionController();
        // GET: Patient 住民管理
        public ActionResult Index()
        {
            return PartialView("_search");
        }

        //input form
        public ActionResult AddBasic()
        {
            //將預設值丟進去
            LTCAS.ViewModels.PatientBasicInfoViewModel pbi = new ViewModels.PatientBasicInfoViewModel();
            pbi.beginDate = DateTime.Now.ToString("yyyy-MM-dd");
            //pbi.closeDate = DateTime.Now.ToString();
            return PartialView(pbi);
        }

        //輸入資料
        [HttpPost]
        public ActionResult saveBasic(FormCollection form)
        {
            string result = "0";
            //result= pub.insertPatientBasicInfo(form);
            if (result.Equals("0"))
            {
                Session.Add("result", "Add Failed!! Error:"+result);

            }
            else
            {
                Session.Add("result", "Add Success!! Result:"+result);

            }
            return RedirectToAction("../Home/Doctors");
        }
    }
}