using Passport_Visa_Management_System.Models;
using Passport_Visa_Management_System.PassportVisaManagementSystemService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Passport_Visa_Management_System.Controllers
{
    public class ApplyVisaController : Controller
    {
        // GET: ApplyVisa
        public ActionResult Index()
        {
            ApplyVisa V = new ApplyVisa();
            Service1Client PVMS = new Service1Client();
            Country[] D = PVMS.FetchCountries();
            ViewBag.CountryDD = D.ToList();
            return View(V);
        }
        [HttpPost]
        public ActionResult Index(ApplyVisa AV)
        {
            Service1Client PVMS = new Service1Client();
            Country[] D = PVMS.FetchCountries();
            ViewBag.CountryDD = D.ToList();
            var username = Request.Cookies["UserName"].Value.ToString();
            int userId = DbOperation.FetchIdByUserName(username);
            AV.UserId = userId;
            bool successful =DbOperation.ApplyingVisa(AV);
            if (successful)
            {
                var json = DbOperation.fetchApplyVisabyUserId(userId);
                string destination = DbOperation.fetchCountryStateCityById(json[0].CountryId);
                Session["successMsg"] = "Dear User your Visa request has been accepted successfully with id " + json[0].VisaNumber  +" ,UserId " + username + " Destination " + destination + " Employee Occupation " + json[0].Occupation + " Date of Application " + json[0].DateOfApplication.ToString("dd-MM-yyyy") + " Date of Issue " + json[0].DateOfIssue.ToString("dd-MM-yyyy") + "\n Date of Expiry " + json[0].DateOfExpiry.ToString("dd-MM-yyyy") + " Registration Cost " + json[0].RegistrationCost;

                return Redirect("/ApplyVisaSuccess");
            }
            else
            {

                return View();

            }

        }
    }
}