using LibraryProject.DTOs.BorrowerDto;
using LibraryProject.DTOs.LoanDto;
using LibraryProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Services.Interface
{
    public interface ILoanService
    {
        LoanGetDto GetById(int id);
        List<LoanGetDto> GetAll();
        LoanGetDto Create(LoanCreateDto loanCreateDto);
        void Update(int id, LoanUpdateDto loanUpdateDto);
        void CreateLoan(Loan loan);
        void ReturnBook(int borrowerId);
        bool IsBookAvailable(int bookId);

        List<Borrower> GetDelayedBorrowers();
     
    }
}
