using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.LoanRequest
{
    public class LoanRequestNewDTO: LoanRequestBaseDTO
    {
        public int idCard_change { get; set; }
        public int employmentLetter_change { get; set; }
        public int bankAccount_change { get; set; }
    }
}