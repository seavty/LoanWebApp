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

            var accountView = MappingHelper.MapDBClassToDTO<tblAccount, AccountViewDTO>(account);
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
    }
}