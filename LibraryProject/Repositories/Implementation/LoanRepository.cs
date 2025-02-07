using LibraryProject.Models;
using LibraryProject.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Repositories.Implementation
{
    public class LoanRepository : GenericRepository<Loan>, ILoanRepository
    {
        public List<Loan> GetAll()
        {

            return _appDbContext.Loans
                                .Include(l => l.LoanItems)
                                .ToList();

        }
    }
}
