using Passport_Visa_Management_System.Models;
using Passport_Visa_Management_System.PassportVisaManagementSystemService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Passport_Visa_Management_System.Controllers
{
    public class ReIssuePassportController : Controller
    {
        // GET: ReIssuePassport
        //[Authorize]
        public ActionResult Index()
        {
            var username = Request.Cookies["UserName"].Value.ToString();
            int userId = DbOperation.FetchIdByUserName(username);
            if(! DbOperation.CheckUserHaveApplyPassport(userId))
            {
                return Redirect("/ReissuePassportError");
            }
            else
            {
            ApplyPassport C = new ApplyPassport();
            Service1Client PVMS = new Service1Client();
            Country[] D = PVMS.FetchCountries();
            ViewBag.CountryDD = D.ToList();
            return View(C);

            }
        }
        [HttpPost]
        public ActionResult Index(ApplyPassport A)
        {
            var username = Request.Cookies["UserName"].Value.ToString();
            ViewBag.CountryDD = null;
            Service1Client PVMS = new Service1Client();
            Country[] D = PVMS.FetchCountries();
            ViewBag.CountryDD = D.ToList();
            int userId = DbOperation.FetchIdByUserName(username);
            A.UserId = userId;
            bool successful = DbOperation.ReissuePassport(A);
            if (successful)
            {
                var json = DbOperation.fetchApplyPassportbyUserId(userId);
                Session["PassportNumber"] = json[0].PassportNumber;
                Session["Amount"] = json[0].Amount;
                Session["successMsg"] = "<b>Passport re issue is successfully done.</b>\n Amount to be paid is Rs." + json[0].Amount + " Passport issue date is " + json[0].IssueDate.ToString("dd-MM-yyyy") + " and expiry date is " + json[0].ExpiryDate.ToString("dd-MM-yyyy");

                return Redirect("/ReIssuePassportSuccess");
            }
            else
            {

            return View();
            }
        }
        public string GetStateByCountryId(int selectedCountry)
        {
            ViewBag.stateListByCountry = DbOperation.FetchStateByCountryId(selectedCountry);
            return ViewBag.stateListByCountry;
        }
        public string GetCityByStateId(int selectState)
        {
            ViewBag.cityListByState = DbOperation.FetchCityByStateId(selectState);
            return ViewBag.cityListByState;
        }
    }
}