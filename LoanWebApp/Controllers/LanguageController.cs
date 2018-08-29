using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace LoanWebApp.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Change(string langAbb)
        {
            if(langAbb != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(langAbb);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(langAbb);
            }

            HttpCookie cookie = new HttpCookie("lang");
            cookie.Value = langAbb;
            Response.Cookies.Add(cookie);
            //Response.Redirect(Url.Content("~"));
            Response.Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.AbsoluteUri);
            return View("Index");
        }
    }
}