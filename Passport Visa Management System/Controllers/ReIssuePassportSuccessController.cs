using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Passport_Visa_Management_System.Controllers
{
    public class ReIssuePassportSuccessController : Controller
    {
        // GET: ReIssuePassportSuccess
        //[Authorize]
        public ActionResult Index()
        {
            if (Request.Cookies["UserName"] == null)
            {
                return Redirect("/SignIn");
            }
            return View();
        }
    }
}