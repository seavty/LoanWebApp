using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanWebApp.Helpers
{
    public static class StringHelper
    {
        public static TSelf TrimStringProperties<TSelf>(this TSelf input)
        {
            if (input == null)
                return input;

            var stringProperties = typeof(TSelf).GetProperties()
                .Where(p => p.PropertyType == typeof(string));

            foreach (var stringProperty in stringProperties)
            {
                string currentValue = (string)stringProperty.GetValue(input, null);
                if (currentValue != null)
                    stringProperty.SetValue(input, currentValue.Trim(), null);
            }
            return input;
        }

        public static string str2Date(string date)
        {
            string re = "";
            try
            {
                var tmp = date.Split('/');
                re = tmp[2] + "/" + tmp[1] + "/" + tmp[0];
            }
            catch (Exception ex) { return ""; }
            return re;
        }
    }
}