using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using KYN_App_v1._1.HelperViewModels;
using System.IO;

namespace KYN_App_v1._1.Models
{

    public class BusinessDetailedUserRatingViewModel
    {
        public long ID { get; set; }
        public long BusinessPrimaryDetailID { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }
        public string UserID { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public int UserRating { get; set; }
        public string UserComment { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public bool IsUserRatingChanged { get; set; }
    }

    public class IndexBusinessViewModel : BaseViewModel
    {
        public IList<ResponseBusinessPrimaryDetailViewModel> BusinessPrimaryDetailEntity { get; set; }
    }

    public class CreateBusinessViewModel : BaseViewModel
    {
        [Required]
        public IEnumerable<BusinessTypeViewModel> BusinessTypes { get; set; }
        [Required]
        public IList<BusinessCategoryViewModel> BusinessCategories { get; set; }
        [Required]
        public IList<BusinessSubCategoryViewModel> BusinessSubCategories { get; set; }
        [Required]
        public IList<BusinessDetailedCategoryViewModel> BusinessDetailedCategories { get; set; }
        [Required]
        public IList<AddressTypeViewModel> AddressTypeList { get; set; }
        [Required]
        public IList<BuildingInformationViewModel> BuildingInformationList { get; set; }
        [Required]
        public CreateBusinessPrimaryDetailViewModel BusinessPrimaryDetail { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public string file { get; set; }
    }


    public class BusinessTypeViewModel
    {
        public int ID { get; set; }
        public string BusinessTypeName { get; set; }
        public string BusinessTypeDescription { get; set; }
        public string BusinessTypeDetail { get; set; }
    }

    public class BusinessCategoryViewModel
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public string Details { get; set; }
        public string ImageUrl { get; set; }
        public string DisplayName { get; set; }
        public int SortIndex { get; set; }
    }

    public class SubCategoryInformationViewModel : BaseViewModel
    {
        public int CategorID { get; set; }
        public string CategorName { get; set; }
        public string SubCategoryImageName { get { return string.Concat(CategorName, ".jpg"); } }
        public IList<BusinessSubCategoryViewModel> SubCategories { get; set; }
    }
    public class BusinessSubCategoryViewModel : BaseViewModel
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryDescription { get; set; }
        public string ImagePath { get { return SubCategoryName + ".jpg"; } }
        public string DisplayName { get; set; }
        public int SortIndex { get; set; }
    }

    public class BusinessDetailedCategoryViewModel
    {
        public int ID { get; set; }
        public int SubCategoryID { get; set; }
        public string DetailedCategoryName { get; set; }
        public string DetailedCategoryDescription { get; set; }
        public string DisplayName { get; set; }
        public int SortIndex { get; set; }
    }

    public class RequestUserBusinessDetailViewModel
    {
        public string UserEmail { get; set; }
        public string UserID { get; set; }
    }

    public class ResponseObject : BaseViewModel
    {
        public IList<BusinessDetailedCategoryViewModel> DetailedCategories { get; set; }
        public List<ResponseBusinessPrimaryDetailViewModel> Businesses { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
    }

    public class ResponseBusinessPrimaryDetailViewModel : BaseViewModel
    {
        public int ID { get; set; }

        public int RegionID { get; set; }

        public double Rating { get; set; }

        public double CalculatedRating
        {
            get
            {
                double _rating = 3;
                if (UserReviews != null && UserReviews.Count > 0)
                {
                    _rating = Math.Round((double)(Rating + UserReviews.Sum(c => c.UserRating)) / UserReviews.Count, 1);
                }
                return _rating;
            }
        }

        public bool IsActive { get; set; }

        public string UserEmail { get; set; }

        public string UserID { get; set; }

        [Display(Name = "Business Type")]
        public BusinessTypeViewModel BusinessType { get; set; }

        [Display(Name = "Business Category")]
        public BusinessCategoryViewModel Category { get; set; }

        [Display(Name = "Business Sub Category")]
        public BusinessSubCategoryViewModel SubCategory { get; set; }

        [Display(Name = "Business Detailed Categories")]
        public IList<BusinessDetailedCategoryViewModel> DetailedCategories { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Operating Since")]
        public DurationCapture StartedOn { get; set; }

        [Display(Name = "Operational Start Time")]
        public WorkingHoursCapture From { get; set; }

        [Display(Name = "Operational End Time")]
        public WorkingHoursCapture To { get; set; }

        [Display(Name = "Official Contact Number-Primary")]
        public string OfficialContactNumber1 { get; set; }

        [Display(Name = "Official Contact Number-Secondary")]
        public string OfficialContactNumber2 { get; set; }

        [Display(Name = "Business Email")]
        public string BusinessEmail { get; set; }

        [Display(Name = "share contact Numbers")]
        public bool CanShareContactDetail { get; set; }

        [Display(Name = "Share Email Address")]
        public bool CanShareEmailAddress { get; set; }

        [Display(Name = "Share Address")]
        public bool CanShareAddress { get; set; }

        [Display(Name = "Is Profile Public")]
        public bool IsProfilePublic { get; set; }

        [Display(Name = "Address Type")]
        public int AddressTypeID { get; set; }

        [Display(Name = "Building Information")]
        public int BuildingInformationID { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Detail")]
        public string AnyOtherDetail { get; set; }

        public string AddressTypeName { get { return AddressTypeInfo.AddressTypeName; } }
        public string BuildingName { get { return BuildingInformation.BuildingName; } }

        public IList<string> ImageSliders
        {
            get
            {
                IList<string> _filePath = new List<string>();
                IList<string> _temp = new List<string>();
                //if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID))
                if(Directory.Exists(HttpContext.Current.Server.MapPath("~/Images/Business_Images/" + ID)))
                {
                    //var Result = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID);
                    var Result = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Images/Business_Images/" + ID));
                    if (Result != null)
                    {
                        _filePath = Result.ToList();
                    }
                }

                if (_filePath == null || _filePath.Count == 0)
                {
                    _filePath.Add(HttpContext.Current.Server.MapPath("~/Images/Business_Images/No_Image.jpg"));
                }
                else
                {
                    foreach (var item in _filePath)
                    {
                        _temp.Add("/Images/Business_Images/" + ID + "/" + Path.GetFileName(item));
                    }
                    _filePath = _temp;
                    //var _FileName = Path.GetFileName(_filePath);
                    //return "../Images/Business_Images/" + ID + "/" + _FileName;
                }

                return _filePath;
            }
        }

        public string ImagePath
        {
            get
            {
                string _filePath = string.Empty;

                //if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID))
                if(Directory.Exists(HttpContext.Current.Server.MapPath("~/Images/Business_Images/" + ID)))
                {
                    //var Result = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID);
                    var Result = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Images/Business_Images/" + ID));

                    foreach (var file in Result)
                    {
                        if (Path.GetFileName(file) == "Main.jpg")
                        {
                            _filePath = "Main.jpg";
                        }
                    }
                    //_filePath = Result == null ? string.Empty : Result.Count() == 0 ? string.Empty : Result[0];
                }

                if (string.IsNullOrEmpty(_filePath))
                {
                    return "/Images/Business_Images/No_Image.jpg";
                }
                else
                {
                    var _FileName = Path.GetFileName(_filePath);
                    return "/Images/Business_Images/" + ID + "/" + _FileName;
                }
            }
        }

        //public string ImagePath
        //{
        //    get
        //    {
        //        string _filePath = string.Empty;
        //        if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID))
        //        {
        //            var Result = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID);
        //            _filePath = Result == null ? string.Empty : Result.Count() == 0 ? string.Empty : Result[0];
        //        }

        //        if (string.IsNullOrEmpty(_filePath))
        //        {
        //            return "../Images/Business_Images/No_Image.jpg";
        //        }
        //        else
        //        {
        //            var _FileName= Path.GetFileName(_filePath);
        //            return "../Images/Business_Images/" + ID + "/" + _FileName;
        //        }
        //    }
        //}

        public BuildingInformationViewModel BuildingInformation { get; set; }
        public AddressTypeViewModel AddressTypeInfo { get; set; }
        public IList<BusinessDetailedUserRatingViewModel> UserReviews { get; set; }
    }

    public enum Months
    {
        NOT_DEFINED = 0, January = 1, Febuary = 2, March = 3, April = 4, May = 5, June = 6, July = 7, August = 8, September = 9, October = 10, November = 11, December = 12
    }

    public enum Days
    {
        NOT_DEFINED = 0, Monday = 1, Tuesday = 2, Wednesday = 3, Thursday = 4, Friday = 5, Saturday = 6, Sunday = 7
    }

    public enum TimePeriods
    {
        NOT_DEFINED = 0, AM = 1, PM = 2
    }

    public class DurationCapture
    {
        public Months StartedMonth { get; set; }
        public int Year { get; set; }
    }

    public class WorkingHoursCapture
    {
        public double Time { get; set; }
        public TimePeriods TimePeriod { get; set; }
    }

    public class CreateBusinessPrimaryDetailViewModel
    {
        public int ID { get; set; }

        public int RegionID { get; set; }

        public double Rating { get; set; }

        public bool IsActive { get; set; }

        public string UserEmail { get; set; }

        public string UserID { get; set; }

        [Display(Name = "Business Type")]
        public int BusinessTypeID { get; set; }

        [Display(Name = "Business Category")]
        public int CategoryID { get; set; }

        [Display(Name = "Business Sub Category")]
        public int SubCategoryID { get; set; }

        [Display(Name = "Business Detailed Category")]
        public IList<int> DetailedCategoryIDs { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Operating Since")]
        public DurationCapture StartedOn { get; set; }

        [Display(Name = "Operational Start Time")]
        public WorkingHoursCapture From { get; set; }

        [Display(Name = "Operational End Time")]
        public WorkingHoursCapture To { get; set; }

        [Display(Name = "Official Contact Number-Primary")]
        public string OfficialContactNumber1 { get; set; }

        [Display(Name = "Official Contact Number-Secondary")]
        public string OfficialContactNumber2 { get; set; }

        [Display(Name = "Business Email")]
        public string BusinessEmail { get; set; }

        [Display(Name = "share contact Numbers")]
        public bool CanShareContactDetail { get; set; }

        [Display(Name = "Share Email Address")]
        public bool CanShareEmailAddress { get; set; }

        [Display(Name = "Share Address")]
        public bool CanShareAddress { get; set; }

        [Display(Name = "Is Profile Public")]
        public bool IsProfilePublic { get; set; }

        [Display(Name = "Address Type")]
        public int AddressTypeID { get; set; }

        [Display(Name = "Building Information")]
        public int BuildingInformationID { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Detail")]
        public string AnyOtherDetail { get; set; }

    }
}