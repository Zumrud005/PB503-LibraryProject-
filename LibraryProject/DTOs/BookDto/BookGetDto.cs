using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.DTOs.BookDto
{
    public class BookGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; } 
        public string Desc { get; set; }
        public int PublishedYear { get; set; }
        public List<AuthorDtos> Authors { get; set; }
    }
}
