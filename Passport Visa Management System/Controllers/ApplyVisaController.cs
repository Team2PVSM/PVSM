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
            var username = Request.Cookies["UserName"].Value.ToString();
            int userId = DbOperation.FetchIdByUserName(username);
            AV.UserId = userId;
            DbOperation.ApplyingVisa(AV);
            return View();
        }
    }
}