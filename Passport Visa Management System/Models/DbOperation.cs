using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Passport_Visa_Management_System.Models
{
    public class DbOperation
    {
        public static bool UserSignIn(PassportVisaManagementSystemService.User U)
        {
            PassportVisaManagementSystemService.Service1Client PVMS = new PassportVisaManagementSystemService.Service1Client();
            return  PVMS.SignIn(U);
        }
    }
}