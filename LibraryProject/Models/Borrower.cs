using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Models
{
    public class Borrower :BaseEntity
    {
        public string Name { get; set; }
        public int PublishedYear { get; set; }
        public List<Loan> Loans { get; set; }

    }
}
