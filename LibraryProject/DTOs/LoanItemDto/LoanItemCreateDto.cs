using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.DTOs.LoanItemDto
{
    public class LoanItemCreateDto
    {
        public int LoanId { get; set; } 
        public int BookId { get; set; } 
    }
}
