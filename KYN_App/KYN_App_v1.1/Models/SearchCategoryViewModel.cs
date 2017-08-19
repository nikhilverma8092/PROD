using KYN_App_v1._1.RestServiceHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace KYN_App_v1._1.Models
{
    public class SearchCategoryViewMode
    {
        public BusinessCategoryViewModel BusinessCategory { get; set; }
        public IList<BusinessSubCategoryViewModel> SubCategories { get; set; }
    }

    public class SearchViewModel : BaseViewModel
    {
        [Display(Name = "Business Category")]
        public int CategoryID { get; set; }

        [Display(Name = "Business Categories")]
        public IList<BusinessCategoryViewModel> CategoryList { get; set; }


        [Display(Name = "Business Sub Category")]
        public int SubCategoryID { get; set; }

        [Display(Name = "Business Categories")]
        public IList<BusinessSubCategoryViewModel> SubCategoryList { get; set; }


        [Display(Name = "Business Detail ID")]
        public int BusinessDetailID { get; set; }

        [Display(Name = "Business Categories")]
        public IList<BusinessDetailedCategoryViewModel> BusinessDetailList { get; set; }


        public IList<SearchCategoryViewMode> Categories { get; set; }

        public IList<string> ImageNames
        {
            get
            {

                IList<string> _filePath = new List<string>();
                IList<string> _temp = new List<string>();
                //if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID))
                if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Images/Promotional")))
                {
                    //var Result = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID);
                    var Result = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Images/Promotional"));
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
                        _temp.Add(Path.GetFileName(item));
                    }
                    _filePath = _temp;
                    //var _FileName = Path.GetFileName(_filePath);
                    //return "../Images/Business_Images/" + ID + "/" + _FileName;
                }

                return _filePath;

                //IList<string> _imageList = new List<string>();
                //_imageList.Add("slider-1.jpg");
                //_imageList.Add("slider-2.jpg");
                //_imageList.Add("slider-3.jpg");
                //_imageList.Add("image-slider-1.jpg");
                //_imageList.Add("image-slider-2.jpg");
                //_imageList.Add("image-slider-3.jpg");
                ////_imageList.Add("slider-4.jpg");

                //return _imageList;
            }
        }
    }
}