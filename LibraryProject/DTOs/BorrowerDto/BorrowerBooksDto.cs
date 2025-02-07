using LibraryProject.DTOs.BookDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.DTOs.BorrowerDto
{
    public class BorrowerBooksDto
    {
        public int BorrowerId { get; set; }
        public string BorrowerName { get; set; }
        public List<BookGetDto> BorrowedBooks { get; set; }
    }
}
