using LoanWebApp.Helpers;
using LoanWebApp.Models.DB;
using LoanWebApp.Models.DTO;
using LoanWebApp.Models.DTO.CheckLoanRequest;
using LoanWebApp.Models.DTO.LoanRequest;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace LoanWebApp.Handlers
{
    public class CheckLoanRequestHandler
    {
        private LoanEntities db = null;

        public CheckLoanRequestHandler()
        {
            db = new LoanEntities();
        }

        //-> SelectByID
        public async Task<CheckLoanRequestViewDTO> SelectByID(int id)
        {
            var accountHandler = new AccountHandler();
            var checkLoanRequest = new CheckLoanRequestViewDTO();
            checkLoanRequest.account = await accountHandler.SelectByID(id);

            var loanRequest = await db.tblLoanRequests.FirstOrDefaultAsync(l => l.deleted == null && l.accountID == id);
            if (loanRequest == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            checkLoanRequest.loanRequest = DoubleHelper.TwoPrecision(MappingHelper.MapDBClassToDTO<tblLoanRequest, LoanRequestViewDTO>(loanRequest));
            return DoubleHelper.TwoPrecision(checkLoanRequest);
        }

        //-> GetList
        public async Task<GetListDTO<CheckLoanRequestViewDTO>> GetList(int currentPage)
        {
            IQueryable<tblAccount> accounts = from a in db.tblAccounts
                                              where a.deleted == null
                                              orderby a.id ascending
                                              select a;
            return await Listing(currentPage, accounts);
        }

        //*** private method ***/
        private async Task<GetListDTO<CheckLoanRequestViewDTO>> Listing(int currentPage, IQueryable<tblAccount> accounts, string search = null)
        {
            var checkLoanRequests = new List<CheckLoanRequestViewDTO>();
            foreach (var account in PaginationHelper.GetList(currentPage, accounts))
            {
                var checkLoanRequest = new CheckLoanRequestViewDTO();
                var accountHandler = new AccountHandler();
                checkLoanRequest.account =  await accountHandler.SelectByID(account.id);
                checkLoanRequests.Add(checkLoanRequest);
            }

            var getList = new GetListDTO<CheckLoanRequestViewDTO>();
            getList.metaData = await PaginationHelper.GetMetaData(currentPage, accounts, "id", "asc", search);
            getList.items = checkLoanRequests;
            return getList;
        }

    }
}