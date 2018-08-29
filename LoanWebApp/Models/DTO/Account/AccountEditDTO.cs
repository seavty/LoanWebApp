using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.Account
{
    public class AccountEditDTO
    {
        [Required]
        public int accountID { get; set; }

        [Required]
        [MaxLength(100)]
        public string idCard { get; set; }

        [Required]
        [MaxLength(100)]
        public string employmentLetter { get; set; }

        [Required]
        [MaxLength(100)]
        public string bankAccount { get; set; }

        public string acct_FullName { get; set; }
        public Nullable<System.DateTime> acct_DOB { get; set; }
        public string acct_HouseNo { get; set; }
        public string acct_Street { get; set; }
        public Nullable<int> acct_Commune { get; set; }
        public Nullable<int> acct_District { get; set; }
        public Nullable<int> acct_Province { get; set; }
        public string acct_BankName { get; set; }
        public string acct_BackAccountName { get; set; }
        public Nullable<decimal> acct_Salary { get; set; }
        public string acct_Address { get; set; }
    }
}