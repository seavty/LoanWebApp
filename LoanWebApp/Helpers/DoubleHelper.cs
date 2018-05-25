using LoanWebApp.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace LoanWebApp.Helpers
{
    public static class DoubleHelper
    {
        
        public static TSelf TwoPrecision<TSelf>(this TSelf input)
        {
            if (input == null)
                return input;

            var doubleProperties = typeof(TSelf).GetProperties()
                .Where(p => p.PropertyType == typeof(double));

            foreach (var doubleProperty in doubleProperties)
            {
                double currentValue = (double)doubleProperty.GetValue(input, null);
                doubleProperty.SetValue(input, Math.Truncate(100 * currentValue) / 100, null);
                
            }
            return input;
        }
    }
}