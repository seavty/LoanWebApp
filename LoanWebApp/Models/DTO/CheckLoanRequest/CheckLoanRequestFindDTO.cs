using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.CheckLoanRequest
{
    public class CheckLoanRequestFindDTO
    {
        public int currentPage { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string status { get; set; }
    }

}