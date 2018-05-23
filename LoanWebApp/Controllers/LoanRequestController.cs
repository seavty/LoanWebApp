using LoanWebApp.Handlers;
using LoanWebApp.Helpers;
using LoanWebApp.Models.DTO.LoanRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LoanWebApp.Controllers
{
    public class LoanRequestController : Controller
    {
        private LoanRequestHandler handler = null;


        public LoanRequestController()
        {
            handler = new LoanRequestHandler();
        }
        // GET: LoanRequest
        public ActionResult Index(int id)
        {
            ViewBag.accountID = id;
            return View();
        }

        //-> Create new loan request

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> LoanRequest(LoanRequestNewDTO loanRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);
                
                Response.StatusCode = 200;
                return Json(await handler.Create(loanRequest), JsonRequestBehavior.AllowGet);

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
    }
}