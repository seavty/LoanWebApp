﻿using LoanWebApp.Models.DTO.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.Account
{
    public class AccountViewDTO: AccountBaseDTO
    {
        public List<DocumentViewDTO> documents { get; set; }
    }
}