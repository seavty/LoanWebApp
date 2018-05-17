using LoanWebApp.Handlers;
using LoanWebApp.Models.DTO.LoanRequest;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<JsonResult> LoanRequest(LoanRequestNewDTO loanRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Response.StatusCode = 200;
                    return Json(await handler.Create(loanRequest), JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = 400;
                return null;
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                //error should wirte to log file
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}