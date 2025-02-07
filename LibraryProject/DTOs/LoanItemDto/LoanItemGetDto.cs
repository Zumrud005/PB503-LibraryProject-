using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.DTOs.LoanItemDto
{
    public class LoanItemGetDto
    {
        public int Id { get; set; }
        public int? LoanId { get; set; }
        public int BookId { get; set; }
        public string? BookTitle { get; set; } 
        public DateTime? LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
