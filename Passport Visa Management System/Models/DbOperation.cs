using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Passport_Visa_Management_System.Models
{
    public class DbOperation
    {
        public static bool UserSignUp(PassportVisaManagementSystemService.User U)
        {
            PassportVisaManagementSystemService.Service1Client PVMS = new PassportVisaManagementSystemService.Service1Client();
            return PVMS.SignUp(U);

        }
        public static bool UserSignIn(PassportVisaManagementSystemService.User U)
        {
            PassportVisaManagementSystemService.Service1Client PVMS = new PassportVisaManagementSystemService.Service1Client();
            return PVMS.SignIn(U.UserId,U.Password);
        }
        public static dynamic FetchUserByEmail(string Email)
        {
            PassportVisaManagementSystemService.Service1Client PVMS = new PassportVisaManagementSystemService.Service1Client();
            string str= PVMS.FetchUserByEmailAddress(Email) ;
            return JsonConvert.DeserializeObject<List<PassportVisaManagementSystemService.User>>(str);
        }
        public static dynamic FetchUserByuserparameter(string parameter, string value)
        {
            PassportVisaManagementSystemService.Service1Client PVMS = new PassportVisaManagementSystemService.Service1Client();
            string str = PVMS.FetchUserByuserparameter(parameter, value);
            return JsonConvert.DeserializeObject<List<PassportVisaManagementSystemService.User>>(str);
        }
    }
}