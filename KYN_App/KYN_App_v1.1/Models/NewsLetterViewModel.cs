using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace KYN_App_v1._1.Models
{
    public class UserBlogViewModel
    {
        public long ID { get; set; }
        public string UserID { get; set; }
        public string UserEmail { get; set; }
        public string Name { get; set; }
        public string BlogTitle { get; set; }
        public string BlogHeadLine { get; set; }
        public string BlogDetail { get; set; }
        public string CreatedOn { get; set; }

        public IList<string> ImageSliders
        {
            get
            {
                IList<string> _filePath = new List<string>();
                IList<string> _temp = new List<string>();
                //if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID))
                if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Images/Blog_Images/1")))
                {
                    //var Result = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID);
                    var Result = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Images/Blog_Images/1"));
                    if (Result != null)
                    {
                        _filePath = Result.ToList();
                    }
                }

                if (_filePath == null || _filePath.Count == 0)
                {
                    _filePath.Add(HttpContext.Current.Server.MapPath("~/Images/Blog_Images/No_Image.jpg"));
                }
                else
                {
                    foreach (var item in _filePath)
                    {
                        _temp.Add("/Images/Blog_Images/1/" + Path.GetFileName(item));
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
                if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Images/Blog_Images/" + ID)))
                {
                    var Result = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Images/Blog_Images/" + ID));

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
                    return "../Images/Blog_Images/No_Image.jpg";
                }
                else
                {
                    var _FileName = Path.GetFileName(_filePath);
                    return "../Images/Blog_Images/" + ID + "/" + _FileName;
                }
            }
        }
    }

    public class UserSupportViewModel
    {
        public int ID { get; set; }
        public long BlogID { get; set; }
        public string UserID { get; set; }
        public string UserEmail { get; set; }
        public string Name { get; set; }
        public bool? InSupport { get; set; }
        public string PostedOn { get; set; }
    }

    public class UserCommentsViewModel
    {
        public int ID { get; set; }
        public long BlogID { get; set; }
        public string UserID { get; set; }
        public string UserEmail { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string PostedOn { get; set; }
    }

    public class NewsLetterViewModel
    {
        public long ID { get; set; }
        public string UserID { get; set; }
        public string UserEmail { get; set; }
        public string Name { get; set; }
        public string BlogTitle { get; set; }
        public string BlogHeadLine { get; set; }
        public string BlogDetail { get; set; }
        public string CreatedOn { get; set; }
        public string NewComment { get; set; }
        public IList<UserCommentsViewModel> PostedComments { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string InSupportName { get; set; }

        [Required]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]

        public string InSupportMobileNumber { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string InSupportEmailID { get; set; }

        public bool? InSupport { get; set; }

        [Display(Name ="Total poster comments")]
        public IList<UserSupportViewModel> PostedSupports { get; set; }

        public IList<string> ImageSliders
        {
            get
            {
                IList<string> _filePath = new List<string>();
                IList<string> _temp = new List<string>();
                //if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID))
                if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Images/Blog_Images/" + ID)))
                {
                    //var Result = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID);
                    var Result = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Images/Blog_Images/" + ID));
                    if (Result != null)
                    {
                        _filePath = Result.ToList();
                    }
                }

                if (_filePath == null || _filePath.Count == 0)
                {
                    _filePath.Add(HttpContext.Current.Server.MapPath("~/Images/Blog_Images/No_Image.jpg"));
                }
                else
                {
                    foreach (var item in _filePath)
                    {
                        _temp.Add("/Images/Blog_Images/" + ID + "/" + Path.GetFileName(item));
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
                if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Images/Blog_Images/" + ID)))
                {
                    //var Result = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID);
                    var Result = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Images/Blog_Images/" + ID));

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
                    return "/Images/Blog_Images/No_Image.jpg";
                }
                else
                {
                    var _FileName = Path.GetFileName(_filePath);
                    return "/Images/Blog_Images/" + ID + "/" + _FileName;
                }
            }

        }
        //{
        //    public string BlogTitle { get { return "IPS Parents-No Fee Hike..."; } }

        //    public int BlogID { get { return 1; } }

        //    public UserCommentsViewModel NewComment { get; set; }

        //    public IList<UserCommentsViewModel> PostedComments { get; set; }

        //    //public IList<string> ImageSliders
        //    //{
        //    //    get
        //    //    {
        //    //        IList<string> _filePath = new List<string>();
        //    //        IList<string> _temp = new List<string>();
        //    //        if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Images//Blog_Images//" + BlogID))
        //    //        {
        //    //            var Result = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Images//Blog_Images//" + BlogID);
        //    //            if (Result != null)
        //    //            {
        //    //                _filePath = Result.ToList();
        //    //            }
        //    //        }

        //    //        if (_filePath == null || _filePath.Count == 0)
        //    //        {
        //    //            _filePath.Add("../Images/Blog_Images/No_Image.jpg");
        //    //        }
        //    //        else
        //    //        {
        //    //            foreach (var item in _filePath)
        //    //            {
        //    //                _temp.Add("../Images/Blog_Images/" + BlogID + "/" + Path.GetFileName(item));
        //    //            }
        //    //            _filePath = _temp;
        //    //            //var _FileName = Path.GetFileName(_filePath);
        //    //            //return "../Images/Business_Images/" + ID + "/" + _FileName;
        //    //        }

        //    //        return _filePath;
        //    //    }
        //    //}

        //    //public string ImagePath
        //    //{
        //    //    get
        //    //    {
        //    //        string _filePath = string.Empty;
        //    //        if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Images//Blog_Images//" + BlogID))
        //    //        {
        //    //            var Result = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Images//Blog_Images//" + BlogID);

        //    //            foreach (var file in Result)
        //    //            {
        //    //                if (Path.GetFileName(file) == "Main.jpg")
        //    //                {
        //    //                    _filePath = "Main.jpg";
        //    //                }
        //    //            }
        //    //            //_filePath = Result == null ? string.Empty : Result.Count() == 0 ? string.Empty : Result[0];
        //    //        }

        //    //        if (string.IsNullOrEmpty(_filePath))
        //    //        {
        //    //            return "../Images/Blog_Images/No_Image.jpg";
        //    //        }
        //    //        else
        //    //        {
        //    //            var _FileName = Path.GetFileName(_filePath);
        //    //            return "../Images/Blog_Images/" + BlogID + "/" + _FileName;
        //    //        }
        //    //    }
        //    //}

        //    public IList<string> ImageSliders
        //    {
        //        get
        //        {
        //            IList<string> _filePath = new List<string>();
        //            IList<string> _temp = new List<string>();
        //            //if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID))
        //            if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Images/Blog_Images/" + BlogID)))
        //            {
        //                //var Result = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID);
        //                var Result = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Images/Blog_Images/" + BlogID));
        //                if (Result != null)
        //                {
        //                    _filePath = Result.ToList();
        //                }
        //            }

        //            if (_filePath == null || _filePath.Count == 0)
        //            {
        //                _filePath.Add(HttpContext.Current.Server.MapPath("~/Images/Blog_Images/No_Image.jpg"));
        //            }
        //            else
        //            {
        //                foreach (var item in _filePath)
        //                {
        //                    _temp.Add("/Images/Blog_Images/" + BlogID + "/" + Path.GetFileName(item));
        //                }
        //                _filePath = _temp;
        //                //var _FileName = Path.GetFileName(_filePath);
        //                //return "../Images/Business_Images/" + ID + "/" + _FileName;
        //            }

        //            return _filePath;
        //        }
        //    }

        //    public string ImagePath
        //    {
        //        get
        //        {
        //            string _filePath = string.Empty;

        //            //if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID))
        //            if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Images/Blog_Images/" + BlogID)))
        //            {
        //                //var Result = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID);
        //                var Result = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Images/Blog_Images/" + BlogID));

        //                foreach (var file in Result)
        //                {
        //                    if (Path.GetFileName(file) == "Main.jpg")
        //                    {
        //                        _filePath = "Main.jpg";
        //                    }
        //                }
        //                //_filePath = Result == null ? string.Empty : Result.Count() == 0 ? string.Empty : Result[0];
        //            }

        //            if (string.IsNullOrEmpty(_filePath))
        //            {
        //                return "/Images/Blog_Images/No_Image.jpg";
        //            }
        //            else
        //            {
        //                var _FileName = Path.GetFileName(_filePath);
        //                return "/Images/Blog_Images/" + BlogID + "/" + _FileName;
        //            }
        //        }
        //    }

    }

}