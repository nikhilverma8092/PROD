using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYN_App_v1._1.Models
{
    public class BaseViewModel
    {
        public int ID { get; set; }
        public int RegionID { get; set; }
        public string ItemType { get; set; }
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string KeyWord { get; set; }
        public string Detail { get; set; }
        public bool IsValid { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int SubCategoryID { get; set; }
        public string SubCategoryName { get; set; }
    }
}