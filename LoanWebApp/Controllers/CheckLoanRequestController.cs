using LoanWebApp.Handlers;
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
    }
}