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
            PassportVisaManagementSystemService.Service1Client PVMS = new PassportVisaManagementSystemService.Service1Client();
            PassportVisaManagementSystemService.HintQuestion[] D  = PVMS.FetchHintQuestion();
            ViewBag.HintQuestionDD = D.ToList();
            return View(U);
        }
        [HttpPost]
        public ActionResult Index(string AccountType, PassportVisaManagementSystemService.User U)
        {
            if (AccountType.ToLower()== "signup")
            {
                Session["SignUpMsg"] = null;
                string str = "Dear User \n Your User Id is" +U.UserId+ "and your password is"+U.Password+ ".\nYou are planning for"+ U.ApplyType +"and your citizen type is"+U.CitizenType;
                bool SignUpResult = DbOperation.UserSignUp(U);
                if (SignUpResult)
                {
                    Session["SignUpMsg"]  = str;
                    return Redirect("/UserHome");
                }
                else
                {

                    return View();
                }

            }
            else
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
}