using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYN_App_v1._1.HelperViewModels
{
    public class AddressTypeViewModel
    {
        public int ID { get; set; }
        public string AddressTypeName { get; set; }
        public string Detail { get; set; }
    }

    public class AddressViewModel
    {
        public int RegionID { get; set; }
        public int AddressTypeID { get; set; }
        public int BuildingID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Pincode { get; set; }
    }

    public class BuildingInformationViewModel
    {
        public int ID { get; set; }
        public int RegionID { get; set; }
        public int AddressTypeID { get; set; }
        public string BuildingName { get; set; }
        public string BuildingInformation { get; set; }
    }

    public class RequestBuildingInformationViewModel
    {
        public int RegionID { get; set; }
        public int AddressTypeID { get; set; }
    }
}