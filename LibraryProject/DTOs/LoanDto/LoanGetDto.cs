using LibraryProject.DTOs.LoanItemDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.DTOs.LoanDto
{
    public class LoanGetDto
    {
        public int Id { get; set; }
        public int BorrowerId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime MustReturnDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public List<LoanItemGetDto> LoanItems { get; set; } = new();
    }
}
