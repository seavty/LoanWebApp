using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.Account
{
    public class AccountReuploadDocumentDTO
    {
        public int id { get; set; }
        public int idCard_change { get; set; }
        public int employmentLetter_change { get; set; }
        public int bankAccount_change { get; set; }
    }
}