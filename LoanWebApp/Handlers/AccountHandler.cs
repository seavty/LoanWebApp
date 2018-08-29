using LoanWebApp.Helpers;
using LoanWebApp.Models.DB;
using LoanWebApp.Models.DTO.Account;
using LoanWebApp.Models.DTO.LoanRequest;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace LoanWebApp.Handlers
{
    public class AccountHandler
    {
        private LoanEntities db = null;
        const int _ErrorCode = 400;
        public AccountHandler()
        {
            db = new LoanEntities();
        }

        //-> SelectByID
        public async Task<AccountViewDTO> SelectByID(int id)
        {
            var account = await db.tblAccounts.FirstOrDefaultAsync(a => a.acct_Deleted == null && a.acct_AccountID == id);
            if (account == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");

            var accountView = DoubleHelper.TwoPrecision(MappingHelper.MapDBClassToDTO<tblAccount, AccountViewDTO>(account));
            accountView.statusCaption = SelectionHelper.AccountStatusCaption(account.acct_Status);
            accountView.documents = DocumentHelper.GetDocuments(db, ConstantHelper.TABLE_ACCOUNT_ID, account.acct_AccountID);
            return accountView;
        }

        /*
        public async Task<AccountViewDTO> reupload(int id, HttpRequestBase Request)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {

                    List<sm_doc> documents = await DocumentHelper.SaveUploadFiles(db, ConstantHelper.TABLE_ACCOUNT_ID, id, Request);// tmp not useful , just reserve data for using in the furture
                    return null;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
        */
        public async Task<AccountViewDTO> reupload(AccountReuploadDocumentDTO reuploadDoc, HttpRequestBase Request)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<sm_doc> documents = await DocumentHelper.SaveUploadFiles(db, ConstantHelper.TABLE_ACCOUNT_ID, reuploadDoc.id, Request);// tmp not useful , just reserve data for using in the furture

                    IQueryable<sm_doc> query = from d in db.sm_doc
                                               where d.docs_Deleted == null && d.docs_TableID == ConstantHelper.TABLE_ACCOUNT_ID && d.docs_Value == reuploadDoc.id.ToString()
                                               orderby d.docs_docID
                                               select d;

                    List<sm_doc> records = await query.Take(3).ToListAsync();

                    int id = 0;
                    if (reuploadDoc.idCard_change == 1)
                    {
                        id = records[0].docs_docID;
                        var tmp = await db.sm_doc.FirstOrDefaultAsync(x => x.docs_Deleted == null && x.docs_docID == id);
                        tmp.docs_Deleted = "1";
                        await db.SaveChangesAsync();
                    }

                    if (reuploadDoc.employmentLetter_change == 1)
                    {
                        id = records[1].docs_docID;
                        var tmp = await db.sm_doc.FirstOrDefaultAsync(x => x.docs_Deleted == null && x.docs_docID == id);
                        tmp.docs_Deleted = "1";
                        await db.SaveChangesAsync();
                    }

                    if (reuploadDoc.bankAccount_change == 1)
                    {
                        id = records[2].docs_docID;
                        var tmp = await db.sm_doc.FirstOrDefaultAsync(x => x.docs_Deleted == null && x.docs_docID == id);
                        tmp.docs_Deleted = "1";
                        await db.SaveChangesAsync();
                    }

                    transaction.Commit();
                    return null;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }




        //-> Create First Time : no account created yet
        public async Task<AccountViewDTO> Create(AccountNewDTO accountDTO, HttpRequestBase Request)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var tblPin = db.tblPins.FirstOrDefault(x => x.pins_Phone == accountDTO.acct_PhoneNumber &&
                    x.pins_isUsed == null && x.pins_Name == accountDTO.pin);
                    if (tblPin == null)
                    {
                        throw new HttpException(_ErrorCode, ConstantHelper.INVALID_PIN);
                    }
                    else
                    {
                        if (DateTime.Now.Subtract(tblPin.pins_Date.Value).Minutes > 3)
                        {
                            throw new HttpException(_ErrorCode, ConstantHelper.PIN_EXPIRED);
                        }
                    }

                    tblAccount eAcc = db.tblAccounts.FirstOrDefault(x => x.acct_PhoneNumber.Trim() == accountDTO.acct_PhoneNumber.Trim());
                    if(eAcc != null)
                    {
                        throw new HttpException(_ErrorCode, ConstantHelper.PHONE_EXIST);
                    }

                    eAcc = db.tblAccounts.FirstOrDefault(x => x.acct_Email.Trim() == accountDTO.acct_Email.Trim());
                    if (eAcc != null)
                    {
                        throw new HttpException(_ErrorCode, ConstantHelper.EMAIL_EXIST);
                    }

                    tblPin.pins_isUsed = "Y";

                    accountDTO = StringHelper.TrimStringProperties(accountDTO);
                    var account = (tblAccount)MappingHelper.MapDTOToDBClass<AccountNewDTO, tblAccount>(accountDTO, new tblAccount());
                    account.acct_CreatedDate = DateTime.Now;
                    account.acct_Status = "Pending";

                    db.tblAccounts.Add(account);
                    await db.SaveChangesAsync();
                    List<sm_doc> documents = await DocumentHelper.SaveUploadFiles(db, ConstantHelper.TABLE_ACCOUNT_ID, account.acct_AccountID, Request);// tmp not useful , just reserve data for using in the furture

                    accountDTO.acct_AccountID = account.acct_AccountID;
                    var loanRequest = await SaveToLoanRequest(accountDTO);
                    transaction.Commit();
                    await db.Entry(loanRequest).ReloadAsync();
                    sendmail(loanRequest, account);
                    return await SelectByID(account.acct_AccountID);
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
            var account = await db.tblAccounts.FirstOrDefaultAsync(a => a.acct_Deleted == null && a.acct_PhoneNumber == checkPhoneNumber.phoneNumber);
            if (account == null)
                return null;

            return await SelectByID(account.acct_AccountID);
        }

        // Create PIN
        public async Task<bool> createPin(int id, string phone)
        {
            string _phone = "";
            bool re = false;
            if (id != 0)
            {
                var account = await db.tblAccounts.FirstOrDefaultAsync(a => a.acct_Deleted == null && a.acct_AccountID == id);
                if (account == null)
                    throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");

                if (account.acct_PhoneNumber != phone)
                    throw new HttpException(_ErrorCode, ConstantHelper.INVALID_PHONE);
                _phone = account.acct_PhoneNumber;
            }
            else
            {
                tblAccount eAcc = db.tblAccounts.FirstOrDefault(x => x.acct_PhoneNumber.Trim() == phone.Trim());
                if (eAcc != null)
                {
                    throw new HttpException(_ErrorCode, ConstantHelper.PHONE_EXIST);
                }
                _phone = phone;
            }

            var loan = db.tblLoanRequests.FirstOrDefault(x => x.loan_AccountID == id &&
                x.loan_Status.ToLower() != "rejected" && x.loan_Status.ToLower() != "approve" && x.loan_Balance > 0);
            if (loan != null)
                throw new HttpException(_ErrorCode, ConstantHelper.ALREADY_REQUEST_LOAN);
            

            var dt = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            var ePin = db.tblPins.Where(x => x.pins_Deleted == null &&
                (id != 0 ? x.pins_AccountID == id : x.pins_Phone == phone) && 
                x.pins_Date >= dt
            ).OrderByDescending(o => o.pins_Date).Take(2);
            if (ePin != null)
            {
                DateTime dt1 = DateTime.Now;
                DateTime dt2 = DateTime.Now;
                int i = 0;
                foreach (var _pin in ePin)
                {
                    if (i == 0)
                    {
                        dt1 = _pin.pins_Date.Value;
                        var tt = DateTime.Now.Subtract(dt1).Minutes + (DateTime.Now.Subtract(dt1).Hours * 60);
                        if (tt > 30)
                            break;
                    }
                    if (i == 1)
                    {
                        dt2 = _pin.pins_Date.Value;
                    }
                    i++;
                }
                if (i > 1)
                {
                    var a = dt1.Subtract(dt2).Minutes;
                    var b = dt1.Subtract(dt1).Hours;
                    if (dt1.Subtract(dt2).Minutes + (dt1.Subtract(dt2).Hours * 60) < 30)
                    {
                        throw new HttpException(_ErrorCode, ConstantHelper.PENDING_SMS);
                        //var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        //response.Content = new StringContent(ConstantHelper.PENDING_SMS);");

                    }
                }
            }
            tblPin pin = new tblPin();
            pin.pins_Date = DateTime.Now;
            if (id != 0)
                pin.pins_AccountID = id;
            else
                pin.pins_Phone = phone;
            pin.pins_Name = GeneratePIN();
            db.tblPins.Add(pin);
            db.SaveChanges();

            //
            if (!string.IsNullOrEmpty(_phone))
            {
                using (HttpClient client = new HttpClient())
                {
                    var res = await client.GetAsync("http://api.mekongsms.com/api/sendsms.aspx?username=xware@mekongnet&pass=ea6fac133e078db85a25de53ad18a0e3&sender=MarielBank&smstext=" +
                        "Dear valued customer,\nHere is your pin : " + pin.pins_Name +
                        "&isflash=0&gsm=" + _phone +
                        "\n\nMarielBank");
                    var status = await res.Content.ReadAsStringAsync();
                    if (status.Length > 0)
                    {
                        if (status.Substring(0, 1) == "0")
                        {
                            return true;
                        }
                    }
                };
            }
            re = false;
            return re;

        }

        private string GeneratePIN()
        {
            Random _random = new Random();
            return _random.Next(0, 9999).ToString("D4");
        }

        //-> CreateLoanRequest
        public async Task<AccountViewDTO> CreateLoanRequest(LoanRequestNewDTO loanRequestDTO)
        {
            var account = await db.tblAccounts.FirstOrDefaultAsync(a => a.acct_Deleted == null && a.acct_AccountID == loanRequestDTO.accountID);
            if (account == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");


            var loan = db.tblLoanRequests.FirstOrDefault(x => x.loan_AccountID == loanRequestDTO.accountID &&
            x.loan_Status.ToLower() != "rejected" && x.loan_Status.ToLower() != "approve" && x.loan_Balance > 0);
            if (loan != null)
                throw new HttpException(_ErrorCode, ConstantHelper.ALREADY_REQUEST_LOAN);

            var tblPin = db.tblPins.FirstOrDefault(x => x.pins_AccountID == account.acct_AccountID &&
            x.pins_isUsed == null && x.pins_Name == loanRequestDTO.pin);
            if (tblPin == null)
            {
                throw new HttpException(_ErrorCode, ConstantHelper.INVALID_PIN);
            }
            else
            {
                if (DateTime.Now.Subtract(tblPin.pins_Date.Value).Minutes > 3)
                {
                    throw new HttpException(_ErrorCode, ConstantHelper.PIN_EXPIRED);
                }
            }

            tblPin.pins_isUsed = "Y";

            loanRequestDTO = StringHelper.TrimStringProperties(loanRequestDTO);
            var loanRequest = await SaveToLoanRequest(
                loanRequestDTO.payDay,
                loanRequestDTO.amount,
                loanRequestDTO.accountID,
                loanRequestDTO.loan_Purpose);
            await db.Entry(loanRequest).ReloadAsync();
            sendmail(loanRequest, account);
            return await SelectByID(int.Parse(loanRequest.loan_AccountID.ToString()));

        }

        /// private function 
        //-> 
        private async Task<tblLoanRequest> SaveToLoanRequest(int payDay, double amount, int accountID, string purpose)
        {
            var loanRequest = new tblLoanRequest();
            loanRequest.loan_CreatedDate = DateTime.Now;
            loanRequest.loan_PayDate = DateTime.Now;
            var interestRate = 0;
            switch (payDay)
            {
                case 10:
                    interestRate = 10;
                    loanRequest.loan_PayDate = DateTime.Now.AddDays(10);
                    break;
                case 20:
                    interestRate = 15;
                    loanRequest.loan_PayDate = DateTime.Now.AddDays(20);
                    break;
                case 30:
                    interestRate = 20;
                    loanRequest.loan_PayDate = DateTime.Now.AddDays(30);
                    break;
                default:
                    interestRate = 0;
                    break;
            }
            loanRequest.loan_AccountID = accountID;
            loanRequest.loan_PayDay = payDay;
            loanRequest.loan_Amount = Decimal.Parse(amount.ToString());
            loanRequest.loan_InterestRate = interestRate;
            loanRequest.loan_InterestAmount = Decimal.Parse((amount * interestRate / 100).ToString());
            loanRequest.loan_LoanAmount = Decimal.Parse((Decimal.Parse(amount.ToString()) + loanRequest.loan_InterestAmount).ToString());
            loanRequest.loan_Purpose = purpose;
            loanRequest.loan_Status = "Pending";
            loanRequest.loan_Balance = loanRequest.loan_LoanAmount;
            loanRequest.loan_PaidAmount = 0;
            db.tblLoanRequests.Add(loanRequest);
            await db.SaveChangesAsync();
            return loanRequest;
        }



        private async Task<tblLoanRequest> SaveToLoanRequest(AccountNewDTO accountDTO)
        {
            var loanRequest = new tblLoanRequest();
            //db.tblLoanRequests.Add(loanRequest);
            loanRequest.loan_CreatedDate = DateTime.Now;
            loanRequest.loan_PayDate = DateTime.Now;
            var interestRate = 0;
            switch (accountDTO.payDay)
            {
                case 10:
                    loanRequest.loan_PayDate = DateTime.Now.AddDays(10);
                    interestRate = 10;
                    break;
                case 20:
                    loanRequest.loan_PayDate = DateTime.Now.AddDays(20);
                    interestRate = 15;
                    break;
                case 30:
                    loanRequest.loan_PayDate = DateTime.Now.AddDays(30);
                    interestRate = 20;
                    break;
                default:
                    interestRate = 0;
                    break;
            }
            loanRequest.loan_AccountID = accountDTO.acct_AccountID;
            loanRequest.loan_PayDay = accountDTO.payDay;
            loanRequest.loan_Amount = Decimal.Parse(accountDTO.amount.ToString());
            loanRequest.loan_InterestRate = interestRate;
            loanRequest.loan_InterestAmount = Decimal.Parse((accountDTO.amount * interestRate / 100).ToString());
            loanRequest.loan_LoanAmount = Decimal.Parse((Decimal.Parse(accountDTO.amount.ToString()) + loanRequest.loan_InterestAmount).ToString());
            loanRequest.loan_Purpose = accountDTO.loan_Purpose;
            loanRequest.loan_Status = "Pending";
            loanRequest.loan_Balance = loanRequest.loan_LoanAmount;
            loanRequest.loan_PaidAmount = 0;
            db.tblLoanRequests.Add(loanRequest);
            await db.SaveChangesAsync();
            return loanRequest;
        }

        private void sendmail(tblLoanRequest loanRequest,tblAccount account)
        {
            DocumentHelper.sendEmail("Request -Loan ID " + loanRequest.loan_Code,
                "Dear Sirs, " +
                "<br/>Your money will be transferred soon as below: " +
                //"Bank account name xxxx" +
                //"Bank account number xxxx" +
                "<br/>Principle " + loanRequest.loan_Amount.Value.ToString("#,##0.00") +
                "<br/>Repayment Date " + loanRequest.loan_PayDate.Value.ToString("dd/MM/yyyy") +
                "<br/>Repayment Amount " + loanRequest.loan_LoanAmount.Value.ToString("#,##0.00") +
                "<br/>Loan ID " + loanRequest.loan_Code, account.acct_Email, isHtml: true);

            DocumentHelper.sendEmail("New Loan Request -Loan ID " + loanRequest.loan_Code,
                "Dear Admin, " +
                "<br/>New loan request info : " +
                "<br/>Principle " + loanRequest.loan_Amount.Value.ToString("#,##0.00") +
                "<br/>Repayment Date " + loanRequest.loan_PayDate.Value.ToString("dd/MM/yyyy") +
                "<br/>Repayment Amount " + loanRequest.loan_LoanAmount.Value.ToString("#,##0.00") +
                "<br/>Loan ID " + loanRequest.loan_Code, 
                System.Configuration.ConfigurationManager.AppSettings["smtpuser"] , isHtml: true);
        }
    }
}