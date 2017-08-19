using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace KYN_App_v1._1.RestServiceHelper
{
    public class ServiceInfo
    {
        //const string BaseServiceUri = "http://geocodeindia.com/KYNService.svc/";
        //const string BaseServiceUri = "http://localhost:81/KYNService.svc/";
        //const string BaseServiceUri = "http://localhost:52497/KYNService.svc/";
        const string BaseServiceUri = "http://crossfriend.com/KYNService.svc/";

        public static string TestGetService { get { return BaseServiceUri + "TestGetService"; } }
        public static string GetBusinessTypes { get { return BaseServiceUri + "GetBusinessTypes"; } }
        public static string GetBusinessCategories { get { return BaseServiceUri + "GetBusinessCategories"; } }
        public static string GetBusinessSubCategories { get { return BaseServiceUri + "GetBusinessSubCategories"; } }
        public static string CreateBusinessPrimaryDetail { get { return BaseServiceUri + "CreateBusinessPrimaryDetail"; } }
        public static string UpdateBusinessPrimaryDetail { get { return BaseServiceUri + "UpdateBusinessPrimaryDetail"; } }
        public static string GetBusinessPrimaryDetailList { get { return BaseServiceUri + "GetBusinessPrimaryDetailList"; } }
        public static string GetBusinessPrimaryDetailByID { get { return BaseServiceUri + "GetBusinessPrimaryDetailByID"; } }

        public static string GetBusinessDetailedCategories { get { return BaseServiceUri + "GetBusinessDetailedCategories"; } }

        public static string GetCountries { get { return BaseServiceUri + "GetCountries"; } }
        public static string GetStates { get { return BaseServiceUri + "GetStates"; } }
        public static string GetCities { get { return BaseServiceUri + "GetCities"; } }
        public static string GetRegions { get { return BaseServiceUri + "GetRegions"; } }
        public static string GetAddressTypes { get { return BaseServiceUri + "GetAddressTypes"; } }
        public static string GetListOfBuilding { get { return BaseServiceUri + "GetListOfBuilding"; } }

        public static string GetAllSubCategories { get { return BaseServiceUri + "GetAllSubCategories"; } }
        public static string GetAllDetailedAndResult { get { return BaseServiceUri + "GetAllDetailedAndResult"; } }
        public static string GetAllDetailedCategories { get { return BaseServiceUri + "GetAllDetailedCategories"; } }
        public static string GetAllSearchBusinessParentData { get { return BaseServiceUri + "GetAllSearchBusinessParentData"; } }
        public static string GetSearchBusinessInformationBySubCategoryID { get { return BaseServiceUri + "GetSearchBusinessInformationBySubCategoryID"; } }
        public static string GetSearchBusinessInformationByDetailCategoryID { get { return BaseServiceUri + "GetSearchBusinessInformationByDetailCategoryID"; } }

        public static string GetAllBlogTitle { get { return BaseServiceUri + "GetAllBlogTitle"; } }
        public static string GetBlogDetail { get { return BaseServiceUri + "GetBlogDetail"; } }
        public static string GetUserComments { get { return BaseServiceUri + "GetUserComments"; } }
        public static string SaveUserComment { get { return BaseServiceUri + "SaveUserComment"; } }
        public static string GetUserSupport { get { return BaseServiceUri + "GetUserSupport"; } }
        public static string SaveUserSupport { get { return BaseServiceUri + "SaveUserSupport"; } }


        public static string GetPromotionTypes { get { return BaseServiceUri + "GetPromotionTypes"; } }
        public static string GetPromotionValueTypes { get { return BaseServiceUri + "GetPromotionValueTypes"; } }
        public static string GetPromotionBasicEntity { get { return BaseServiceUri + "GetPromotionBasicEntity"; } }
        public static string CreatePromotionDetail { get { return BaseServiceUri + "CreatePromotionDetail"; } }
        public static string UpdatePromotionDetail { get { return BaseServiceUri + "UpdatePromotionDetail"; } }
        public static string GetAllPromotionForBusiness { get { return BaseServiceUri + "GetAllPromotionForBusiness"; } }
        public static string GenerateCouponCode { get { return BaseServiceUri + "GenerateCouponCode"; } }
        public static string GetAllActiveCoupons { get { return BaseServiceUri + "GetAllActiveCoupons"; } }
        public static string VerifyCoupon { get { return BaseServiceUri + "VerifyCoupon"; } }

        public static string CreateUser { get { return BaseServiceUri + "CreateUser"; } }
        public static string CreateUserPrimaryDetail { get { return BaseServiceUri + "CreateUserPrimaryDetail"; } }
        public static string CreateUserSecondaryDetail { get { return BaseServiceUri + "CreateUserSecondaryDetail"; } }
        public static string UpdateUserPrimaryDetail { get { return BaseServiceUri + "UpdateUserPrimaryDetail"; } }
        public static string UpdateUserSecondaryDetail { get { return BaseServiceUri + "UpdateUserSecondaryDetail"; } }

        public static string CreateUserProfile { get { return BaseServiceUri + "CreateUserProfile"; } }
        public static string GetUserProfile { get { return BaseServiceUri + "GetUserProfileEntity"; } }
        public static string UpdateUserProfile { get { return BaseServiceUri + "UpdateUserProfile"; } }

        public static string GetListOfSearchKeywords { get { return BaseServiceUri + "GetListOfSearchKeywords"; } }

        public static string CreateBusinessDetailUserRating { get { return BaseServiceUri + "CreateBusinessDetailUserRating"; } }
        public static string UpdateBusinessDetailUserRating { get { return BaseServiceUri + "UpdateBusinessDetailUserRating"; } }
        public static string GetAllBusinessUserRating { get { return BaseServiceUri + "GetAllBusinessUserRating"; } }



        public static string GetUser { get { return BaseServiceUri + "GetUser"; } }
        public static string GetBusinessDetail { get { return BaseServiceUri + "GetBusinessDetails"; } }

        public static string GetAllActivePromotions { get { return BaseServiceUri + "GetAllActivePromotions"; } }


        internal static T InvokeGetService<T>(string serviceUri, string _data)
        {
            T _returnResponse = default(T);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceUri);
                request.Headers.Add("Authorization", "Basic ");
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                _returnResponse = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
            catch (Exception ex)
            {

            }
            return _returnResponse;
        }

        internal static T InvokePostService<T>(string serviceUri, string _data)
        {
            T _returnResponse = default(T);
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            try
            {
                string url = serviceUri;
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/json; charset=utf-8";
                req.Timeout = 30000;
                req.Headers.Add("SOAPAction", url);
                req.ContentLength = _data.Length;
                var sw = new StreamWriter(req.GetRequestStream());
                sw.Write(_data);
                sw.Close();
                res = (HttpWebResponse)req.GetResponse();
                Stream responseStream = res.GetResponseStream();
                var streamReader = new StreamReader(responseStream);
                string responseString = streamReader.ReadToEnd();
                _returnResponse = JsonConvert.DeserializeObject<T>(responseString);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _returnResponse;
        }
    }
}