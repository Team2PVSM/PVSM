using Newtonsoft.Json.Linq;
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
            Session["Page"] = null;
            PassportVisaManagementSystemService.User U = new PassportVisaManagementSystemService.User();
            PassportVisaManagementSystemService.Service1Client PVMS = new PassportVisaManagementSystemService.Service1Client();
            PassportVisaManagementSystemService.HintQuestion[] D = PVMS.FetchHintQuestion();
            ViewBag.HintQuestionDD = D.ToList();
            return View(U);
        }
        [HttpPost]
        public ActionResult Index(string AccountType, PassportVisaManagementSystemService.User U)
        {
           
            if (AccountType.ToLower() == "signup")
            {
                Session["SignUpMsg"] = null;
                Session["userName"] = null;
                Session["password"] = null;
                string email = U.EmailAddress;
                bool SignUpResult = DbOperation.UserSignUp(U);
                var list = DbOperation.FetchUserByEmail(U.EmailAddress); //email id  must be unique
                if (list != null && list.Count == 1)
                {
                    Session["userName"] = list[0].UserId;
                    Session["password"] = list[0].Password;
                }
                else
                {
                    int l = list.Count;
                    Session["userName"] = list[l - 1].UserId;
                    Session["password"] = list[l - 1].Password;

                }

                string str = "Dear User \n Your User Id is " + list[0].UserId + " and your password is " + list[0].Password + ".\nYou are planning for " + list[0].ApplyType + " and your citizen type is " + list[0].CitizenType;
                if (SignUpResult)
                {
                    Session["Page"] = AccountType;
                    Session["SignUpMsg"] = str;
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
                    Session["Page"] = AccountType;
                    var userList = DbOperation.FetchUserByuserparameter("userid", U.UserId);

                    if (userList[0].ApplyType.ToLower() == "passport")
                    {
                        return Redirect("/ApplyPassport");

                    }
                    else if (userList[0].ApplyType.ToLower() == "visa")
                    {
                        return Redirect("/ApplyVisa");

                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {

                    return View();
                }
            }

        }
    }
}