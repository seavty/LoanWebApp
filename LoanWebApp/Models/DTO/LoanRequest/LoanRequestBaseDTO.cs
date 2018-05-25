using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.LoanRequest
{
    public class LoanRequestBaseDTO
    {
        public int? id { get; set; }

        [Required]
        public double amount { get; set; }

        [Required]
        public int payDay { get; set; }

        [Required]
        public int accountID { get; set; }

    }
}