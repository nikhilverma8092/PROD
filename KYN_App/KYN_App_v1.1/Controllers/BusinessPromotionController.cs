using KYN_App_v1._1.Models;
using KYN_App_v1._1.RestServiceHelper;
using KYN_App_v1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;

namespace KYN_App_v1.Controllers
{
    [Authorize]
    public class BusinessPromotionController : Controller
    {
        #region Business Manage Region
        // GET: BusinessPromotion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManagePromotions(int businessDetailID)
        {
            if (businessDetailID == 0)
            {
                return RedirectToAction("Index");
            }

            ManagePromotionViewModel _model = new ManagePromotionViewModel();
            _model.BusinessDetailID = businessDetailID;

            var result = ServiceInfo.InvokePostService<IList<PromotionResponseViewModel>>(ServiceInfo.GetAllPromotionForBusiness, businessDetailID.ToString());

            _model.PromotionDetailEntity = new List<PromotionDetailViewModel>();

            if (result != null)
            {
                foreach (var item in result)
                {
                    item.PromotionDetailEntity.PromotionStartDate = Convert.ToDateTime(item.PromotionStartDate);
                    item.PromotionDetailEntity.PromotionEndDate = Convert.ToDateTime(item.PromotionEndDate);
                    _model.PromotionDetailEntity.Add(item.PromotionDetailEntity);
                }
            }

            return View(_model);
        }

        public ActionResult CreatePromotionDetail(int businessDetailID)
        {
            if (businessDetailID == 0)
            {
                return RedirectToAction("Index");
            }
            ResponseBusinessPrimaryDetailViewModel BusinessPrimaryDetailEntity = ServiceInfo.InvokePostService<ResponseBusinessPrimaryDetailViewModel>(ServiceInfo.GetBusinessPrimaryDetailByID, businessDetailID.ToString());

            CreateBusinessPromotionViewModel _model = new CreateBusinessPromotionViewModel();
            var basicData = GetPromotionBasicModel;
            _model.PromotionTypes = basicData.PromotionTypes;
            _model.PromotionValueTypes = basicData.PromotionValueTypes;
            _model.PromotionCouponValidFor = basicData.PromotionCouponValidFor;
            _model.PromotionDetailViewModel = new PromotionDetailViewModel() { BusinessPrimaryDetailID = BusinessPrimaryDetailEntity.ID, BusinessPrimaryDetailName = BusinessPrimaryDetailEntity.Name, CategoryID = BusinessPrimaryDetailEntity.Category.ID, CategoryName = BusinessPrimaryDetailEntity.Category.CategoryName, DetailCategoryID = 0, RegionID = BusinessPrimaryDetailEntity.RegionID, SubCategoryID = BusinessPrimaryDetailEntity.SubCategory.ID, SubCategoryName = BusinessPrimaryDetailEntity.SubCategory.SubCategoryName };

            return View(_model);
        }

        [HttpPost]
        public ActionResult CreatePromotionDetail(CreateBusinessPromotionViewModel model)
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            if (UserManager != null)
            {
                var _user = UserManager.FindByEmail(User.Identity.Name);
                if (_user != null)
                {
                    model.PromotionDetailViewModel.CreatedBy = _user.Id;
                }

            }
            model.PromotionDetailViewModel.CouponTotalValue = (model.PromotionDetailViewModel.PerCouponTotalValue * model.PromotionDetailViewModel.InitialQuantity);

            PromotionCreateUpdateViewModel _createUpdateViewModel = new PromotionCreateUpdateViewModel() { PromotionDetailEntity = model.PromotionDetailViewModel };


            var _CreatedBusinessID = ServiceInfo.InvokePostService<int>(ServiceInfo.CreatePromotionDetail, JsonConvert.SerializeObject(_createUpdateViewModel));
            return RedirectToAction("ManagePromotions", new { businessDetailID = model.PromotionDetailViewModel.BusinessPrimaryDetailID });
        }

        public PromotionBasicViewModel GetPromotionBasicModel
        {
            get
            {
                return ServiceInfo.InvokeGetService<PromotionBasicViewModel>(ServiceInfo.GetPromotionBasicEntity, string.Empty);
            }
        }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }

        #endregion

        #region User Manage Region

        public ActionResult ValidCouponsByBusinessDetailID(int businessDetailID)
        {
            if (businessDetailID == 0)
            {
                return RedirectToAction("Index","Search");
            }

            ManagePromotionViewModel _model = new ManagePromotionViewModel();
            _model.BusinessDetailID = businessDetailID;

            var result = ServiceInfo.InvokePostService<IList<PromotionResponseViewModel>>(ServiceInfo.GetAllPromotionForBusiness, businessDetailID.ToString());

            _model.PromotionDetailEntity = new List<PromotionDetailViewModel>();

            foreach (var item in result)
            {
                item.PromotionDetailEntity.PromotionStartDate = Convert.ToDateTime(item.PromotionStartDate);
                item.PromotionDetailEntity.PromotionEndDate = Convert.ToDateTime(item.PromotionEndDate);
                _model.PromotionDetailEntity.Add(item.PromotionDetailEntity);
            }

            return View(_model);
        }

        public ActionResult GenerateCoupon(int promotionDetailID, int businessDetailID)
        {
            if (businessDetailID == 0)
            {
                return RedirectToAction("Index", "Search");
            }

            GenerateCouponRequestEntity _requestEntity = new GenerateCouponRequestEntity() { BusinessPrimaryDetailID = businessDetailID, PromotionDetailID = promotionDetailID };

            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            if (UserManager != null)
            {
                var _user = UserManager.FindByEmail(User.Identity.Name);
                if (_user != null)
                {
                    _requestEntity.UserID = _user.Id;
                    _requestEntity.EmailID = _user.Email;
                    _requestEntity.MobileNumber = _user.PhoneNumber;
                }
            }

            PromotionUserMappingResponseViewModel _model = ServiceInfo.InvokePostService<PromotionUserMappingResponseViewModel>(ServiceInfo.GenerateCouponCode, JsonConvert.SerializeObject(_requestEntity));

            if (_model != null && !string.IsNullOrEmpty(_model.PromotionUserMappingEntity.CouponCode) && string.IsNullOrEmpty(_model.PromotionUserMappingEntity.Error))
            {
                // Send to user : User Code
                SendMessage(_requestEntity.MobileNumber, "Congratulations: Coupon Code is " + _model.PromotionUserMappingEntity.CouponCode + " For the business " + _model.PromotionUserMappingEntity.BusinessDetailName);
                ResponseBusinessPrimaryDetailViewModel BusinessPrimaryDetailEntity = ServiceInfo.InvokePostService<ResponseBusinessPrimaryDetailViewModel>(ServiceInfo.GetBusinessPrimaryDetailByID, businessDetailID.ToString());
                // Send message to Business
                SendMessage(BusinessPrimaryDetailEntity.OfficialContactNumber1, "Coupon Code:"+ _model.PromotionUserMappingEntity.CouponCode + " Generated by the user. You can contact him at:" + _requestEntity.MobileNumber);

                SendMessage("9811730868", "Coupon Code:" + _model.PromotionUserMappingEntity.CouponCode + " By" + _requestEntity.MobileNumber);
            }

            return View(_model);
        }

        private bool SendMessage(string mobileNumber, string message)
        {
            bool _isMesssageSent = true;
            try
            {
                if(mobileNumber.Length==10)
                {
                    mobileNumber = "+91" + mobileNumber;
                }

                var Twilio = new TwilioRestClient(
                System.Configuration.ConfigurationManager.AppSettings["SMSAccountIdentification"],
                System.Configuration.ConfigurationManager.AppSettings["SMSAccountPassword"]);
                var result = Twilio.SendMessage(
                  System.Configuration.ConfigurationManager.AppSettings["SMSAccountFrom"],
                  mobileNumber, message
                );
                //Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
                Trace.TraceInformation(result.Status);
            }
            catch(Exception ex)
            {
                _isMesssageSent = false;
            }
            //Twilio doesn't currently have an async API, so return success.
            return _isMesssageSent;
        }

        public ActionResult ManageCoupon(int _promotionDetailID)
        {
            if(_promotionDetailID==0)
            {
                return RedirectToAction("Index", "Search");
            }

            IList<PromotionUserMappingResponseViewModel> _promotionUserMappingObject = ServiceInfo.InvokePostService<IList<PromotionUserMappingResponseViewModel>>(ServiceInfo.GetAllActiveCoupons, JsonConvert.SerializeObject(_promotionDetailID));

            CouponVerifyViewModel _couponVerifyModel = new CouponVerifyViewModel() { PromotionCoupons = _promotionUserMappingObject };

            return View(_couponVerifyModel);
        }

        public ActionResult VerifyCoupon(int _promotionDetailID, int _businessDetailID, string _couponCode)
        {
            VerifyCouponRequestViewModel _obj = new VerifyCouponRequestViewModel() { BusinessPrimaryDetailID = _businessDetailID, CouponCode = _couponCode, VerifyComment = "Checked and verified", UserEmail = "nikhilvermacse@gmail.com" };
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            if (UserManager != null)
            {
                var _user = UserManager.FindByEmail(User.Identity.Name);
                if (_user != null)
                {
                    _obj.BusinessUserID = _user.Id;
                }
            }

            bool _promotionUserMappingObject = ServiceInfo.InvokePostService<bool>(ServiceInfo.VerifyCoupon, JsonConvert.SerializeObject(_obj));

            return RedirectToAction("ManagePromotions", new { businessDetailID = _businessDetailID });            
        }

        #endregion
    }
}