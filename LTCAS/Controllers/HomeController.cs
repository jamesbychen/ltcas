using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTCAS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //ViewBag.Token = pub.generateToken(0).ToString();
            return View();
        }

        //登入畫面
        public ActionResult UserLogin()
        {
            return View();
        }

        //管理員：新增/修改使用者資訊
        public ActionResult EditLoginUser()
        {
            return View();
        }

        //忘記密碼，傳送新的密碼去使用者的EMAIL
        public ActionResult ForgetPassword()
        {
            
            return View();
        }
    }
}