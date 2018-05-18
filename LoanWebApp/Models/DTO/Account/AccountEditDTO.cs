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
    }
}