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
            writeTokenCookie(token);
            bool over = pub.isTokenOver(token);
            //ViewBag.Cookie = token;
            //ViewBag.Browser = pub.getBrowserInfo(Request.Browser);
            //ViewBag.Browser = Request.UserAgent;
            ViewBag.Browser = pub.getOSName(Request.UserAgent);
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
                if (ret.Length == 0)
                {
                    Redirect("/index");
                }
                else
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
                            ViewBag.Message = "發生未知的錯誤！";
                            break;
                    }
                }
            }
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

        public ActionResult testConnection()
        {
            string connStr = @"Data Source=JAMES-PC\SQLEXPRESS;Initial Catalog=ltc_db;User Id=ltc01;Password=1234;Integrated Security=True";
            connStr = System.Configuration.ConfigurationManager.ConnectionStrings["Elmah.Sql"].ConnectionString;
            ViewBag.connstr = connStr;
            string cmdStr = "select ErrorId from ELMAH_Error";
            string cmdStr2 = @"INSERT INTO [dbo].[ELMAH_Error]
           ([Application]
           ,[Host]
           ,[Type]
           ,[Source]
           ,[Message]
           ,[User]
           ,[StatusCode]
           ,[TimeUtc]
           ,[AllXml])
                 VALUES
           ('app'
           ,'host'
           ,'type'
           ,'source'
           ,'message'
           ,'user'
           ,500
           ,getdate()
           ,'<XML></XML>')";
            //create connection
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connStr);
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmdStr, conn);
            da.InsertCommand = new System.Data.SqlClient.SqlCommand();
            da.InsertCommand.CommandText = cmdStr2;
            da.InsertCommand.Connection = conn;
            //conn.Open();
            da.InsertCommand.ExecuteNonQuery();
            //conn.Close();
            System.Data.DataTable dt = new System.Data.DataTable();
            da.Fill(dt);
            ViewBag.ErrorID = dt.Rows[0][0].ToString();
            return View();
        }


        //手動新增帳號
        public ActionResult AddLoginUser()
        {
            ViewBag.userrole = pub.getRoleList();//取得腳色清單
            //default value

            return View();
        }
        [HttpPost]
        public ActionResult AddLoginUser(AddUser au)
        {
            ViewBag.userrole = au.userrole;

            ViewBag.message = pub.addLoginUser(au.userid, au.shopno, au.username,au.selectRole, au.usermail, au.password);

            return View();
        }

        #region R/W Token to cookie
        private void writeTokenCookie(string token)
        {
            System.Web.HttpCookie cookie = new System.Web.HttpCookie("logintoken");
            cookie.Value = token;
            //Response.AppendCookie(cookie);
            Response.Cookies.Add(cookie);
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
        #endregion
    }

    public class AddUser
    {
        public string userid { get; set; }
        public string shopno { get; set; }
        public List<SelectListItem> userrole { get; set; }
        public string selectRole { get; set; }
        public string username { get; set; }
        public string usermail { get; set; }
        public string password { get; set; }
    }
}