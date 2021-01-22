using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Passport_Visa_Management_System.Models;
using Passport_Visa_Management_System.PassportVisaManagementSystemService;

namespace Passport_Visa_Management_System.Controllers
{
    public class VisaCancellationController : Controller
    {
        // GET: VisaCancellation
        //[Authorize]
        public ActionResult Index()
        {
            if (Request.Cookies["UserName"] == null)
            {
                return Redirect("/SignIn");
            }
            var username = Request.Cookies["UserName"].Value.ToString();
            int userId = DbOperation.FetchIdByUserName(username);
            if (!DbOperation.CheckUserHaveApplyVisa(userId))
            {
                return Redirect("/CancelVisaError");
            }
            else
            {
                Session["PassportNumber"] = DbOperation.FetchPassportNumber(userId);
                Session["VisaNumber"] = DbOperation.FetchVisaNumber(userId);

                ApplyVisa AV = new ApplyVisa();
                return View(AV);

            }

        }
        [HttpPost]
        public ActionResult Index(ApplyVisa AV)
        {
            var username = Request.Cookies["UserName"].Value.ToString();
            int userId = DbOperation.FetchIdByUserName(username);
            string us = DbOperation.FetchPassportNumber(userId);
            string Visa = DbOperation.FetchVisaNumber(userId);
            AV.VisaNumber = Visa;
            AV.UserId = userId;
            Session["PassportNumber"] = us;
            if (checkForCancelVisaValidation(AV))
            {
                return View();
            }
            bool successful = DbOperation.VisaCancel(AV);
            if (successful)
            {
                var json = DbOperation.fetchApplyVisabyUserId(userId);
                Session["successMsg"] = "Your request has been submitted successfully. Please pay " + json[0].CancellationCharge + " to complete the cancellation process";

                return Redirect("/CancelVisaSuccess");
            }
            else
            {

                return View();
            }
        }
        public bool checkForCancelVisaValidation(ApplyVisa U)
        {
            if (U.DateOfIssue == null || U.DateOfIssue.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("issuedate", "Fill Date");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}