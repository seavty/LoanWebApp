using LoanWebApp.Helpers;
using LoanWebApp.Models.DB;
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
    public class LoanRequestHandler
    {
        private LoanEntities db = null;

        public LoanRequestHandler()
        {
            db = new LoanEntities();
        }

        //-> SelectByID
        public async Task<LoanRequestViewDTO> SelectByID(int id)
        {
            var loanRequest = await db.tblLoanRequests.FirstOrDefaultAsync(l => l.deleted == null && l.id == id);
            if (loanRequest == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            return MappingHelper.MapDBClassToDTO<tblLoanRequest, LoanRequestViewDTO>(loanRequest);
        }

        //-> Create
        public async Task<LoanRequestViewDTO> Create(LoanRequestNewDTO loanRequestDTO)
        {
            IQueryable<tblLoanRequest>loanRequestQuery = from l in db.tblLoanRequests
                                                where l.deleted == null
                                                select l;
            int countLoanRequest = await loanRequestQuery.CountAsync();

            if (countLoanRequest > 1)
                throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.ALREADY_REQUEST_LOAN);


            loanRequestDTO = StringHelper.TrimStringProperties(loanRequestDTO);
            var loanRequest = (tblLoanRequest)MappingHelper.MapDTOToDBClass<LoanRequestNewDTO, tblLoanRequest>(loanRequestDTO, new tblLoanRequest());
            loanRequest.createdDate = DateTime.Now;
            loanRequest.payDate = DateTime.Now;
            //loanRequest.loanAmount =  Decimal.Parse((LoanRequestCalculation(loanRequestDTO)).ToString());
            loanRequest = LoanRequestCalculation(loanRequestDTO, loanRequest);
            db.tblLoanRequests.Add(loanRequest);
            await db.SaveChangesAsync();
            db.Entry(loanRequest).Reload();
            return await SelectByID(loanRequest.id);
        }

        //private function 
        private tblLoanRequest LoanRequestCalculation(LoanRequestNewDTO loanRequestDTO, tblLoanRequest loanRequest)
        {
            var interestRate = 0;
            switch (loanRequestDTO.payDay)
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
            loanRequest.interestRate = interestRate;
            loanRequest.interestAmount = Decimal.Parse((loanRequestDTO.amount * interestRate / 100).ToString());
            loanRequest.loanAmount = Decimal.Parse(( Decimal.Parse(loanRequestDTO.amount.ToString()) + loanRequest.interestAmount).ToString());

            return loanRequest;
        }
    }
}