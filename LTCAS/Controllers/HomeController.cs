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
        // GET: Home
        public ActionResult Index()
        {
            PublicFunctionController pub = new PublicFunctionController();
            string token = pub.generateTimestampToken();
            string timestamp = pub.decodeTimestampToken(token).ToString();
            ViewBag.Token = token;
            ViewBag.Timestamp = timestamp;
            writeTokenCookie("write cookie");
            ViewBag.Cookie = readTokenCookie();
            return View();
        }

        public ActionResult testError()
        {
            //throw new NotImplementedException();
            Response.StatusCode = 404;
            throw new HttpException();
            //return View();
        }

        //登入畫面
        public ActionResult UserLogin()
        {

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
            login_user lu = new login_user();
            lu.sn = int.Parse(json["sn"].ToString());
            lu.userid = json["userid"].ToString();
            lu.username = json["username"].ToString();
            lu.password = json["password"].ToString();
            lu.logintime = DateTime.Parse(json["logintime"].ToString());
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
            if(cookie!=null)
                return cookie.Value;
            else
            {
                return "";
            }
        }
        #endregion
    }
}