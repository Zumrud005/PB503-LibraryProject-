﻿using LibraryProject.DTOs.BookDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.DTOs.BorrowerDto
{
    public class BorrowerGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<BookGetDto> BorrowedBooks { get; set; } = new List<BookGetDto>();
    }
}
