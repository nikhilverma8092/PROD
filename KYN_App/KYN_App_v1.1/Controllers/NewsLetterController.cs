using KYN_App_v1._1.Models;
using KYN_App_v1._1.RestServiceHelper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KYN_App_v1._1.Controllers
{
    public class NewsLetterController : Controller
    {

        // GET: NewsLetter
        public ActionResult Index()
        {
            IList<UserBlogViewModel> _model = GetAllBlogTitle();
            return View(_model);
            //return RedirectToAction("GetBlog", new { ID = 1 });
        }

        [Authorize]
        public ActionResult GetBlog(int ID)
        {
            NewsLetterViewModel _model = new NewsLetterViewModel();
            var _userBlog = GetBlogDetail(ID);
            _model.ID = _userBlog.ID;
            _model.BlogDetail = _userBlog.BlogDetail;
            _model.BlogHeadLine = _userBlog.BlogHeadLine;
            _model.BlogTitle = _userBlog.BlogTitle;
            _model.CreatedOn = _userBlog.CreatedOn;
            _model.Name = _userBlog.Name;
            _model.UserEmail = _userBlog.UserEmail;
            _model.UserID = _userBlog.UserID;

            _model.PostedComments = GetComments(string.Empty);

            _model.PostedSupports = GetSupport(string.Empty);

            _model.InSupport = _model.PostedSupports.Where(c => c.UserEmail == _userBlog.UserEmail).Select(c => c.InSupport).FirstOrDefault();

            return View(_userBlog.ID.ToString(), _model);
        }


        private IList<UserBlogViewModel> GetAllBlogTitle()
        {
            return ServiceInfo.InvokePostService<List<UserBlogViewModel>>(ServiceInfo.GetAllBlogTitle, string.Empty);
        }

        private UserBlogViewModel GetBlogDetail(int ID)
        {
            return ServiceInfo.InvokePostService<UserBlogViewModel>(ServiceInfo.GetBlogDetail, ID.ToString());
        }

        private IList<UserCommentsViewModel> GetComments(string blogTitle)
        {
            return ServiceInfo.InvokePostService<List<UserCommentsViewModel>>(ServiceInfo.GetUserComments, "1");
        }

        private IList<UserSupportViewModel> GetSupport(string blogTitle)
        {
            return ServiceInfo.InvokePostService<List<UserSupportViewModel>>(ServiceInfo.GetUserSupport, "1");
        }

        private UserCommentsViewModel GetUserCommentModel(ApplicationUser _applicationUser, NewsLetterViewModel _model)
        {
            UserCommentsViewModel _userComment = new UserCommentsViewModel();
            if (_applicationUser != null)
            {
                _userComment.UserID = _applicationUser.Id;
                _userComment.UserEmail = _applicationUser.Email;
                _userComment.Name = _applicationUser.FirstName + " " + _applicationUser.LastName;
            }
          
            _userComment.PostedOn = DateTime.Now.ToString();
            _userComment.BlogID = _model.ID;
            _userComment.Comment = _model.NewComment;
            return _userComment;
        }

        private UserSupportViewModel GetUserSupportModel(ApplicationUser _applicationUser, NewsLetterViewModel _model)
        {
            UserSupportViewModel _userSupport = new UserSupportViewModel();
            if (_applicationUser != null)
            {
                _userSupport.UserID = _applicationUser.Id;
                _userSupport.UserEmail = _applicationUser.Email;
                _userSupport.Name = _applicationUser.FirstName + " " + _applicationUser.LastName;
            }
          

            _userSupport.PostedOn = DateTime.Now.ToString();
            _userSupport.BlogID = _model.ID;
            _userSupport.InSupport = _model.InSupport;
            return _userSupport;
        }

        [HttpPost]
        public ActionResult SubmitComment(NewsLetterViewModel _model)
        {
            UserCommentsViewModel _userComment = new UserCommentsViewModel();

            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            if (string.IsNullOrEmpty(_model.NewComment))
            {
                ServiceInfo.InvokePostService<bool>(ServiceInfo.SaveUserSupport, JsonConvert.SerializeObject(GetUserSupportModel(UserManager.FindByEmail(User.Identity.Name), _model)));
            }

            if (!string.IsNullOrEmpty(_model.NewComment))
            {
                ServiceInfo.InvokePostService<bool>(ServiceInfo.SaveUserComment, JsonConvert.SerializeObject(GetUserCommentModel(UserManager.FindByEmail(User.Identity.Name), _model)));
            }
            return RedirectToAction("GetBlog", new { ID = 1 });
        }
    }

    //{
    //    public ActionResult Index()
    //    {

    //        NewsLetterViewModel _model = new NewsLetterViewModel();
    //        _model.PostedComments = new List<UserCommentsViewModel>();

    //        IList<UserCommentsViewModel> _commentList = GetComments(string.Empty);

    //        if (_commentList == null)
    //        {
    //            _commentList = new List<UserCommentsViewModel>();
    //            _commentList.Add(new UserCommentsViewModel() { UserID = "1", Name = "Nikhil Verma", Comment = "Some Text Comment", PostedOn = "August 25, 2014 at 9:30 PM" });
    //            //_commentList.Add(new Comments() { UserID = "1", UserName = "Nikhil Verma", Comment = "Some Text Comment", PostedOn = "August 25, 2014 at 9:30 PM" });
    //            //_commentList.Add(new Comments() { UserID = "1", UserName = "Nikhil Verma", Comment = "Some Text Comment", PostedOn = "August 25, 2014 at 9:30 PM" });
    //            //_commentList.Add(new Comments() { UserID = "1", UserName = "Nikhil Verma", Comment = "Some Text Comment", PostedOn = "August 25, 2014 at 9:30 PM" });
    //            //_commentList.Add(new Comments() { UserID = "1", UserName = "Nikhil Verma", Comment = "Some Text Comment", PostedOn = "August 25, 2014 at 9:30 PM" });
    //            //Session["Comments"] = _commentList;
    //        }

    //        _model.PostedComments = _commentList;
    //        return View(_model);
    //    }

    //    private IList<UserCommentsViewModel> GetComments(string blogTitle)
    //    {
    //        return null;

    //        //return ServiceInfo.InvokePostService<List<UserCommentsViewModel>>(ServiceInfo.GetUserComments, blogTitle);
    //    }

    //    [HttpPost]
    //    public async Task<ActionResult> SubmitComment(NewsLetterViewModel _model)
    //    {
    //        //UserCommentsViewModel _userComment = new UserCommentsViewModel();

    //        //UserManager<ApplicationUser>  UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
    //        //if (UserManager != null)
    //        //{
    //        //    var _user = UserManager.FindByEmail(User.Identity.Name);
    //        //    if (_user != null)
    //        //    {
    //        //        _userComment.UserID = _user.Id;
    //        //        _userComment.UserEmail = _user.Email;
    //        //        _userComment.Name = _user.FirstName + " " + _user.LastName;
    //        //    }
    //        //}

    //        //_userComment.PostedOn = "April 10, 2020";
    //        //_userComment.BlogID = 1;
    //        //_userComment.Comment = _model.NewComment.Comment;            

    //        //ServiceInfo.InvokePostService<List<UserCommentsViewModel>>(ServiceInfo.GetUserComments, _userComment.ToString());
    //        return RedirectToAction("Index");
    //    }
    //}
}