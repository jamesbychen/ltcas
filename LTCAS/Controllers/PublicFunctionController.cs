using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LTCAS.Models;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;

namespace LTCAS.Controllers
{
    public class PublicFunctionController : Controller
    {
        private ltc_dbEntities db = new ltc_dbEntities();
        //產生時間戳記的TOKEN
        public string generateTimestampToken()
        {
            DateTime now = DateTime.Now;
            //test
            //now = DateTime.Now.AddMinutes(-20.0);
            //
            byte[] time = BitConverter.GetBytes(now.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            return token;
        }

        //將TOKEN的時間解析出來
        public DateTime decodeTimestampToken(string token)
        {
            byte[] data = Convert.FromBase64String(token);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));

            return when;
        }

        //檢查TOKEN中的時間是否逾時
        public bool isTokenOver(string token)
        {
            bool overtime = true;
            int limit = 120;//預設為2hr(120min)
            DateTime tokenTime = decodeTimestampToken(token);
            TimeSpan period = DateTime.Now - tokenTime;
            if (period.Minutes > limit)
                overtime = true;//逾時成立
            else
                overtime = false;//尚在時限內
            return overtime;
        }

        public string checkLogin(string id, string password)
        {
            //ltc_dbEntities db = new ltc_dbEntities();

            string returnMsg = "";
            login_users lu = db.login_users.FirstOrDefault(p => p.userid == id);
            if (lu == null)
            {
                returnMsg = "userid";//Can not find user ID
            }
            else
            {
                if (!mixwithMD5(password).Equals(lu.password))
                {
                    returnMsg = "password";//Wrong password
                }
            }
            return returnMsg;
        }

        public List<SelectListItem> getRoleList()
        {
            List<user_roles> lur = db.user_roles.ToList();
            List<SelectListItem> item = new List<SelectListItem>();

            item.Add(new SelectListItem() { Value = "", Text = "----------", Selected = true });
            foreach (user_roles ur in lur)
            {
                item.Add(new SelectListItem() { Value = ur.sn.ToString(), Text = ur.name.ToString() });
            }

            return item;
        }
        public SelectList getRoleList2()
        {
            var ur = db.user_roles;
            SelectList sl = new SelectList(ur,"please select");
            return sl;
        }


        public string addLoginUser(string userid, string shopno, string username, string userrole, string useremail, string password)
        {
            //檢查是否已有此帳號
            string dup = checkLogin(userid, "");
            if (dup.Equals("password"))
            {
                return "帳號已重複請重新輸入！";
            }
            else
            {
                login_users lu = new login_users();
                lu.userid = userid;
                lu.username = username;
                lu.shopno = shopno;
                lu.userrole = int.Parse(userrole);
                lu.usermail = useremail;
                lu.password = password;
                db.login_users.Add(lu);
                db.SaveChanges();
                return "Add User-" + username + "Success!";
            }
        }

        //
        public string getBrowserInfo(HttpBrowserCapabilitiesBase browser)
        {
            string s = "Browser Capabilities<br />"
        + "Type = " + browser.Type + "<br />"
        + "Name = " + browser.Browser + "<br />"
        + "Version = " + browser.Version + "<br />"
        + "Major Version = " + browser.MajorVersion + "<br />"
        + "Minor Version = " + browser.MinorVersion + "<br />"
        + "Platform = " + browser.Platform + "<br />"
        + "Is Beta = " + browser.Beta + "<br />"
        + "Is Crawler = " + browser.Crawler + "<br />"
        + "Is AOL = " + browser.AOL + "<br />"
        + "Is Win16 = " + browser.Win16 + "<br />"
        + "Is Win32 = " + browser.Win32 + "<br />"
        + "Supports Frames = " + browser.Frames + "<br />"
        + "Supports Tables = " + browser.Tables + "<br />"
        + "Supports Cookies = " + browser.Cookies + "<br />"
        + "Supports VBScript = " + browser.VBScript + "<br />"
        + "Supports JavaScript = " +
            browser.EcmaScriptVersion.ToString() + "<br />"
        + "Supports Java Applets = " + browser.JavaApplets + "<br />"
        + "Supports ActiveX Controls = " + browser.ActiveXControls
              + "<br />";
            return s;
        }


        //OS Version
        public string getOSName(string UserAgen)
        {
            string os = "";
            if (UserAgen.IndexOf("Windows NT 10.0") > -1)
            {
                os = "Windows 10";
            }
            else if (UserAgen.IndexOf("Windows NT 6.3") > -1)
            {
                os = "Windows 8.1 / Windows Server 2012 R2";
            }
            else if (UserAgen.IndexOf("Windows NT 6.2") > -1)
            {
                os = "Windows 8 / Windows Server 2012";
            }
            else if (UserAgen.IndexOf("Windows NT 6.1") > -1)
            {
                os = "Windows 7 / Windows Server 2008 R2";
            }
            else if (UserAgen.IndexOf("Windows NT 6.0") > -1)
            {
                os = "Windows Vista / Windows Server 2008";
            }
            else if (UserAgen.IndexOf("Windows NT 5.2") > -1)
            {
                os = "Windows Server 2003 / Windows Server 2003 R2";
            }
            else if (UserAgen.IndexOf("Windows NT 5.1") > -1)
            {
                os = "Windows XP";
            }
            else if (UserAgen.IndexOf("Windows NT 5.0") > -1)
            {
                os = "Windows 2000";
            }

            return os;
        }

        //Password with MD5
        public string mixwithMD5(string originalStr)
        {
            string output = "";//輸出
            string key = "TEST1234";//加密鑰匙
            byte[] encryptMD5 =MD5.Create().ComputeHash(Encoding.Default.GetBytes(originalStr+key));//字串與KEY相加之後以MD5編碼
            output = Convert.ToBase64String(encryptMD5);//轉回STRING
            return output;
        }

        //Action history record
        public void actionRecord(string userid,string action,string remark)
        {
            login_record rec = new login_record();
            rec.userid = userid;
            rec.recaction = action;
            rec.rectime = DateTime.Now;
            rec.remark = remark;
            db.login_record.Add(rec);
            db.SaveChanges();
        }
    }
}