﻿using LoanWebApp.Handlers;
using LoanWebApp.Models.DTO.CheckLoanRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LoanWebApp.Controllers
{
    
    public class CheckLoanRequestController : Controller
    {
        private CheckLoanRequestHandler handler = null;

        public CheckLoanRequestController()
        {
            handler = new CheckLoanRequestHandler();
        }

        // GET: CheckLoanRequest
        public ActionResult Index()
        {
            return View();
        }

        //->Paging 
        public async Task<ActionResult> Paging(CheckLoanRequestFindDTO findDTO)
        {
            return PartialView(await handler.GetList(findDTO));
        }


        // GET: AccountViewDTOes/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(await handler.SelectByID(id));
        }

        //-> Save
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Save(CheckLoanRequestEditDTO checkLoanRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Response.StatusCode = 200;
                    return Json(await handler.Save(checkLoanRequest), JsonRequestBehavior.AllowGet);
                }
                return null;
                
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        //-> SubmitRequest
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> SubmitRequest(LoanRequestSubmitStatusDTO loanRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Response.StatusCode = 200;
                    return Json(await handler.SubmitRequest(loanRequest), JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = 410;
                return null;
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}