using LoanWebApp.Handlers;
using LoanWebApp.Models.DTO.Account;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LoanWebApp.Controllers
{
    public class AccountController : Controller
    {
        private AccountHandler handler = null;

        public AccountController()
        {
            handler = new AccountHandler();
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        //-> Create New Customer
        /*
        [HttpPost]
        public async Task<JsonResult> Register(AccountNewDTO account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Response.StatusCode = 200;
                    return Json(await handler.Create(account, Request), JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = 400;
                return null;
            }
            catch(Exception ex)
            {
                Response.StatusCode = 500;
                //error should wirte to log file
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        */

        /*
        [HttpPost]
        public String Register(AccountNewDTO account)
        {
            return "ok";
        }
        */

        [HttpPost]
        public async Task<String> Register(AccountNewDTO account)
        {

            if (ModelState.IsValid)
                return "ok" + (await handler.Create(account, Request)).id;
            else
                return "no";
        }
    }
}