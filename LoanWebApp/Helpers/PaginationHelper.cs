using LoanWebApp.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LoanWebApp.Helpers
{
    public static class PaginationHelper
    {
        public static readonly int PAGE_SIZE = 10;
        public static readonly int PAGE_SIZE_MASTER_DETAIL_List = 10;
        public static readonly int NUMBER_OF_PAGING_IN_PAGINATION_BAR = 5;

        public static readonly string PAGINATION_DISABLE_CLASS = "disabled";


        //->  GetStartRow
        private static int GetStartRow(int currentPage, bool isMasterDetailList = false)
        {
            int startRow = 0;
            if (currentPage > 1)
            {
                int myPageSize = PAGE_SIZE;
                if (isMasterDetailList)
                    myPageSize = PAGE_SIZE_MASTER_DETAIL_List;
                startRow = (currentPage - 1) * myPageSize;
            }
            return startRow;
        }
        
        //->  get list 
        public static IQueryable<dynamic> GetList(int currentPage, IQueryable<dynamic> records)
        {
            return records.Skip(GetStartRow(currentPage)).Take(PAGE_SIZE);
        }

        //-> GetMetaData
        public static async Task<MetaDataDTO> GetMetaData(int currentPage, IQueryable<dynamic> records, string orderColumn, string orderBy, string search, bool isMasterDetailList = false)
        {
            int myPageSize = PAGE_SIZE;
            if (isMasterDetailList)
                myPageSize = PAGE_SIZE_MASTER_DETAIL_List;

            int totalRecord = await records.CountAsync();
            double getTotalPage = ((double)totalRecord / myPageSize);
            int totalPage = (int)Math.Ceiling(getTotalPage);

            if (currentPage < 1 || currentPage > totalPage)
                currentPage = 0;

            var metaData = new MetaDataDTO();
            metaData.currentPage = currentPage;

            //-- start row
            if (currentPage > 0)
                metaData.startRow = GetStartRow(currentPage) + 1;
            
            //-- end row 
            int endRow = currentPage * myPageSize;
            if (endRow > totalRecord)
                endRow = totalRecord;
            metaData.endRow = endRow;

            metaData.startPage = GetStartPage(currentPage);
            metaData.endPage = GetEndPage( GetStartPage(currentPage), totalPage);

            //-- first page
            if (currentPage == 0 || currentPage == 1)
                metaData.firstPageCssClass = PAGINATION_DISABLE_CLASS;

            //-- last page
            if (currentPage == totalPage)
                metaData.lastPageCssClass = PAGINATION_DISABLE_CLASS;

            //-- previous page 
            if (currentPage == 0 || currentPage == 1)
                metaData.previousPageCssClass = PAGINATION_DISABLE_CLASS;
            else
                metaData.previousPage = currentPage - 1;

            //-- next page
            if (currentPage == totalPage)
                metaData.nextPageCssClass = PAGINATION_DISABLE_CLASS;
            else
                metaData.nextPage = currentPage + 1;
           
            //--supplment row 
            if (currentPage == totalPage)
                metaData.supplementRow = currentPage * PAGE_SIZE - totalRecord ;

            metaData.pageSize = myPageSize;
            metaData.totalPage = totalPage;
            metaData.totalRecord = totalRecord;
            metaData.orderColumn = orderColumn;
            metaData.orderBy = orderBy;
            metaData.search = search;

            return metaData;
        }

        //-- private function --//

        //-> GetStartPage
        private static int GetStartPage(int currentPage)
        {
            int startPage = 1;
            if (currentPage > NUMBER_OF_PAGING_IN_PAGINATION_BAR)
            {
                if (currentPage % NUMBER_OF_PAGING_IN_PAGINATION_BAR == 0)
                    startPage = currentPage - NUMBER_OF_PAGING_IN_PAGINATION_BAR + 1;
                else
                    startPage = ( currentPage / NUMBER_OF_PAGING_IN_PAGINATION_BAR )  * NUMBER_OF_PAGING_IN_PAGINATION_BAR + 1;
            }
            return startPage;
        }

        private static int GetEndPage(int startPage, int totalPage)
        {
            if (totalPage <= NUMBER_OF_PAGING_IN_PAGINATION_BAR)
                return totalPage;
            else
                return startPage + NUMBER_OF_PAGING_IN_PAGINATION_BAR - 1;
        }
    }
}