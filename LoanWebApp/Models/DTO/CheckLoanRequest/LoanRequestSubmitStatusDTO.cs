using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.CheckLoanRequest
{
    public class LoanRequestSubmitStatusDTO
    {
        [Required]
        public int accountID { get; set; }

        [Required]
        [MaxLength(100)]
        public string status { get; set; }

        [MaxLength(1000)]
        public string reasonReject { get; set; }
    }
}