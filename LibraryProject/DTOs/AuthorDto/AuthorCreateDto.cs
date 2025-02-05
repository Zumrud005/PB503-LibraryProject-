using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.DTOs.AuthorDto
{
    public class AuthorCreateDto
    {
        public string Name { get; set; } = string.Empty;
        //public List<int> BookIds { get; set; } = new();
    }
}
