using LoanWebApp.Helpers;
using LoanWebApp.Models.DB;
using LoanWebApp.Models.DTO.Account;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace LoanWebApp.Handlers
{
    public class AccountHandler
    {
        private LoanEntities db = null;

        public AccountHandler()
        {
            db = new LoanEntities();
        }

        //-> SelectByID
        public async Task<AccountViewDTO> SelectByID(int id)
        {
            var account = await db.tblAccounts.FirstOrDefaultAsync(a => a.deleted == null && a.id == id);
            if (account == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");

            var accountView = DoubleHelper.TwoPrecision(MappingHelper.MapDBClassToDTO<tblAccount, AccountViewDTO>(account));
            accountView.statusCaption = SelectionHelper.AccountStatusCaption(account.status);
            accountView.documents = DocumentHelper.GetDocuments(db, ConstantHelper.TABLE_ACCOUNT_ID, account.id);
            return accountView;
        }

        //-> Create
        public async Task<AccountViewDTO> Create(AccountNewDTO accountDTO, HttpRequestBase Request)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    accountDTO = StringHelper.TrimStringProperties(accountDTO);
                    var account = (tblAccount)MappingHelper.MapDTOToDBClass<AccountNewDTO, tblAccount>(accountDTO, new tblAccount());
                    account.createdDate = DateTime.Now;
                    db.tblAccounts.Add(account);
                    await db.SaveChangesAsync();
                    List<sysDocument> documents = await DocumentHelper.SaveUploadFiles(db, ConstantHelper.TABLE_ACCOUNT_ID, account.id, Request);// tmp not useful , just reserve data for using in the furture

                    var loanRequest = await SaveToLoanRequest(accountDTO);
                    transaction.Commit();
                    return await SelectByID(account.id);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        //-> Check Number 
        public async Task<AccountViewDTO> CheckPhoneNumber(AccountCheckInfo checkPhoneNumber)
        {
            checkPhoneNumber = checkPhoneNumber.TrimStringProperties();
            var account = await db.tblAccounts.FirstOrDefaultAsync(a => a.deleted == null && a.phoneNumber == checkPhoneNumber.phoneNumber);
            if (account == null)
                return null;

            return await SelectByID(account.id);
        }

        /// private function 
        //-> 

        private async Task<tblLoanRequest> SaveToLoanRequest(AccountNewDTO accountDTO)
        {
            var loanRequest = new tblLoanRequest();
            db.tblLoanRequests.Add(loanRequest);
            loanRequest.createdDate = DateTime.Now;
            loanRequest.payDate = DateTime.Now;
            var interestRate = 0;
            switch (accountDTO.payDay)
            {
                case 10:
                    interestRate = 10;
                    break;
                case 15:
                    interestRate = 15;
                    break;
                case 30:
                    interestRate = 30;
                    break;
                default:
                    interestRate = 0;
                    break;
            }
            loanRequest.amount = Decimal.Parse(accountDTO.amount.ToString());
            loanRequest.interestRate = interestRate;
            loanRequest.interestAmount = Decimal.Parse((accountDTO.amount * interestRate / 100).ToString());
            loanRequest.loanAmount = Decimal.Parse((Decimal.Parse(accountDTO.amount.ToString()) + loanRequest.interestAmount).ToString());
            db.tblLoanRequests.Add(loanRequest);
            await db.SaveChangesAsync();
            return loanRequest;
        }
    }
}