using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.Auth
{
    public class LoginDTO
    {
        [Required]
        [MaxLength(30)]
        [DisplayName("User Name: (*)")]
        public string userName { get; set; }

        [Required]
        [MaxLength(30)]
        [DisplayName("Password: (*)")]
        public string password { get; set; }
    }
}