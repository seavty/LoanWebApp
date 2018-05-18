using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.Account
{
    public class AccountBaseDTO
    {
        public int? id { get; set; }

        [Required]
        [MaxLength(100)]
        public string phoneNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string email { get; set; }
    }
}