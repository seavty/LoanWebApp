using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanWebApp.Models.DTO
{
    public class MetaDataDTO
    {
        public int currentPage { get; set; }

        public int startRow { get; set; }
        public int endRow { get; set; }

        public int startPage { get; set; }
        public int endPage { get; set; }



        public int previousPage { get; set; }
        public string previousPageCssClass { get; set; }

        public int nextPage { get; set; }
        public string nextPageCssClass { get; set; }

        public string firstPageCssClass { get; set; }
        public string lastPageCssClass { get; set; }

        public int supplementRow { get; set; }


        public int pageSize { get; set; }
        public int totalPage { get; set; }
        public int totalRecord { get; set; }

        public String orderColumn { get; set; }
        public String orderBy { get; set; }
        public String search { get; set; }
    }
}