using KYN_App_v1._1.Models;
using KYN_App_v1._1.RestServiceHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KYN_App_v1._1.Controllers
{
    public class FooterContentController : Controller
    {
        [Route("Sitemap")]
        public ActionResult SiteMap()
        {
            SearchViewModel _model = new SearchViewModel();
            _model.Categories = GetValidData();
            return View(_model);
        }

        private IList<SearchCategoryViewMode> GetValidData()
        {
            IList<SearchCategoryViewMode> _LoadData = Session["SearchCategoryViewModelList"] as IList<SearchCategoryViewMode>;
            int _counter = Session["Counter"] == null ? 0 : (int)Session["Counter"];

            if (_LoadData == null || _counter > 10)
            {
                _counter = 0;
                Session.RemoveAll();
                _LoadData = ServiceInfo.InvokeGetService<IList<SearchCategoryViewMode>>(ServiceInfo.GetAllSearchBusinessParentData, string.Empty);
                Session.Add("SearchCategoryViewModelList", _LoadData);
                Session.Add("Counter", 0);
            }
            Session["Counter"] = _counter + 1;
            return _LoadData;
        }

        [Route("HowitWorks")]
        public ActionResult HowitWorks()
        {
            return View();
        }

        [Route("Faq")]
        public ActionResult Faq()
        {
            return View();
        }

        [Route("Feedback")]
        public ActionResult Feedback()
        {
            return View();
        }

        [Route("Contact")]
        public ActionResult Contact()
        {
            return View();
        }

        [Route("Typography")]
        public ActionResult Typography()
        {
            return View();
        }
    }
}