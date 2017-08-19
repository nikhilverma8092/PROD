
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KYN_App_v1._1.Models;
using KYN_App_v1._1.RestServiceHelper;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace KYN_App_v1._1.Controllers
{
    [RoutePrefix("Ghaziabad/Crossings")]
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            SearchViewModel _model = new SearchViewModel();
            _model.Categories = GetValidCategoryData();
            return View(_model);
        }

        [HttpPost]
        public JsonResult Index(string Prefix)
        {
            IList<BaseViewModel> ObjList = (IList<BaseViewModel>)Session["SearchKeywords"];
            if (ObjList == null)
            {
                ObjList = ServiceInfo.InvokeGetService<IList<BaseViewModel>>(ServiceInfo.GetListOfSearchKeywords, string.Empty);
                Session.Add("SearchKeywords", ObjList);
            }

            if (Prefix.Length < 3)
                return Json(string.Empty, JsonRequestBehavior.AllowGet);

            //Searching records from list using LINQ query
            var CityName = (from N in ObjList
                            where N.Name.ToUpper().Trim().Contains(Prefix.ToUpper())
                            select new { N.FullName });
            return Json(CityName, JsonRequestBehavior.AllowGet);
        }

        [Route("")]
        public ActionResult Index1()
        {
            SearchViewModel _model = new SearchViewModel();
            _model.Categories = GetValidCategoryData();
            return View("Index",_model);
        }

        [HttpPost]
        public ActionResult SearchResultByKeyword(BaseViewModel model)
        {
            IList<BaseViewModel> ObjList = (IList<BaseViewModel>)Session["SearchKeywords"];
            if (ObjList == null)
            {
                ObjList = ServiceInfo.InvokeGetService<IList<BaseViewModel>>(ServiceInfo.GetListOfSearchKeywords, string.Empty);
                Session.Add("SearchKeywords", ObjList);
            }

            var _selectedItemList = (from N in ObjList
                            where N.FullName.ToUpper().Trim().Contains(model.FullName.ToUpper())
                            select N).FirstOrDefault();
            if (_selectedItemList != null)
            {
                if (_selectedItemList.ItemType == "Category")
                {
                    //return RedirectToAction("get" + _selectedItemList.Name);
                    //return RedirectToAction("GetMoreSubCategories", new { categoryName=_selectedItemList.Name });
                    return RedirectToActionPermanent("GetMoreSubCategories", new { categoryName = _selectedItemList.Name });
                }
                else if (_selectedItemList.ItemType == "SubCategory")
                {
                    return RedirectToActionPermanent("SearchBusinessDetailBySubCategory", new { categoryName = _selectedItemList.CategoryName, subCategoryName = _selectedItemList.SubCategoryName });
                }
                else if (_selectedItemList.ItemType == "DetailedCategory")
                {
                    return RedirectToActionPermanent("GetSearchBusinessInformationByDetailCategoryID", new { categoryName = _selectedItemList.CategoryName, subCategoryName = _selectedItemList.SubCategoryName, detailCategoryID = _selectedItemList.ItemID });
                }
                else if (_selectedItemList.ItemType == "BusinessPrimaryDetail")
                {
                    return RedirectToActionPermanent("GetBusinessDetailByID", new { categoryName = _selectedItemList.CategoryName, subCategoryName = _selectedItemList.SubCategoryName, BusinessName = _selectedItemList.Name.Replace(' ', '-').Replace('&', '-'), businessID = _selectedItemList.ItemID });
                }
                //else if (_selectedItemList.ItemType == "SubCategory")
                //{
                //    //return RedirectToRoutePermanent("/ghaziabad/crossings/" + _selectedItemList.Name);
                //    //return RedirectToActionPermanent("SearchBusinessDetailBySubCategory", new { categoryName = _selectedItemList.FullName });
                //    //return View("SearchBusinessResult", GetBusinessBySubcategoryID(_selectedItemList.ItemID));
                //    return RedirectToAction("SearchBusinessDetailBySubCategory", new { categoryName = _selectedItemList.Name, subCategoryName = _selectedItemList.Name });
                //}
                //else if (_selectedItemList.ItemType == "DetailedCategory")
                //{
                //    return View("SearchBusinessResult", GetSearchBusinessByDetailCategoryID(_selectedItemList.ItemID));
                //}
                //else if (_selectedItemList.ItemType == "BusinessPrimaryDetail")
                //{
                //    return View("BusinessDetail", GetBusinessDetailByID(_selectedItemList.ItemID));
                //}
            }

            return RedirectToAction("Index");
        }

        [Route("{categoryName}")]
        public ActionResult GetMoreSubCategories(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                return RedirectToAction("Index");
            }

            //int categoryID = GetValidData().Where(c => c.BusinessCategory.CategoryName.ToUpper() == categoryName.ToUpper()).Select(c => c.BusinessCategory.ID).FirstOrDefault();
            int categoryID = GetValidCategoryData().Where(c => c.BusinessCategory.CategoryName.ToUpper() == categoryName.ToUpper()).Select(c => c.BusinessCategory.ID).FirstOrDefault();
            IList<BusinessSubCategoryViewModel> BusinessPrimaryDetailEntity = GetValidSubCategories(categoryName, categoryID);
            SubCategoryInformationViewModel _modelObject = new SubCategoryInformationViewModel()
            {
                CategorID = categoryID,
                CategorName = categoryName,
                SubCategories = BusinessPrimaryDetailEntity
            };
            //IList<BusinessSubCategoryViewModel> BusinessPrimaryDetailEntity = ServiceInfo.InvokePostService<IList<BusinessSubCategoryViewModel>>(ServiceInfo.GetAllSubCategories, categoryID.ToString());
            //SubCategoryInformationViewModel _modelObject = new SubCategoryInformationViewModel()
            //{ CategorID = categoryID, CategorName = categoryName, SubCategories = BusinessPrimaryDetailEntity };
            return View("SubCategoryInformation", _modelObject);
        }

        [Route("{categoryName}/{subCategoryName}")]
        public ActionResult SearchBusinessDetailBySubCategory(string categoryName, string subCategoryName)
        {
            if (string.IsNullOrEmpty(subCategoryName))
            {
                return RedirectToAction("Index");
            }
            //IList<SearchCategoryViewMode> _LoadData = Session["SearchCategoryViewModelList"] as IList<SearchCategoryViewMode>;
            //int subCategoryID = 0;
            //IList<BusinessSubCategoryViewModel> _subCategories = GetValidData().Where(c => c.BusinessCategory.CategoryName.ToUpper() == categoryName.ToUpper()).Select(c => c.SubCategories).FirstOrDefault();
            //if (_subCategories != null && _subCategories.Count > 0)
            //{
            //    subCategoryID = _subCategories.Where(c => c.SubCategoryName.ToUpper() == subCategoryName.ToUpper()).Select(k => k.ID).FirstOrDefault();
            //}
            //ResponseObject BusinessPrimaryDetailEntity = ServiceInfo.InvokePostService<ResponseObject>(ServiceInfo.GetAllDetailedAndResult, subCategoryID.ToString());
            //BusinessPrimaryDetailEntity.CategoryName = categoryName;
            //BusinessPrimaryDetailEntity.SubCategoryName = subCategoryName;
            //return View("SearchBusinessResult", BusinessPrimaryDetailEntity);
            ResponseObject BusinessPrimaryDetailEntity = GetValidBusinessesBySubcategory(categoryName, subCategoryName);
            BusinessPrimaryDetailEntity.CategoryName = categoryName;
            BusinessPrimaryDetailEntity.SubCategoryName = subCategoryName;
            return View("SearchBusinessResult", BusinessPrimaryDetailEntity);
        }

        //private ResponseObject GetBusinessBySubcategoryID(int subCategoryID)
        //{
        //    ResponseObject BusinessPrimaryDetailEntity = ServiceInfo.InvokePostService<ResponseObject>(ServiceInfo.GetAllDetailedAndResult, subCategoryID.ToString());
        //    if(BusinessPrimaryDetailEntity!=null && BusinessPrimaryDetailEntity.Businesses!=null&& BusinessPrimaryDetailEntity.Businesses.Count>0)
        //    {
        //        BusinessPrimaryDetailEntity.CategoryName = BusinessPrimaryDetailEntity.Businesses[0].Category.CategoryName;
        //        BusinessPrimaryDetailEntity.SubCategoryName = BusinessPrimaryDetailEntity.Businesses[0].SubCategory.Name;
        //    }
        //    return BusinessPrimaryDetailEntity;
        //}

        [Route("{categoryName}/{subCategoryName}/{detailCategoryID}")]
        public ActionResult GetSearchBusinessInformationByDetailCategoryID(string categoryName, string subCategoryName, int detailCategoryID)
        {
            if (detailCategoryID <= 0)
            {
                return RedirectToAction("Index");
            }

            //List<ResponseBusinessPrimaryDetailViewModel> BusinessPrimaryDetailEntity = ServiceInfo.InvokePostService<List<ResponseBusinessPrimaryDetailViewModel>>(ServiceInfo.GetSearchBusinessInformationByDetailCategoryID, detailCategoryID.ToString());
            //ResponseObject _finalData = new ResponseObject() { Businesses = BusinessPrimaryDetailEntity, DetailedCategories = new List<BusinessDetailedCategoryViewModel>() };
            //return View("SearchBusinessResult", _finalData);
            ResponseObject _finalData = GetValidBusinessesByDetailcategory(categoryName, subCategoryName, detailCategoryID);
            return View("SearchBusinessResult", _finalData);
        }

        //private ResponseObject GetSearchBusinessByDetailCategoryID(int detailCategoryID)
        //{
        //    List<ResponseBusinessPrimaryDetailViewModel> BusinessPrimaryDetailEntity = ServiceInfo.InvokePostService<List<ResponseBusinessPrimaryDetailViewModel>>(ServiceInfo.GetSearchBusinessInformationByDetailCategoryID, detailCategoryID.ToString());
        //    ResponseObject _finalData = new ResponseObject() { Businesses = BusinessPrimaryDetailEntity, DetailedCategories = new List<BusinessDetailedCategoryViewModel>() };
        //    return _finalData;
        //}

        [Route("{categoryName}/{subCategoryName}/{BusinessName}/{businessID}")]
        public ActionResult GetBusinessDetailByID(string categoryName, string subCategoryName, string BusinessName, int businessID)
        {
            if (businessID <= 0)
            {
                return RedirectToAction("Index");
            }

            //ResponseBusinessPrimaryDetailViewModel BusinessPrimaryDetailEntity = ServiceInfo.InvokePostService<ResponseBusinessPrimaryDetailViewModel>(ServiceInfo.GetBusinessPrimaryDetailByID, businessID.ToString());
            //BusinessPrimaryDetailEntity.UserReviews = ServiceInfo.InvokePostService<IList<BusinessDetailedUserRatingViewModel>>(ServiceInfo.GetAllBusinessUserRating, businessID.ToString());
            //Session.Add(businessID, BusinessPrimaryDetailEntity);
            //return View("BusinessDetail", BusinessPrimaryDetailEntity);
            ResponseBusinessPrimaryDetailViewModel BusinessPrimaryDetailEntity = GetValidBusinessesByID(categoryName, subCategoryName, businessID);
            BusinessPrimaryDetailEntity.UserReviews = ServiceInfo.InvokePostService<IList<BusinessDetailedUserRatingViewModel>>(ServiceInfo.GetAllBusinessUserRating, businessID.ToString());
            return View("BusinessDetail", BusinessPrimaryDetailEntity);
        }

        [Route("{categoryName}/{subCategoryName}/{BusinessName}/{businessID}/Gallery")]
        public ActionResult OpenGallery(string categoryName, string subCategoryName, string BusinessName, int businessID)
        {
            if (businessID <= 0)
            {
                return RedirectToAction("Index");
            }

            //ResponseBusinessPrimaryDetailViewModel BusinessPrimaryDetailEntity = (ResponseBusinessPrimaryDetailViewModel)Session[businessID];
            //if (BusinessPrimaryDetailEntity == null)
            //{
            //    BusinessPrimaryDetailEntity = ServiceInfo.InvokePostService<ResponseBusinessPrimaryDetailViewModel>(ServiceInfo.GetBusinessPrimaryDetailByID, businessID.ToString());
            //}
            //return View("BusinessGallery", BusinessPrimaryDetailEntity);
            ResponseBusinessPrimaryDetailViewModel BusinessPrimaryDetailEntity = GetValidBusinessesByID(categoryName, subCategoryName, businessID);
            if (BusinessPrimaryDetailEntity == null)
            {
                BusinessPrimaryDetailEntity = ServiceInfo.InvokePostService<ResponseBusinessPrimaryDetailViewModel>(ServiceInfo.GetBusinessPrimaryDetailByID, businessID.ToString());
            }
            return View("BusinessGallery", BusinessPrimaryDetailEntity);
        }

        public ResponseBusinessPrimaryDetailViewModel GetBusinessDetailByID(int businessID)
        {
            ResponseBusinessPrimaryDetailViewModel BusinessPrimaryDetailEntity = ServiceInfo.InvokePostService<ResponseBusinessPrimaryDetailViewModel>(ServiceInfo.GetBusinessPrimaryDetailByID, businessID.ToString());
            BusinessPrimaryDetailEntity.UserReviews = ServiceInfo.InvokePostService<IList<BusinessDetailedUserRatingViewModel>>(ServiceInfo.GetAllBusinessUserRating, businessID.ToString());
            return BusinessPrimaryDetailEntity;
        }

        public ActionResult CreateUserReview(string comment, int rating, int businessID)
        {
            BusinessDetailedUserRatingViewModel _data = new BusinessDetailedUserRatingViewModel()
            {
                BusinessPrimaryDetailID = businessID,
                CategoryID = 1,
                SubCategoryID = 1,
                IsUserRatingChanged = true,
                UserComment = comment,
                UserRating = rating,
                UserEmail = "nikhilvermacse@gmail.com",
                UserID = "userID",
                UserName = "Crossfriend User"
            };

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            if (UserManager != null)
            {
                var _user = UserManager.FindByEmail(User.Identity.Name);
                if (_user != null)
                {
                    _data.UserEmail = _user.Email;
                    _data.UserID = _user.Id;
                    _data.UserName = _user.FirstName + " "+_user.LastName;
                }
            }

            

            var result = ServiceInfo.InvokePostService<IList<BusinessDetailedUserRatingViewModel>>(ServiceInfo.CreateBusinessDetailUserRating, JsonConvert.SerializeObject(_data));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //private IList<SearchCategoryViewMode> GetValidData()
        //{
        //    IList<SearchCategoryViewMode> _LoadData = Session["SearchCategoryViewModelList"] as IList<SearchCategoryViewMode>;
        //    int _counter = Session["Counter"]==null?0: (int)Session["Counter"];

        //    if (_LoadData == null || _counter > 10)
        //    {
        //        _counter = 0;
        //        Session.RemoveAll();
        //        _LoadData = ServiceInfo.InvokeGetService<IList<SearchCategoryViewMode>>(ServiceInfo.GetAllSearchBusinessParentData, string.Empty);
        //        Session.Add("SearchCategoryViewModelList", _LoadData);
        //        Session.Add("Counter", 0);
        //    }
        //    Session["Counter"] = _counter + 1;
        //    return _LoadData;
        //}

        private IList<SearchCategoryViewMode> GetValidCategoryData()
        {
            IList<SearchCategoryViewMode> _LoadData = Session["SearchCategoryViewModelList"] as IList<SearchCategoryViewMode>;
            if (_LoadData == null)
            {
                _LoadData = ServiceInfo.InvokeGetService<IList<SearchCategoryViewMode>>(ServiceInfo.GetAllSearchBusinessParentData, string.Empty);
                Session.Add("SearchCategoryViewModelList", _LoadData);
            }
            return _LoadData;
        }

        private IList<BusinessSubCategoryViewModel> GetValidSubCategories(string categoryName, int categoryID)
        {
            IList<BusinessSubCategoryViewModel> _subCategoryData = Session[categoryName + "_SubCategories"] as IList<BusinessSubCategoryViewModel>;
            if (_subCategoryData == null)
            {
                _subCategoryData = ServiceInfo.InvokePostService<IList<BusinessSubCategoryViewModel>>(ServiceInfo.GetAllSubCategories, categoryID.ToString());
                Session.Add(categoryName + "_SubCategories", _subCategoryData);
            }
            return _subCategoryData;
        }

        private ResponseObject GetValidBusinessesBySubcategory(string categoryName, string subCategoryName)
        {
            ResponseObject BusinessPrimaryDetailEntity = Session[categoryName + "_" + subCategoryName + "_Business"] as ResponseObject;
            if (BusinessPrimaryDetailEntity == null)
            {
                IList<SearchCategoryViewMode> _categoryList = GetValidCategoryData();
                int categoryID = _categoryList.Where(c => c.BusinessCategory.CategoryName.ToUpper() == categoryName.ToUpper()).Select(k => k.BusinessCategory.ID).FirstOrDefault();
                var subCategoryID = 0;
                if (categoryID > 0)
                {
                    var _subcategories = GetValidSubCategories(categoryName, categoryID);
                    subCategoryID = _subcategories.Where(c => c.SubCategoryName.ToUpper() == subCategoryName.ToUpper()).Select(k => k.ID).FirstOrDefault();
                }

                if (categoryID > 0 && subCategoryID > 0)
                {
                    BusinessPrimaryDetailEntity = ServiceInfo.InvokePostService<ResponseObject>(ServiceInfo.GetAllDetailedAndResult, subCategoryID.ToString());
                    Session.Add(categoryName + "_" + subCategoryName + "_Business", BusinessPrimaryDetailEntity);
                }
            }
            return BusinessPrimaryDetailEntity;
        }

        private ResponseObject GetValidBusinessesByDetailcategory(string categoryName, string subCategoryName, int detailedCategoryID)
        {
            ResponseObject BusinessPrimaryDetailEntity = GetValidBusinessesBySubcategory(categoryName, subCategoryName);

            if (BusinessPrimaryDetailEntity != null && detailedCategoryID > 0)
            {
                BusinessPrimaryDetailEntity.Businesses = BusinessPrimaryDetailEntity.Businesses.Where(q => q.DetailedCategories.Any(c => c.ID == detailedCategoryID)).ToList();
            }

            return BusinessPrimaryDetailEntity;
        }

        private ResponseBusinessPrimaryDetailViewModel GetValidBusinessesByID(string categoryName, string subCategoryName, int businessID)
        {
            ResponseObject BusinessPrimaryDetailEntity = GetValidBusinessesBySubcategory(categoryName, subCategoryName);

            //if (BusinessPrimaryDetailEntity != null && businessID > 0)
            //{
            //    BusinessPrimaryDetailEntity.Businesses = BusinessPrimaryDetailEntity.Businesses.Where(q => q.ID == businessID).ToList();
            //}

            ResponseBusinessPrimaryDetailViewModel _validBusiness = BusinessPrimaryDetailEntity.Businesses.Where(c => c.ID == businessID).FirstOrDefault();

            if (_validBusiness == null || !_validBusiness.IsValid)
            {
                _validBusiness = ServiceInfo.InvokePostService<ResponseBusinessPrimaryDetailViewModel>(ServiceInfo.GetBusinessPrimaryDetailByID, businessID.ToString());
            }

            return _validBusiness;
        }
    }
}