using LibraryProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Repositories.Interface
{
    public interface IAuthorRepository :IGenericRepository<Author>
    {
        List<Author> GetByIds(List<int> ids); 
        Author GetByIdWithBooks(int id); 
        List<Author> GetAllWithBooks();
        void RemoveBookAuthorRelations(Author author);
        List<Book>? BooksSet(List<int> bookId);
    }
}
