using LibraryProject.DTOs.AuthorDto;
using LibraryProject.DTOs.BookDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Services.Interface
{
    public interface IAuthorService
    {
        void Add(AuthorCreateDto authorDto);
        void Update(int id, AuthorUpdateDto authorDto);
        void Delete(int id);
        AuthorGetDto GetAuthorById(int id);
        List<AuthorGetDto> GetAllAuthors();
    }
}
