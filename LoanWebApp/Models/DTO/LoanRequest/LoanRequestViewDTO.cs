using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.LoanRequest
{
    public class LoanRequestViewDTO: LoanRequestBaseDTO
    {
        public double loanAmount { get; set; }
        public double interestRate { get; set; }
        public double interestAmount { get; set; }

        public DateTime payDate { get; set; }
    }
}