using LibraryProject.DTOs.LoanItemDto;
using LibraryProject.Models;
using LibraryProject.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Repositories.Implementation
{
    public class LoanItemRepository :GenericRepository<LoanItem> ,ILoanItemRepository
    {
        public List<LoanItemGetDto> IGetAll()
        {
            return _appDbContext.LoanItems
                .Include(li => li.Book)
                .Include(li => li.Loan)
                .Select(li => new LoanItemGetDto
                {
                    Id = li.Id,
                    LoanId = li.LoanId,
                    BookId = li.BookId,
                    BookTitle = li.Book.Title,
                    LoanDate = li.Loan.LoanDate
                })
                .ToList();
        }

        public LoanItemGetDto IGetById(int id)
        {
            return _appDbContext.LoanItems
                .Include(li => li.Book)
                .Include(li => li.Loan)
                .Where(li => li.Id == id)
                .Select(li => new LoanItemGetDto { 
                
                    Id = li.Id,
                    LoanId = li.LoanId,
                    BookId = li.BookId,
                    BookTitle = li.Book.Title,
                    LoanDate = li.Loan.LoanDate,
                    ReturnDate = null
                })
                .FirstOrDefault();
        }
        public void IDelete(int id)
        {
            var loanItem = _appDbContext.LoanItems.Find(id);
            if (loanItem != null)
            {
                _appDbContext.LoanItems.Remove(loanItem);
                _appDbContext.SaveChanges();
            }
        }


    }
}
