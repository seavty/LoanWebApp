using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.Account
{
    public class AccountNewDTO: AccountBaseDTO
    {

        [Required]
        public double amount { get; set; }

        [Required]
        public int payDay { get; set; }

        [Required]
        public string loan_Purpose { get; set; }

        public string pin { get; set; }
    }
}