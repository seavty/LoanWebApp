using LoanWebApp.Handlers;
using LoanWebApp.Helpers;
using LoanWebApp.Models.DTO.Account;
using LoanWebApp.Models.DTO.LoanRequest;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
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

        [HttpGet]
        public ActionResult RequestLoan(int id)
        {
            ViewBag.accountID = id; 
            return View();
        }

        //

        
        [HttpPost]
        [ActionName("pinrequest")]
        public async Task<JsonResult> RequestPin_Post(int id,string phone)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);

                Response.StatusCode = 200;
                var account = await handler.createPin(id, phone);
                return Json(account, JsonRequestBehavior.AllowGet);

            }
            catch (HttpException ex)
            {
                if (ex.Message == ConstantHelper.INVALID_PHONE ||
                    ex.Message == ConstantHelper.ALREADY_REQUEST_LOAN ||
                    ex.Message == ConstantHelper.PENDING_SMS
                    || ex.Message == ConstantHelper.PHONE_EXIST
                    || ex.Message == ConstantHelper.EMAIL_EXIST)
                    Response.StatusCode = 400;
                else
                    Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }
        }

        /*
        [HttpPost]
        [ActionName("Reupload")]
        public async Task<JsonResult> reupload(int id, HttpRequestBase Request)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);

                Response.StatusCode = 200;
                var account = await handler.reupload(id, Request);
                return Json(id, JsonRequestBehavior.AllowGet);
                

            }
            catch (Exception ex)
            {
                if (ex.Message == ConstantHelper.ALREADY_REQUEST_LOAN
                    || ex.Message == ConstantHelper.KEY_IN_REQUIRED_FIELD
                    || ex.Message == ConstantHelper.INVALID_PIN
                    || ex.Message == ConstantHelper.PIN_EXPIRED)
                    Response.StatusCode = 400;
                else
                    Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }
        }
        */
        //--me 
        [HttpPost]
        [ActionName("Reupload")]
        public async Task<ActionResult> reupload(AccountReuploadDocumentDTO reuploadDocument)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);

                Response.StatusCode = 200;
                var account = await handler.reupload(reuploadDocument, Request);
                //return Json(id, JsonRequestBehavior.AllowGet);
                Response.Write("ok");
                return null;
                

            }
            catch (Exception ex)
            {
                if (ex.Message == ConstantHelper.ALREADY_REQUEST_LOAN
                    || ex.Message == ConstantHelper.KEY_IN_REQUIRED_FIELD
                    || ex.Message == ConstantHelper.INVALID_PIN
                    || ex.Message == ConstantHelper.PIN_EXPIRED)
                    Response.StatusCode = 400;
                else
                    Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }
        }

        //--end me


        [HttpPost]
        [ActionName("RequestLoan")]
        public async Task<JsonResult> RequestLoan_Post(LoanRequestNewDTO loanRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);

                Response.StatusCode = 200;
                var account = await handler.CreateLoanRequest(loanRequest, Request);
                return Json(account.acct_AccountID, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (ex.Message == ConstantHelper.ALREADY_REQUEST_LOAN 
                    || ex.Message == ConstantHelper.KEY_IN_REQUIRED_FIELD
                    || ex.Message == ConstantHelper.INVALID_PIN
                    || ex.Message == ConstantHelper.PIN_EXPIRED)
                    Response.StatusCode = 400;
                else
                    Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        [ActionName("Register")]
        public async Task<String> Register_Post(AccountNewDTO account)
        {
            try
            {
                if (ModelState.IsValid)
                    return "ok" + (await handler.Create(account, Request)).acct_AccountID;
                else
                    return "no";

            }
            catch (Exception ex)
            {
                if (ex.Message == ConstantHelper.ALREADY_REQUEST_LOAN
                    || ex.Message == ConstantHelper.KEY_IN_REQUIRED_FIELD
                    || ex.Message == ConstantHelper.INVALID_PIN
                    || ex.Message == ConstantHelper.PIN_EXPIRED 
                    || ex.Message == ConstantHelper.PHONE_EXIST
                    || ex.Message == ConstantHelper.EMAIL_EXIST)
                    Response.StatusCode = 400;
                else
                    Response.StatusCode = 500;
                return ex.Message.ToString();
            }

        }

        // GET: Account
        public ActionResult CheckInfo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CheckInfo(AccountCheckInfo checkInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);

                Response.StatusCode = 200;
                var account = await handler.CheckPhoneNumber(checkInfo);
                if (account == null)
                    return Json("", JsonRequestBehavior.AllowGet);
                return Json(account.acct_AccountID, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (ex.Message == ConstantHelper.ALREADY_REQUEST_LOAN || ex.Message == ConstantHelper.KEY_IN_REQUIRED_FIELD)
                    Response.StatusCode = 400;
                else
                    Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
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

        /*
        [HttpPost]
        public async Task<String> Register(AccountNewDTO account)
        {

            if (ModelState.IsValid)
                return "ok" + (await handler.Create(account, Request)).id;
            else
                return "no";
        }
        */
    }
}