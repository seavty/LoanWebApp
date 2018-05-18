using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanWebApp.Helpers
{
    public static class SelectionHelper
    {

        public static string AccountStatusCaption(string status)
        {
            switch (status)
            {
                case "SubmitForApproval":
                    return "Submit For Approval";
                case "Pending":
                case "Approved":
                case "Rejected":
                    return status;
                default:
                    return "";
            }
        }
    }
}