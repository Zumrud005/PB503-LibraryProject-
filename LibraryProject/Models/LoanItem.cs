using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Models
{
    public class LoanItem :BaseEntity
    {
     
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int LoanId { get; set; }
        public Loan Loan { get; set; }

    }
}
