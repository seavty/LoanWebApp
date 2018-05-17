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
            loanRequestDTO = StringHelper.TrimStringProperties(loanRequestDTO);
            var loanRequest = (tblLoanRequest)MappingHelper.MapDTOToDBClass<LoanRequestNewDTO, tblLoanRequest>(loanRequestDTO, new tblLoanRequest());
            loanRequest.createdDate = DateTime.Now;
            loanRequest.payDate = DateTime.Now;
            loanRequest.loanAmount =  Decimal.Parse((LoanAmount(loanRequestDTO)).ToString());
            db.tblLoanRequests.Add(loanRequest);
            await db.SaveChangesAsync();
            db.Entry(loanRequest).Reload();
            return await SelectByID(loanRequest.id);
        }

        //private function 
        private Double LoanAmount(LoanRequestNewDTO loanRequest)
        {
            var interest = 0;
            switch (loanRequest.payDay)
            {
                case 10:
                    interest = 10;
                    break;
                case 15:
                    interest = 15;
                    break;
                case 30:
                    interest = 30;
                    break;
                default:
                    interest = 0;
                    break;
            }
            return loanRequest.amount + (loanRequest.amount * interest / 100);
        }
    }
}