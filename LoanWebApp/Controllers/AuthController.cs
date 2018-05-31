using LoanWebApp.Models.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoanWebApp.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public string Login(LoginDTO login)
        {
            if (login.userName == "admin" && login.password == "123")
                return "ok";
            Response.StatusCode = 404;
            return "";
        }
    }
}