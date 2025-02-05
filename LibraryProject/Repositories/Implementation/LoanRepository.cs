using LibraryProject.Models;
using LibraryProject.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Repositories.Implementation
{
    public class LoanRepository :GenericRepository<Loan>, ILoanRepository
    {
    }
}
