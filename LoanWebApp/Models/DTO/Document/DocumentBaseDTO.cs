using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO.Document
{
    public class DocumentBaseDTO
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public string fullPath { get; set; }
    }
}