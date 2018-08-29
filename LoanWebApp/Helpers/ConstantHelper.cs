using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanWebApp.Helpers
{
    public static class ConstantHelper
    {
        public static readonly int TABLE_ACCOUNT_ID = 8090;

        public static readonly string PENDING_SMS = Resources.lang.wait30;// "Please wait 30 more minutes to send sms again !";
        public static readonly string ALREADY_REQUEST_LOAN = Resources.lang.alreadyLoan;//"You already requested a loan!";
        public static readonly string KEY_IN_REQUIRED_FIELD = Resources.lang.required;// "Please key in all required field(s)!";

        public static readonly string INVALID_PIN = Resources.lang.invalidPin;//"Invalid PIN!";
        public static readonly string PIN_EXPIRED = Resources.lang.pinExpired;//"PIN Expired !";

        public static readonly string INVALID_PHONE = Resources.lang.invalidPhoneNumber;//"Invalid Phone Number !"; 

        public static readonly string PHONE_EXIST = Resources.lang.phoneExist;
        public static readonly string EMAIL_EXIST = Resources.lang.emailExist;

        public static readonly int ONE_EQUAL_ONE = 1;

    }
}