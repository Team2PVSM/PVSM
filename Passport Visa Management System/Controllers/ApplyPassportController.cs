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
            DbOperation.ApplyPassportNew(A);
            return View();
        }
    }
}