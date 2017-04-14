using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LTCAS.ViewModels
{
    //住民基本資料
    public class PatientBasicInfoViewModel
    {

        public Int32 PatientSn { get; set; }
        [Display(Name = "姓氏 Last Name")]
        public string lastname { get; set; }
        [Display(Name = "名字 First Name")]
        public string firstname { get; set; }
        [Display(Name = "身份證字號 Social ID")]
        public string socialID { get; set; }
        [Display(Name = "性別 Sex Gender")]
        public string sex { get; set; }
        [Display(Name = "生日 Birthday")]
        public string birthday { get; set; }
        [Display(Name = "創表日期")]
        public string createDate { get; set; }
        [Display(Name = "開案日期 Begin Date")]
        public string beginDate { get; set; }
        [Display(Name = "地址 Address")]
        public string address { get; set; }
        [Display(Name = "電話 Telephone")]
        public string telephone { get; set; }
        [Display(Name = "機構代碼 Shop No")]
        public string shopNo { get; set; }
        [Display(Name = "轉介單位")]
        public string fromHospital { get; set; }
        [Display(Name = "轉介單位代號")]
        public string fromHospitalCode { get; set; }
        [Display(Name = "診斷 Diagonsis")]
        public string diagnosis { get; set; }
        [Display(Name = "結案日期 Close Date")]
        public string closeDate { get; set; }
        [Display(Name = "結案原因 Close Reason")]
        public string closeReason { get; set; }

        public List<PatientContactsViewModel> Contacts { get; set; }
    }
    
    //住民聯絡人資訊
    public class PatientContactsViewModel
    {
        [Display(Name = "聯絡人 Contact")]
        public string contact { get; set; }
        [Display(Name = "關係 Relationship")]
        public string relationship { get; set; }
        [Display(Name = "聯絡地址 Address")]
        public string address { get; set; }
        [Display(Name = "聯絡電話 Telephone Number")]
        public string telephone { get; set; }
        [Display(Name = "行動電話 Cellphone Number")]
        public string cellphone { get; set; }

    }
}