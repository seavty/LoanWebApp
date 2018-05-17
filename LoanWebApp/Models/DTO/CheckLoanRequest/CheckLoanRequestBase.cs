using LoanWebApp.Models.DTO.Account;
using LoanWebApp.Models.DTO.LoanRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.CheckLoanRequest
{
    public class CheckLoanRequestBase
    {
        public AccountViewDTO account { get; set; }
        public LoanRequestViewDTO loanRequest { get; set; }
    }
}