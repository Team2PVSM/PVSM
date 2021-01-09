using Passport_Visa_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Passport_Visa_Management_System.Controllers
{
    public class SignInController : Controller
    {
        // GET: SignIn
        public ActionResult Index()
        {
            PassportVisaManagementSystemService.User U = new PassportVisaManagementSystemService.User();
            return View(U);
        }
        [HttpPost]
        public ActionResult Index(PassportVisaManagementSystemService.User U)
        {
            bool SignInResult = DbOperation.UserSignIn(U);
            if (SignInResult)
            {
                return Redirect("/UserHome");
            }
            else
            {

                return View();
            }
        }
    }
}