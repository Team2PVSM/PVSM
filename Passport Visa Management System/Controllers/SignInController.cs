using Newtonsoft.Json.Linq;
using Passport_Visa_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Passport_Visa_Management_System.PassportVisaManagementSystemService;
using System.Text.RegularExpressions;

namespace Passport_Visa_Management_System.Controllers
{
    public class SignInController : Controller
    {
        // GET: SignIn
        public ActionResult Index()
        {
            Session["Page"] = null;
            ViewBag.signinActive = "active";
            PassportVisaManagementSystemService.User U = new PassportVisaManagementSystemService.User();
            PassportVisaManagementSystemService.Service1Client PVMS = new PassportVisaManagementSystemService.Service1Client();
            PassportVisaManagementSystemService.HintQuestion[] D = PVMS.FetchHintQuestion();
            ViewBag.HintQuestionDD = D.ToList();
            return View(U);
        }
        [HttpPost]
        public ActionResult Index(string AccountType, PassportVisaManagementSystemService.User U)
        {
            ViewBag.HintQuestionDD = null;
            PassportVisaManagementSystemService.Service1Client PVMS = new PassportVisaManagementSystemService.Service1Client();
            PassportVisaManagementSystemService.HintQuestion[] D = PVMS.FetchHintQuestion();
            ViewBag.HintQuestionDD = D.ToList();
           
            if (AccountType.ToLower() == "signup" && checkForSignUpValidation(U))
            {
                if (AccountType.ToLower()== "signup")
                {
                    ViewBag.signupActive = "active";
                    ViewBag.signinActive = "";
                }
                if (AccountType.ToLower() == "signin")
                {
                    ViewBag.signinActive = "active";
                    ViewBag.signupActive = "";

                }
                return View();
            }
            if (AccountType.ToLower() == "signin" && checkForSignInValidation(U))
            {
                if (AccountType.ToLower() == "signup")
                {
                    ViewBag.signupActive = "active";
                    ViewBag.signinActive = "";
                }
                if (AccountType.ToLower() == "signin")
                {
                    ViewBag.signinActive = "active";
                    ViewBag.signupActive = "";

                }
                return View();
            }
            //checkForSignInValidation(U);
            //Cookies.Expires = DateTime.Now.AddSeconds(1);
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
                    Response.Cookies["UserName"].Value = list[0].UserId;
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
                    Response.Cookies["UserName"].Value = U.UserId;
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

        public bool checkForSignUpValidation(User U )
        {
            var age = DateTime.Now.Subtract(U.DateOfBirth).TotalDays / 365;
            var email = DbOperation.CheckUniqueEmail(U.EmailAddress);
            bool isEmail = Regex.IsMatch(U.EmailAddress, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

            if (U.FirstName == null ||U.FirstName == "" || U.FirstName.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("FirstName", "FirstName Name cannot be empty");
                return true;   
            }
            else if (U.SurName == null || U.SurName == "" || U.SurName.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("SurName", "Sur Name cannot be empty");
                return true;
            }
            else if (U.DateOfBirth == null  || U.DateOfBirth.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("DateOfBirth", "Date Of Birth cannot be empty");
                return true;
            }
            else if (age <= 18)
            {
                ModelState.AddModelError("DateOfBirth", "Birth Date Should be greater than 18");
                return true;
            }
            else if (U.Address == null || U.Address.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("Address", "Address cannot be empty");
                return true;
            }
            else if (U.ContactNo == 0 || U.ContactNo.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("ContactNo", "Contact No cannot be empty");
                return true;
            }
            else if (U.ContactNo.ToString().Trim().Length <= 9 || U.ContactNo.ToString().Trim().Length >= 11)
            {
                ModelState.AddModelError("ContactNo", "Provide Valid Number");
                return true;
            }
            else if (U.EmailAddress == null || U.EmailAddress.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("EmailAddress", "Email Address cannot be empty");
                return true;
            }
            else if(email != null)
            {
                ModelState.AddModelError("EmailAddress", "Email ID is Already Registered");
                return true;
            }
            else if (isEmail == false)
            {
                ModelState.AddModelError("EmailAddress", "Enter Valid Email ID");
                return true;
            }
            else if (U.Qualification == null || U.Qualification.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("Qualification", "Qualification cannot be empty");
                return true;
            }
            else if (U.Gender == "-1"|| U.Gender.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("Gender", "Gender cannot be empty");
                return true;
            }
            else if (U.HintQuestionId == 0 || U.HintQuestionId.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("HintQuestionDD", "Hint Question cannot be empty");
                return true;
            }
            else if (U.HintAnswer == null || U.HintAnswer.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("HintAnswer", "Hint Answer cannot be empty");
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkForSignInValidation(User U)
        {
            if (U.UserId == null || U.UserId == "" || U.UserId.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("UserId", "User Id cannot be empty");
                return true;
            }
            else if (U.Password == null || U.Password == "" || U.Password.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("Password", "Password cannot be empty");
                return true;
            }
            else
            {
                return false;
            }
        }

        }
    }