using LibraryProject.DTOs.LoanItemDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Services.Interface
{
    public interface ILoanItemService
    {
        LoanItemGetDto GetByIdLoanItem(int id); 
        List<LoanItemGetDto> GetAll();
        void Create(LoanItemCreateDto loanItemCreateDto);
        void Update(int id,LoanItemUpdateDto loanItemUpdateDto);
        void Delete(int id);

    }
}
