using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanWebApp.Extension
{
    public static class DateTimeExtension
    {
        public static string ToDDMMYYYY(this DateTime value)
        {
            return value.ToString("dd/MM/yyyy");
        }
    }
}