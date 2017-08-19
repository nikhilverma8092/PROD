using KYN_App_v1._1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KYN_App_v1._1.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UploadMultipleFiles(int ID)
        {
            Session.Add("ID", ID);
            UploadMultipleViewModel _multipleViewModel = new Models.UploadMultipleViewModel() { BusinessID = ID };
            return View(_multipleViewModel);
        }

        [HttpPost]
        public string UploadSecondaryImages(string url)
        {
            //string id = url.Split('/').Last();

            //if (string.IsNullOrEmpty(id))
            //{
            //    return RedirectToAction("Index", "Business");
            //}

            int _CreatedBusinessID = 0;

            if (!Int32.TryParse(Session["ID"].ToString(), out _CreatedBusinessID))
            {
                return "false";
            }

            foreach (string upload in Request.Files)
            {
                if (!string.IsNullOrEmpty(Request.Files[upload].FileName) && _CreatedBusinessID > 0)
                {
                    try
                    {
                        string _path = AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + _CreatedBusinessID;

                        if (!Directory.Exists(_path))
                        {
                            var DirectoryInfo = Directory.CreateDirectory(_path);
                        }

                        string path = AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + _CreatedBusinessID;
                        //string filename = Path.GetFileName(Request.Files[upload].FileName);
                        string filename = DateTime.UtcNow.Ticks.ToString() + ".jpg";
                        Request.Files[upload].SaveAs(Path.Combine(path, filename));
                        string ImagePath = filename;
                        string ImageName = Request.Form["ImageName"];
                    }
                    catch(Exception ex)
                    {
                        return "false";
                    }
                }
            }
            return "true";
        }

        public ActionResult DeleteFile(int ID, string imageID)
        {
            UploadMultipleViewModel _obj = new UploadMultipleViewModel() { BusinessID = ID };

            _obj.DeleteFile(imageID);

            return RedirectToAction("UploadMultipleFiles", "Upload", new { ID = ID });
        }

        [HttpPost]
        public ActionResult UploadFile(string url)
        {
            string id = url.Split('/').Last();

            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Business");
            }

            int _CreatedBusinessID = 0;

            if (!Int32.TryParse(id, out _CreatedBusinessID))
            {
                return RedirectToAction("Index", "Business");
            }
            try
            {
                foreach (string upload in Request.Files)
                {
                    if (!string.IsNullOrEmpty(Request.Files[upload].FileName) && _CreatedBusinessID > 0)
                    {
                        string _path = AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + _CreatedBusinessID;

                        if (!Directory.Exists(_path))
                        {
                            var DirectoryInfo = Directory.CreateDirectory(_path);
                        }

                        string path = AppDomain.CurrentDomain.BaseDirectory + "Images//Business_Images//" + _CreatedBusinessID;
                        //string filename = Path.GetFileName(Request.Files[upload].FileName);
                        string filename = "Main.jpg";
                        Request.Files[upload].SaveAs(Path.Combine(path, filename));
                        string ImagePath = filename;
                        string ImageName = Request.Form["ImageName"];

                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Index", "Business");
        }
    }
}