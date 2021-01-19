using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Passport_Visa_Management_System.PassportVisaManagementSystemService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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
        public static bool ApplyPassportNew(ApplyPassport A)
        {
            Service1Client PVMS = new Service1Client();
            return PVMS.ApplyForPassport(A);
        }
        public static dynamic FetchStateByCountryId(int CountryId)
        {

            PassportVisaManagementSystemService.Service1Client PVMS = new PassportVisaManagementSystemService.Service1Client();
            string str = PVMS.FetchState(CountryId);
            return str;
        }
        public static dynamic FetchCityByStateId(int StateId)
        {

            PassportVisaManagementSystemService.Service1Client PVMS = new PassportVisaManagementSystemService.Service1Client();
            string str = PVMS.FetchCity(StateId);
            return str;
        }
        public static int FetchIdByUserName(string userName)
        {
            PassportVisaManagementSystemService.Service1Client PVMS = new PassportVisaManagementSystemService.Service1Client();
            return PVMS.getIdByUserId(userName);
        }
        public static dynamic fetchApplyPassportbyUserId(int UserId)
        {
            PassportVisaManagementSystemService.Service1Client PVMS = new PassportVisaManagementSystemService.Service1Client();
            string str = PVMS.fetchApplyPassportbyUserId(UserId);
            return JsonConvert.DeserializeObject<List<PassportVisaManagementSystemService.ApplyPassport>>(str);
        }
		public static bool ReissuePassport(ApplyPassport R)
        {
            Service1Client PVMS = new Service1Client();
            return PVMS.ReIssuePassport(R);
        }
        public static bool ApplyingVisa(ApplyVisa A)
        {
            Service1Client PVMS = new Service1Client();
            return PVMS.ApplyForVisa(A);
        }
    }
}