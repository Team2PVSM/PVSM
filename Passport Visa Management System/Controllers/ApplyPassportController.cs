using Passport_Visa_Management_System.Models;
using Passport_Visa_Management_System.PassportVisaManagementSystemService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Passport_Visa_Management_System.Controllers
{
    public class ApplyPassportController : Controller
    {
        // GET: ApplyPassport
        //[Authorize]
        public ActionResult Index()
        {
            ApplyPassport C = new ApplyPassport();
            Service1Client PVMS = new Service1Client();
            Country[] D = PVMS.FetchCountries();
            ViewBag.CountryDD = D.ToList();
            return View(C);
        }
        [HttpPost]
        public ActionResult Index(ApplyPassport A)
        {
            
            var username = Request.Cookies["UserName"].Value.ToString();
            ViewBag.CountryDD = null;
            Service1Client PVMS = new Service1Client();
            Country[] D = PVMS.FetchCountries();
            ViewBag.CountryDD = D.ToList();
            if (checkForApplyPassportValidation(A))
            {
                return View();
            }
            int userId = DbOperation.FetchIdByUserName(username);
            A.UserId = userId;
            bool successful = DbOperation.ApplyPassportNew(A);
            if (successful)
            {
                var json = DbOperation.fetchApplyPassportbyUserId(userId);
                Session["PassportNumber"] = json[0].PassportNumber;
                Session["Amount"] = json[0].Amount;
                Session["successMsg"] = "<b>Need the passport number while giving payment? Please note down your Id \n </b> \n" + json[0].PassportNumber + ".Passport application cost is Rs." + json[0].Amount;

                return Redirect("/ApplyPassportSuccess");
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
        public bool checkForApplyPassportValidation(ApplyPassport U)
        {
             if (U.CountryId ==0  || U.CountryId.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("CountryId", "Country can't be empty");
                return true;
            }
            else if (U.StateId == 0 || U.StateId.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("StateId", "State can't be empty");
                return true;
            }
            else if (U.CityId == 0 || U.CityId.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("CityId", "City can't be empty");
                return true;
            }
            else if (U.Pin == 0 || U.Pin.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("Pin", "Pin can't be empty");
                return true;
            }
            else if(U.Pin.ToString().Trim().Length >= 7 || U.Pin.ToString().Trim().Length <= 5)
            {
                ModelState.AddModelError("Pin", "Pin Should be of 6 Digits");
                return true;
            }
            else if (U.IssueDate == null || U.IssueDate.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("IssueDate", "Fill Date");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}