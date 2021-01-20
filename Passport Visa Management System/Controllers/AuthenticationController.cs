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
        public ActionResult Index()
        {
            User U = new User();
            Service1Client PVMS = new Service1Client();
            HintQuestion[] D = PVMS.FetchHintQuestion();
            ViewBag.HintQuestionDD = D.ToList();
            return View(U);
        }
        [HttpPost]
        public ActionResult Index(User U)
        {
            DbOperation.Authenticity(U);
            return View();
        }
    }
}