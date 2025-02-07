using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.DTOs.LoanDto
{
    public class LoanCreateDto
    {
        public int BorrowerId { get; set; }
        public List<int> BookIds { get; set; } = new();
    }
}
