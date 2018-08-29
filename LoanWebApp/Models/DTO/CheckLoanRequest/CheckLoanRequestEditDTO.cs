using LoanWebApp.Models.DTO.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.CheckLoanRequest
{
    public class CheckLoanRequestEditDTO
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
        
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
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
        
        public string acct_Remark1 { get; set; }
        public string acct_Remark2 { get; set; }
        public string acct_Remark3 { get; set; }
        public string acct_Verify1 { get; set; }
        public string acct_Verify2 { get; set; }
        public string acct_Verify3 { get; set; }
    }
}