using LibraryProject.Data;
using LibraryProject.Models;
using LibraryProject.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Repositories.Implementation
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
       
        public List<Book> GetAllWithAuthors()
         => _appDbContext.Books.Include(x => x.Authors).ToList();

        public List<Book> GetByIds(List<int> ids)
        => _appDbContext.Books.Where(x => ids.Contains(x.Id)).ToList();

        public Book GetByIdWithAuthors(int id)
        => _appDbContext.Books.Include(x => x.Authors).FirstOrDefault(x => x.Id == id);

        public bool IsAvailable(int id)
        => !_appDbContext.LoanItems.Any(li => li.BookId == id && li.Loan.ReturnDate == null);

        public void RemoveBookAuthorRelations(Book book)
        {

            book.Authors.Clear();

            _appDbContext.SaveChanges();
        }
    }
}
