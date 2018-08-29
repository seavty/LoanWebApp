using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.Account
{
    public class AccountBaseDTO
    {
        public int? acct_AccountID { get; set; }

        [Required]
        [MaxLength(100)]
        public string acct_PhoneNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string acct_Email { get; set; }

        public string acct_FullName { get; set; }

        
        public Nullable<DateTime> acct_DOB { get; set; }
        public string acct_HouseNo { get; set; }
        public string acct_Street { get; set; }
        public string acct_Commune { get; set; }
        public string acct_District { get; set; }
        public string acct_Province { get; set; }
        public string acct_BankName { get; set; }
        public string acct_BackAccountName { get; set; }
        public Nullable<decimal> acct_Salary { get; set; }
        public string acct_Address { get; set; }
        
        public string acct_Remark1 { get; set; }
        public string acct_Remark2 { get; set; }
        public string acct_Remark3 { get; set; }
        public string acct_Verify1 { get; set; }
        public string acct_Verify2 { get; set; }
        public string acct_Verify3 { get; set; }
    }
}