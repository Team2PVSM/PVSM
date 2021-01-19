using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Passport_Visa_Management_System.PassportVisaManagementSystemService;

namespace Passport_Visa_Management_System.Controllers
{
    public class VisaCancellationController : Controller
    {
        // GET: VisaCancellation
        public ActionResult Index()
        {
            ApplyVisa AV = new ApplyVisa();

            return View(AV);
        }
        [HttpPost]
        public ActionResult Index(ApplyVisa AV)
        {

            return View();
        }
        }
}