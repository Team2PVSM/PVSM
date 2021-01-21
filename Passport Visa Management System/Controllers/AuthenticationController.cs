using Passport_Visa_Management_System.Models;
using Passport_Visa_Management_System.PassportVisaManagementSystemService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Passport_Visa_Management_System.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        //[Authorize]
        public ActionResult Index()
        {
            User U = new User();
            Service1Client PVMS = new Service1Client();
            //HintQuestion[] D = PVMS.FetchHintQuestion();
            //ViewBag.HintQuestionDD = D.ToList();
            var username = Request.Cookies["UserName"].Value.ToString();
            TempData["HintQuestion"] = DbOperation.FetchHintQuestionByUserName(username);
            return View(U);
        }
        [HttpPost]
        public ActionResult Index(User U)
        {
            var username = Request.Cookies["UserName"].Value.ToString();
            int userId = DbOperation.FetchIdByUserName(username);
            TempData["HintQuestion"] = DbOperation.FetchHintQuestionByUserName(username);
            U.UserId = username;
            
            if (DbOperation.Authenticity(U))
            {
                return Redirect("/VisaCancellation");
            }
            else
            {
                TempData["Msg"] = "Answer does not Match.Please Try Again";
                return View();

            }
        }
    }
}