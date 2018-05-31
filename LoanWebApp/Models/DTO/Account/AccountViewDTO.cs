using LoanWebApp.Models.DTO.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.Account
{
    public class AccountViewDTO: AccountBaseDTO
    {
        public string idCard { get; set; }
        public string employmentLetter { get; set; }
        public string bankAccount { get; set; }
        public string status { get; set; }
        public string statusCaption { get; set; }
        public string reasonReject { get; set; }

        public List<DocumentViewDTO> documents { get; set; }

        public string name { get; set; }
    }
}