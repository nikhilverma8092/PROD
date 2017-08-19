using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using KYN_App_v1._1.HelperViewModels;
using KYN_App_v1._1.Models;
using KYN_App_v1._1.RestServiceHelper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System.IO;

namespace KYN_App_v1._1.Controllers
{
    [Authorize]
    [RoutePrefix("Business")]
    public class BusinessController : Controller
    {
        public BusinessController()
        {
        }

        // GET: Business
        [Route("RegisteredBusiness")]
        public ActionResult Index()
        {
            RequestUserBusinessDetailViewModel _userBusinessRequest = new RequestUserBusinessDetailViewModel();

            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            if (UserManager != null)
            {
                var _user = UserManager.FindByEmail(User.Identity.Name);
                if (_user != null)
                {
                    _userBusinessRequest.UserEmail = _user.Email;
                    _userBusinessRequest.UserID = _user.Id;
                }
            }

            IndexBusinessViewModel _model = new IndexBusinessViewModel();


            _model.BusinessPrimaryDetailEntity = ServiceInfo.InvokePostService<IList<ResponseBusinessPrimaryDetailViewModel>>(ServiceInfo.GetBusinessPrimaryDetailList, JsonConvert.SerializeObject(_userBusinessRequest));
            return View(_model);
        }

        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public ActionResult CreateBusinessDetail()
        {
            //var result = UserManager.FindByEmail(User.Identity.Name);

            CreateBusinessViewModel _model = new CreateBusinessViewModel();
            _model.BusinessTypes = GetBusinessTypes;
            _model.BusinessCategories = GetBusinessCategories;
            _model.AddressTypeList = GetAddressTypes;
            _model.BuildingInformationList = new List<BuildingInformationViewModel>();
            _model.BusinessSubCategories = new List<BusinessSubCategoryViewModel>();
            _model.BusinessDetailedCategories = new List<BusinessDetailedCategoryViewModel>();
            _model.BusinessPrimaryDetail = new CreateBusinessPrimaryDetailViewModel();

            //_model.BusinessSubCategories = new List<BusinessSubCategoryViewModel>() { new BusinessSubCategoryViewModel() { CategoryID = 1, ID = 1, SubCategoryName = "test", SubCategoryDescription = "teset" } };
            return View(_model);
        }

        public ActionResult UpdateBusinessDetail(int businessDetailID)
        {
            if (businessDetailID == 0)
            {
                return RedirectToAction("Index");
            }
            ResponseBusinessPrimaryDetailViewModel BusinessPrimaryDetailEntity = ServiceInfo.InvokePostService<ResponseBusinessPrimaryDetailViewModel>(ServiceInfo.GetBusinessPrimaryDetailByID, businessDetailID.ToString());

            CreateBusinessViewModel _model = new CreateBusinessViewModel();
            _model.BusinessTypes = GetBusinessTypes;
            _model.BusinessCategories = GetBusinessCategories;
            _model.BusinessSubCategories = GetBusinessSubCategories(BusinessPrimaryDetailEntity.Category.ID);
            _model.BusinessDetailedCategories = GetBusinessDetailCategories(BusinessPrimaryDetailEntity.SubCategory.ID);
            _model.BuildingInformationList = GetBuildingList(new RequestBuildingInformationViewModel() { AddressTypeID = BusinessPrimaryDetailEntity.AddressTypeID, RegionID = 1 });
            _model.AddressTypeList = GetAddressTypes;
            _model.BusinessPrimaryDetail = new CreateBusinessPrimaryDetailViewModel()
            {
                Name = BusinessPrimaryDetailEntity.Name,
                CategoryID = BusinessPrimaryDetailEntity.Category.ID,
                SubCategoryID = BusinessPrimaryDetailEntity.SubCategory.ID,
                AddressTypeID = BusinessPrimaryDetailEntity.AddressTypeID,
                BusinessTypeID = BusinessPrimaryDetailEntity.BusinessType.ID,
                AnyOtherDetail = BusinessPrimaryDetailEntity.AnyOtherDetail,
                BuildingInformationID = BusinessPrimaryDetailEntity.BuildingInformationID,
                BusinessEmail = BusinessPrimaryDetailEntity.BusinessEmail,
                CanShareAddress = BusinessPrimaryDetailEntity.CanShareAddress,
                CanShareContactDetail = BusinessPrimaryDetailEntity.CanShareContactDetail,
                CanShareEmailAddress = BusinessPrimaryDetailEntity.CanShareEmailAddress,
                //ContactEndTime = Convert.ToDateTime(BusinessPrimaryDetailEntity.ContactEndTime),
                //ContactStartTime = Convert.ToDateTime(BusinessPrimaryDetailEntity.ContactStartTime),
                Description = BusinessPrimaryDetailEntity.Description,
                ID = BusinessPrimaryDetailEntity.ID,
                IsActive = BusinessPrimaryDetailEntity.IsActive,
                IsProfilePublic = BusinessPrimaryDetailEntity.IsProfilePublic,
                OfficialContactNumber1 = BusinessPrimaryDetailEntity.OfficialContactNumber1,
                OfficialContactNumber2 = BusinessPrimaryDetailEntity.OfficialContactNumber2,
                //OperationalSince = Convert.ToDateTime(BusinessPrimaryDetailEntity.OperationalSince),
                Rating = BusinessPrimaryDetailEntity.Rating,
                RegionID = BusinessPrimaryDetailEntity.RegionID,
                UserEmail = BusinessPrimaryDetailEntity.UserEmail,
                UserID = BusinessPrimaryDetailEntity.UserID,
                DetailedCategoryIDs = BusinessPrimaryDetailEntity.DetailedCategories == null ? new List<int>() : BusinessPrimaryDetailEntity.DetailedCategories.Select(c => c.ID).ToList()
            };




            return View(_model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBusinessDetail(CreateBusinessViewModel model)
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            if (UserManager != null)
            {
                var _user = UserManager.FindByEmail(User.Identity.Name);
                if (_user != null)
                {
                    model.BusinessPrimaryDetail.UserID = _user.Id;
                    model.BusinessPrimaryDetail.UserEmail = _user.Email;
                    model.BusinessPrimaryDetail.RegionID = 1;
                    //model.BusinessPrimaryDetail.OperationalSince = DateTime.Now;
                    //model.BusinessPrimaryDetail.ContactStartTime = DateTime.Now;
                    //model.BusinessPrimaryDetail.ContactEndTime = DateTime.Now;
                }
            }

            var _CreatedBusinessID = 0;

            if (model.BusinessPrimaryDetail.ID > 0)
            {
                bool _isUpdated = ServiceInfo.InvokePostService<bool>(ServiceInfo.UpdateBusinessPrimaryDetail, JsonConvert.SerializeObject(model.BusinessPrimaryDetail));
                _CreatedBusinessID = model.BusinessPrimaryDetail.ID;
            }
            else
            {
                _CreatedBusinessID = ServiceInfo.InvokePostService<int>(ServiceInfo.CreateBusinessPrimaryDetail, JsonConvert.SerializeObject(model.BusinessPrimaryDetail));
            }
            foreach (string upload in Request.Files)
            {
                if (!string.IsNullOrEmpty(Request.Files[upload].FileName) && _CreatedBusinessID > 0)
                {
                    string _path = AppDomain.CurrentDomain.BaseDirectory + "Images\\Business_Images\\" + _CreatedBusinessID;

                    if (!Directory.Exists(_path))
                    {
                        var DirectoryInfo = Directory.CreateDirectory(_path);
                    }

                    string path = AppDomain.CurrentDomain.BaseDirectory + "Images\\Business_Images\\" + _CreatedBusinessID;
                    //string filename = Path.GetFileName(Request.Files[upload].FileName);
                    string filename = "Main.jpg";
                    Request.Files[upload].SaveAs(Path.Combine(path, filename));
                    string ImagePath = filename;
                    string ImageName = Request.Form["ImageName"];

                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult FillBusinessSubCategories(int businessCategoryID)
        {
            return Json(GetBusinessSubCategories(businessCategoryID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillBusinessDetailedCategories(int subCategoryID)
        {
            return Json(GetBusinessDetailCategories(subCategoryID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillBuildingInformation(int addressTypeID)
        {
            return Json(GetBuildingList(new RequestBuildingInformationViewModel() { RegionID = 1, AddressTypeID = addressTypeID }), JsonRequestBehavior.AllowGet);
        }

        private IList<BusinessTypeViewModel> GetBusinessTypes
        {
            get
            {
                return ServiceInfo.InvokeGetService<List<BusinessTypeViewModel>>(ServiceInfo.GetBusinessTypes, string.Empty);
            }
        }

        private IList<BusinessCategoryViewModel> GetBusinessCategories
        {
            get
            {
                return ServiceInfo.InvokeGetService<List<BusinessCategoryViewModel>>(ServiceInfo.GetBusinessCategories, string.Empty);
            }
        }

        private IList<AddressTypeViewModel> GetAddressTypes
        {
            get
            {
                return ServiceInfo.InvokeGetService<List<AddressTypeViewModel>>(ServiceInfo.GetAddressTypes, string.Empty);
            }
        }

        private IList<BuildingInformationViewModel> GetBuildingList(RequestBuildingInformationViewModel requestBuildingInfo)
        {
            return ServiceInfo.InvokePostService<List<BuildingInformationViewModel>>(ServiceInfo.GetListOfBuilding, JsonConvert.SerializeObject(requestBuildingInfo));
        }

        private IList<BusinessSubCategoryViewModel> GetBusinessSubCategories(int categoryID)
        {
            return ServiceInfo.InvokePostService<List<BusinessSubCategoryViewModel>>(ServiceInfo.GetBusinessSubCategories, categoryID.ToString());
        }

        private IList<BusinessDetailedCategoryViewModel> GetBusinessDetailCategories(int subCategoryID)
        {
            return ServiceInfo.InvokePostService<List<BusinessDetailedCategoryViewModel>>(ServiceInfo.GetBusinessDetailedCategories, subCategoryID.ToString());
        }
    }
}