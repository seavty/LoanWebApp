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

        public static string ToDDMMYYYY(this DateTime? value)
        {
            if (value == null)
                return "";
            else {
                if (!string.IsNullOrEmpty(value.ToString()))
                    return DateTime.Parse(value.ToString()).ToString("dd/MM/yyyy");
                else
                    return "";
            }
        }
    }
}