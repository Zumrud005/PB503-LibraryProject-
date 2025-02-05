using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.DTOs.BookDto
{
    public class BookUpdateDto
    {
        public string Title { get; set; } 
        public string Desc { get; set; } 
        public int PublishedYear { get; set; }
        public List<int> AuthorIds { get; set; } 
    }
}
