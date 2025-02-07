using LibraryProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Repositories.Interface
{
    public interface IBookRepository :IGenericRepository<Book>
    {
        List<Book> GetByIds(List<int> ids);
        Book GetByIdWithAuthors(int id);
        List<Book> GetAllWithAuthors();
        bool IsAvailable(int id);
        List<Author>? AuthorsSet(List<int> authorId);
        void RemoveBookAuthorRelations(Book book);
    }
}
