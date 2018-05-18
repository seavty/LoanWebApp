﻿using LoanWebApp.Helpers;
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

        //-> Save
        public async Task<CheckLoanRequestViewDTO> Save(CheckLoanRequestEditDTO checkLoanRequest)
        {
            checkLoanRequest = StringHelper.TrimStringProperties(checkLoanRequest);

            tblAccount account = await db.tblAccounts.FirstOrDefaultAsync(a => a.deleted == null && a.id == checkLoanRequest.accountID);
            if (account == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "This record has been deleted");

            account = (tblAccount)MappingHelper.MapDTOToDBClass<CheckLoanRequestEditDTO, tblAccount>(checkLoanRequest, account);
            account.updatedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return await SelectByID(account.id);
        }

        //-> Save
        public async Task<CheckLoanRequestViewDTO> SubmitForApproval(int accountID, string status)
        {
            tblAccount account = await db.tblAccounts.FirstOrDefaultAsync(a => a.deleted == null && a.id == accountID);
            if (account == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "This record has been deleted");

            account.updatedDate = DateTime.Now;
            account.status = status;

            //-- update submit for approval status here
            //-- in Check Loan request form add status ; pending; submit for approval; approved, reject
            await db.SaveChangesAsync();
            return await SelectByID(account.id);
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