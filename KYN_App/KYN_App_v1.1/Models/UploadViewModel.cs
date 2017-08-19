using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace KYN_App_v1._1.Models
{
    public class UploadMultipleViewModel : BaseViewModel
    {
        public int BusinessID { get; set; }


        public bool DeleteFile(string imageID)
        {
            IList<string> _allImages = ImageSliders;
            if (_allImages.Contains(imageID))
            {
                try
                {
                    File.Delete(HttpContext.Current.Server.MapPath(imageID));
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }



        public IList<string> ImageSliders
        {
            get
            {
                IList<string> _filePath = new List<string>();
                IList<string> _temp = new List<string>();
                //if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID))
                if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Images/Business_Images/" + BusinessID)))
                {
                    //var Result = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + ID);
                    var Result = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Images/Business_Images/" + BusinessID));
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
                        _temp.Add("/Images/Business_Images/" + BusinessID + "/" + Path.GetFileName(item));
                    }
                    _filePath = _temp;
                    //var _FileName = Path.GetFileName(_filePath);
                    //return "../Images/Business_Images/" + ID + "/" + _FileName;
                }
                _filePath.Remove("/Images/Business_Images/" + BusinessID + "/Main.jpg");

                return _filePath;
            }
        }
    }
}