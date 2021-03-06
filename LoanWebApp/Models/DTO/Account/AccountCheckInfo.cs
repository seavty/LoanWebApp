﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.Account
{
    public class AccountCheckInfo
    {
        [Required]
        [MaxLength(100)]
        [DisplayName("Type number to Submit (*):")]
        public string phoneNumber { get; set; }
    }
}