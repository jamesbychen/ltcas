using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LTCAS.Models;
namespace LTCAS.Controllers
{
    public class HomeController : Controller
    {
        PublicFunctionController pub = new PublicFunctionController();

        // GET: Home
        public ActionResult Index()
        {
            string token = pub.generateTimestampToken();
            string timestamp = pub.decodeTimestampToken(token).ToString();
            ViewBag.Token = token;
            ViewBag.Timestamp = timestamp;
            //writeTokenCookie("james",token);
            //ViewBag.Cookie = token;
            ViewBag.Browser = pub.getBrowserInfo(Request.Browser);
            //ViewBag.Browser = Request.UserAgent;
            ViewBag.userOS = pub.getOSName(Request.UserAgent);
            //ViewBag.Browser = Request.UserHostAddress;
            //確認登入
            if (Request.Cookies["logintoken"] != null)
            {
                //檢查逾時
                if (!pub.isTokenOver(Request.Cookies["logintoken"].Value.ToString()))
                {
                    //仍在時效內則繼續進行
                    ltc_dbEntities db = new ltc_dbEntities();
                    int usersn = int.Parse(Request.Cookies["usersn"].Value.ToString());
                    login_users user = db.login_users.Where(m => m.sn == usersn).FirstOrDefault();
                    LoginInfo login = new LoginInfo();
                    login.userid = user.userid;
                    login.userrole = user.userrole;
                    login.getRoleName();
                    //
                    string ip = Request.UserHostAddress;
                    HttpBrowserCapabilitiesBase browser = Request.Browser;
                    string userAgent = Request.UserAgent;
                    ViewBag.Message = "Name:" + user.username + "<br />"
                        + "Shop No:" + user.shopno + "<br />"
                        + "Role:" + login.rolename + "<br />"
                        + "Login Time:" + pub.decodeTimestampToken(Request.Cookies["logintoken"].Value.ToString()) + "<br />";
                    //
                }
                else
                {
                    return RedirectToAction("UserLogin");
                }
            }
            else
            {
                return RedirectToAction("UserLogin");
            }

            return View();
        }


        //登入畫面
        public ActionResult UserLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserLogin(string userid, string password)
        {
            if (userid.Length > 0 & password.Length > 0)
            {
                string ret = pub.checkLogin(userid, password);
                if (ret.Equals("userid") || ret.Equals("password"))
                {
                    switch (ret)
                    {
                        case "userid":
                            ViewBag.Message = "無此帳號請重新輸入";
                            break;
                        case "password":
                            ViewBag.Message = "密碼錯誤！";
                            break;
                        default:
                            ViewBag.Message = "unknow error！";
                            break;
                    }
                }
                else
                {
                    //登入成功寫入COOKIE
                    writeTokenCookie(ret, pub.generateTimestampToken());
                    //紀錄
                    string ip = Request.ServerVariables.Get("Remote_Addr").ToString(); //Request.UserHostAddress;//ip address
                    string browser = Request.Browser.Browser + Request.Browser.Version;//browser
                    string userAgent = pub.getOSName(Request.UserAgent);//OS
                    string remark = "IP:" + ip + ";Browser:" + browser + ";OS:" + userAgent;
                    pub.actionRecord(int.Parse(ret), "login", remark);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public ActionResult logout()
        {
            clearCookie();
            //Redirect("/index");
            return View();
        }

        //管理員：新增/修改使用者資訊
        public ActionResult EditLoginUser()
        {
            //從PostMan產生的RestSharp去呼叫API
            var client = new RestClient("http://localhost:2719/api/loginuser/3");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "9b04b6e0-a007-0f4e-ad44-2167a890904a");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            var json = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
            login_users lu = new login_users();
            lu.sn = int.Parse(json["sn"].ToString());
            lu.userid = json["userid"].ToString();
            lu.username = json["username"].ToString();
            lu.password = json["password"].ToString();
            return View(lu);
        }

        //忘記密碼，傳送新的密碼去使用者的EMAIL
        public ActionResult ForgetPassword()
        {

            return View();
        }

        //Doctor Page
        public ActionResult Doctors()
        {
            if (Session["result"] != null)
            {
                ViewBag.Message = Session["result"].ToString();

            }
            return View();
        }


        //手動新增帳號
        public ActionResult AddLoginUser()
        {
            //取得腳色清單
            //ViewBag.userrole = pub.getRoleList();
            AddUser au = new AddUser();
            //au.userrole = pub.getRoleList();
            au.userrole = pub.getRoleList2();
            //default value

            return View(au);
        }
        [HttpPost]
        public ActionResult AddLoginUser(AddUser au)
        {
            //ViewBag.userrole = au.userrole;
            au.userrole = pub.getRoleList2();
            ViewBag.message = pub.addLoginUser(au.userid, au.shopno, au.username, au.selectRole, au.usermail, pub.mixwithMD5(au.password));

            return View(au);
        }

        #region R/W Token to cookie
        private void writeTokenCookie(string usersn, string token)
        {
            System.Web.HttpCookie cookieUser = new System.Web.HttpCookie("usersn");
            System.Web.HttpCookie cookieToken = new System.Web.HttpCookie("logintoken");
            cookieUser.Value = usersn;
            cookieToken.Value = token;
            Response.Cookies.Add(cookieUser);
            Response.Cookies.Add(cookieToken);
        }

        private string readTokenCookie()
        {
            System.Web.HttpCookie cookie = Request.Cookies["logintoken"];
            if (cookie != null)
                return cookie.Value;
            else
            {
                return "";
            }
        }
        private string readUserIDCookie()
        {
            System.Web.HttpCookie cookie = Request.Cookies["usersn"];
            if (cookie != null)
                return cookie.Value;
            else
            {
                return "";
            }

        }
        private void clearCookie()
        {
            //只是將cookie設定逾時
            if (Request.Cookies["userid"] != null)
            {
                Request.Cookies["userid"].Expires = DateTime.Now.AddDays(-1d);
            }
            if (Request.Cookies["logintoken"] != null)
            {
                Request.Cookies["logintoken"].Expires = DateTime.Now.AddDays(-1d);
            }
        }
        #endregion

        #region TEST
        public ActionResult testConnection()
        {
            string connStr = "Data Source=192.168.0.49;Initial Catalog=inter_hfamily;User Id=hfamily_user;Password=hfamily_user;";
            connStr = System.Configuration.ConfigurationManager.ConnectionStrings["Elmah.Sql"].ConnectionString;
            ViewBag.connstr = connStr;
            string cmdStr = "select sn from login_record";
            string cmdStr2 = @"insert into login_record values('aaa',getdate(),'test','test')";
            //create connection
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connStr);
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmdStr, conn);
            da.InsertCommand = new System.Data.SqlClient.SqlCommand();
            da.InsertCommand.CommandText = cmdStr2;
            da.InsertCommand.Connection = conn;
            conn.Open();
            da.InsertCommand.ExecuteNonQuery();
            conn.Close();
            System.Data.DataTable dt = new System.Data.DataTable();
            da.Fill(dt);
            ViewBag.ErrorID = dt.Rows[0][0].ToString();
            return View();
        }
        public ActionResult testError()
        {
            //throw new NotImplementedException();
            //Response.StatusCode = 404;
            //throw new HttpException();
            ViewBag.Cookie = readTokenCookie();
            return View();
        }

        #endregion
    }

    public class AddUser
    {
        public string userid { get; set; }
        public string shopno { get; set; }
        public SelectList userrole { get; set; }
        //public List<SelectListItem> userrole { get; set; }
        public string selectRole { get; set; }
        public string username { get; set; }
        public string usermail { get; set; }
        public string password { get; set; }
    }
}