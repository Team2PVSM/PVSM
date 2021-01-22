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
            if (!DbOperation.CheckUserHaveApplyPassport(userId))
            {
                return Redirect("/ReissuePassportError");
            }
            else
            {
                ApplyPassport C = new ApplyPassport();
                Service1Client PVMS = new Service1Client();
                Country[] D = PVMS.FetchCountries();
                List<SelectListItem> CountrynewList = new List<SelectListItem>();
                CountrynewList.Add(new SelectListItem { Text = "Select Country", Value = "-1" });
                for (var i = 0; i < D.Length; i++)
                {
                    CountrynewList.Add(new SelectListItem { Text = D[i].CountryName, Value = D[i].CountryId.ToString() });
                }
                ViewBag.CountryDD = CountrynewList.ToList();
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
            List<SelectListItem> CountrynewList = new List<SelectListItem>();
            CountrynewList.Clear();
            CountrynewList.Add(new SelectListItem { Text = "Select Country", Value = "-1" });
            for (var i = 0; i < D.Length; i++)
            {
                CountrynewList.Add(new SelectListItem { Text = D[i].CountryName, Value = D[i].CountryId.ToString() });
            }
            ViewBag.CountryDD = CountrynewList.ToList();
            int userId = DbOperation.FetchIdByUserName(username);
            A.UserId = userId;
            if (checkForReissuePassportValidation(A))
            {
                return View();
            }
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
        public bool checkForReissuePassportValidation(ApplyPassport U)
        {
            if(U.Reason == null || U.Reason.ToString().Trim().Length == 0)
            {
                ModelState.AddModelError("CountryId", "Provide Reason");
                return true;
            }
            if (U.CountryId == 0 || U.CountryId.ToString().Trim().Length == 0)
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
            else if (U.Pin.ToString().Trim().Length >= 7 || U.Pin.ToString().Trim().Length <= 5)
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