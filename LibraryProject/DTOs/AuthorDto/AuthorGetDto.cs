using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.DTOs.AuthorDto
{
    public class AuthorGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<BookDtos> Books { get; set; } = new();
    }
}
