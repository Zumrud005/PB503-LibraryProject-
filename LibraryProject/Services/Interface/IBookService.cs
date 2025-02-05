using LibraryProject.DTOs.BookDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Services.Interface
{
    public interface IBookService
    {
        void Add(BookCreateDto bookDto);
        void Update(int id, BookUpdateDto bookDto);
        void Delete(int id);
        BookGetDto GetBookById(int id);
        List<BookGetDto> GetAllBooks();
    }
}
