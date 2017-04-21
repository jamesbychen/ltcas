﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LTCAS.Models;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using LTCAS.ViewModels;

namespace LTCAS.Controllers
{
    public class PublicFunctionController : Controller
    {
        private ltc_dbEntities db = new ltc_dbEntities();
        private string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["Elmah.Sql"].ConnectionString;
        private SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Elmah.Sql"].ConnectionString);
        private SqlCommand cmd = new SqlCommand();
        private SqlDataAdapter da = new SqlDataAdapter();
        private SqlDataReader dr;
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
            if (period.TotalMinutes > limit)
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
                    returnMsg = "password";//密碼錯誤
                }
                else
                {
                    returnMsg = lu.sn.ToString();//密碼正確則傳回SN

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
            SelectList sl = new SelectList(ur, "sn", "name");
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
                lu.status = 1;
                db.login_users.Add(lu);
                db.SaveChanges();
                //
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
            byte[] encryptMD5 = MD5.Create().ComputeHash(Encoding.Default.GetBytes(originalStr + key));//字串與KEY相加之後以MD5編碼
            output = Convert.ToBase64String(encryptMD5);//轉回STRING
            return output;
        }

        //Action history record
        public void actionRecord(int usersn, string action, string remark)
        {
            login_record rec = new login_record();
            rec.usersn = usersn;
            rec.recaction = action;
            rec.rectime = DateTime.Now;
            rec.remark = remark;
            db.login_record.Add(rec);
            db.SaveChanges();
        }

        //將Dictionary轉成JSON字串輸出
        public string Output2Json(Dictionary<string, string> list)
        {
            return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(list);
        }

        //新增住民資料（首次）
        public string insertPatientBasicInfo(FormCollection form)
        {
            cmd.CommandText = "usp_insertPatientBasicInfo";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@firstname", SqlDbType.NVarChar, 10).Value = form["firstname"].ToString();
            cmd.Parameters.Add("@lastname", SqlDbType.NVarChar, 20).Value = form["lastname"].ToString();
            cmd.Parameters.Add("@socialID", SqlDbType.NVarChar, 10).Value = form["socialID"].ToString();
            cmd.Parameters.Add("@birthday", SqlDbType.NVarChar, 10).Value = form["birthday"].ToString();
            cmd.Parameters.Add("@sex", SqlDbType.Char, 1).Value = form["sex"].ToString();
            cmd.Parameters.Add("@begin_date", SqlDbType.DateTime).Value = DateTime.Now;// form["beginDate"];
            cmd.Parameters.Add("@address", SqlDbType.NVarChar, 500).Value = form["address"].ToString();
            cmd.Parameters.Add("@telephone", SqlDbType.VarChar, 20).Value = form["telephone"].ToString();
            cmd.Parameters.Add("@shopno", SqlDbType.NVarChar, 10).Value = "1111";//暫時固定，shopno應該抓user的屬性
            cmd.Parameters.Add("@from_hospital", SqlDbType.NVarChar, 50).Value = form["fromHospital"].ToString();
            cmd.Parameters.Add("@from_hospital_code", SqlDbType.NVarChar, 10).Value = form["fromHospitalCode"].ToString();
            cmd.Parameters.Add("@diagnosis", SqlDbType.NVarChar, 1000).Value = form["diagnosis"].ToString();
            //首次不需輸入結案資訊
            cmd.Parameters.Add("@close_date", SqlDbType.DateTime).Value = DateTime.Parse("1900-01-01");// form["closeDate"];
            cmd.Parameters.Add("@close_reason", SqlDbType.NVarChar, 1000).Value = "";// form["closeReason"].ToString();
            //
            cmd.Parameters.Add("@remark", SqlDbType.NVarChar, 1000).Value = "";//備註欄位，暫時取消
            cmd.Parameters.Add("@contacts", SqlDbType.NVarChar, 50).Value = form["contacts"].ToString();
            cmd.Parameters.Add("@contact_relationship", SqlDbType.NVarChar, 10).Value = form["contact_relationship"].ToString();
            cmd.Parameters.Add("@contact_address", SqlDbType.NVarChar, 500).Value = form["contact_address"].ToString();
            cmd.Parameters.Add("@contact_telephone", SqlDbType.NVarChar, 20).Value = form["contact_telephone"].ToString();
            cmd.Parameters.Add("@contact_cellphone", SqlDbType.NVarChar, 20).Value = form["contact_cellphone"].ToString();
            //user ID
            cmd.Parameters.Add("@actionuser", SqlDbType.Int).Value = int.Parse(form["usersn"].ToString());
            //回傳值：
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.Output;
            cmd.Connection = conn;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            //回傳值：
            string result = cmd.Parameters["@result"].Value.ToString();
            //

            return result;
        }

        //住民資料（清單）
        public List<PatientBasicInfoViewModel> getPatientList(string keyword = "")
        {
            List<patient_basic_info> list = new List<patient_basic_info>();
            List<PatientBasicInfoViewModel> patientlist = new List<PatientBasicInfoViewModel>();
            ltc_dbEntities db = new ltc_dbEntities();
            list = db.patient_basic_info.Where(m => m.status == 1).ToList();
            foreach (var p in list)
            {
                //basic
                PatientBasicInfoViewModel pbi = new PatientBasicInfoViewModel();
                pbi.PatientSn = p.sn;
                pbi.firstname = p.firstname;
                pbi.lastname = p.lastname;
                pbi.socialID = p.socialID;
                pbi.sex = p.sex;
                pbi.birthday = p.birthday;
                pbi.beginDate = p.begindate.GetValueOrDefault(Convert.ToDateTime("1900/1/1")).ToString("yyyy/MM/dd");
                pbi.shopNo = p.shopno;
                pbi.fromHospital = p.from_hospital;
                pbi.closeDate = p.closedate.GetValueOrDefault(Convert.ToDateTime("1900/1/1")).ToString("yyyy/MM/dd");
                pbi.bindData();

                //contact
                List<ViewModels.PatientContactsViewModel> contactsList = new List<ViewModels.PatientContactsViewModel>();
                List<patient_contacts> pList = new List<patient_contacts>();
                pList = db.patient_contacts.Where(m => m.patient_sn == p.sn && m.status == 1).ToList();
                foreach (var c in pList)
                {
                    PatientContactsViewModel pcv = new PatientContactsViewModel();
                    pcv.contact = c.contacts;
                    pcv.address = c.address;
                    pcv.relationship = c.relationship;
                    pcv.telephone = c.telephone;
                    pcv.cellphone = c.cellphone;
                    contactsList.Add(pcv);
                }
                pbi.Contacts = contactsList;
                //
                patientlist.Add(pbi);


            }
            return patientlist;
        }

        //取出住民（基本）資料（單筆）
        public patient_basic_info getPatientBasicInfo(int patientSn)
        {
            patient_basic_info pbi = new patient_basic_info();
            pbi = db.patient_basic_info.Where(m => m.sn == patientSn).FirstOrDefault();
            return pbi;
        }
    }
    public class LoginInfo
    {
        private ltc_dbEntities db = new ltc_dbEntities();
        public string userid { get; set; }
        public string usermail { get; set; }
        public string shopno { get; set; }
        public string shopname { get; set; }
        public int userrole { get; set; }
        public string rolename { get; set; }
        public void getRoleName()
        {
            rolename = db.user_roles.Where(m => m.sn == userrole).FirstOrDefault().name.ToString();
        }

    }
}