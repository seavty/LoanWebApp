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
        public async Task<ActionResult> Index()
        {
            return View(await handler.GetList(1));
        }


        //->Paging 
        public async Task<ActionResult> Paging(int id)
        {
            return PartialView(await handler.GetList(id));
        }

        // GET: AccountViewDTOes/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(await handler.SelectByID(id));
        }

        //-> Save
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

        //-> SubmitForApproval
        [HttpPost]
        public async Task<ActionResult> SubmitRequest(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Response.StatusCode = 200;
                    return Json(await handler.SubmitForApproval(id, Request.Form["status"].ToString()), JsonRequestBehavior.AllowGet);
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