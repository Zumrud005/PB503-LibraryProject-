using LibraryProject.DTOs;
using LibraryProject.DTOs.BookDto;
using LibraryProject.Models;
using LibraryProject.Repositories.Implementation;
using LibraryProject.Repositories.Interface;
using LibraryProject.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LibraryProject.Services.Implementation
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        public BookService()
        {
            _authorRepository = new AuthorRepository();
            _bookRepository = new BookRepository();
        }
        public void Add(BookCreateDto bookCreateDto)
        {
            if (bookCreateDto is null
           || string.IsNullOrWhiteSpace(bookCreateDto.Title)
           || string.IsNullOrWhiteSpace(bookCreateDto.Desc)) throw new ArgumentException("Book titles or book decs can not be null or empty");

            var book = new Book()
            {
                Title = bookCreateDto.Title,
                Description = bookCreateDto.Desc,
                PublishedYear = bookCreateDto.PublishedYear,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                UpdateAt = DateTime.UtcNow.AddHours(4),
                Authors = new List<Author>()
            };

            BookRepository bookRepository = new BookRepository();
            var authors = bookRepository._appDbContext.Authors
                .Where(a => bookCreateDto.AuthorIds.Contains(a.Id))
                .ToList();
            if (authors is null || authors.Count <  bookCreateDto.AuthorIds.Count) throw new ArgumentException("Author(s) not found");

            foreach (var author in authors)
            {
                book.Authors.Add(author);
            }
            bookRepository.Add(book);

            bookRepository.Commit();

           
        }

        public void Delete(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book is null)  throw new KeyNotFoundException("Book not found.");

            _bookRepository.Remove(book);
            _bookRepository.Commit();
        }

        public List<BookGetDto> GetAllBooks()
        {
            var books = _bookRepository.GetAllWithAuthors();
            if (books is null || !books.Any()) throw new InvalidOperationException("No books found.");

            return books.Select(x => new BookGetDto
            {
                Id = x.Id,
                Title = x.Title,
                Desc = x.Description,
                PublishedYear = x.PublishedYear,
                IsAvailable = _bookRepository.IsAvailable(x.Id),
                Authors = x.Authors.Select(x => new AuthorDtos { Id = x.Id, Name = x.Name }).ToList()
            }).ToList();
        }

        public BookGetDto GetBookById(int id)
        {
            var book = _bookRepository.GetByIdWithAuthors(id);
            if (book is null)  throw new KeyNotFoundException("Book not found.");

            return new BookGetDto
            {
                Id = book.Id,
                Title = book.Title,
                Desc = book.Description,
                PublishedYear = book.PublishedYear,
                IsAvailable = _bookRepository.IsAvailable(book.Id),
                Authors = book.Authors.Select(x => new AuthorDtos { Id = x.Id, Name = x.Name }).ToList()
            };
        }

        public void Update(int id, BookUpdateDto bookUpdateDto)
        {
            BookRepository bookRepository = new BookRepository();
            if (bookUpdateDto is null) throw new ArgumentNullException(nameof(bookUpdateDto));

           
            var book = bookRepository.GetByIdWithAuthors(id);
            if (book is null) throw new KeyNotFoundException("Book not found.");

            if (string.IsNullOrWhiteSpace(bookUpdateDto.Title) || string.IsNullOrWhiteSpace(bookUpdateDto.Desc)) throw new ArgumentException("Book title or book descreption cannot be empty.");

           
            bookRepository.RemoveBookAuthorRelations(book);

            book.Title = bookUpdateDto.Title;
            book.Description = bookUpdateDto.Desc;
            book.PublishedYear = bookUpdateDto.PublishedYear;
         
            
            book.UpdateAt = DateTime.UtcNow.AddHours(4);

            var authors = bookRepository._appDbContext.Authors.Where(a => bookUpdateDto.AuthorIds
                                                              .Contains(a.Id))
                                                              .ToList();
            if (authors is null || authors.Count < bookUpdateDto.AuthorIds.Count) throw new KeyNotFoundException("Author(s) not found");

            foreach (var author in authors)
            {
                book.Authors.Add(author);
            }

            bookRepository.Commit();
        }

        public BookGetDto GetMostBorrowedBook()
        {
            LoanItemRepository loanItemRepository = new LoanItemRepository();
            BookRepository bookRepository = new BookRepository();

            var loanItems = loanItemRepository.GetAll();

            if (loanItems == null || !loanItems.Any())
                throw new InvalidOperationException("No books have been borrowed!");

            
            var mostBorrowedBookId = loanItems
                .GroupBy(li => li.BookId)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .FirstOrDefault();

            var mostBorrowedBook = bookRepository.GetById(mostBorrowedBookId);

            if (mostBorrowedBook is null)
                throw new KeyNotFoundException("Book not found");

            return new BookGetDto
            {
                Id = mostBorrowedBook.Id,
                Title = mostBorrowedBook.Title,
                Desc = mostBorrowedBook.Description,
                PublishedYear = mostBorrowedBook.PublishedYear,
                IsAvailable = _bookRepository.IsAvailable(mostBorrowedBook.Id),

            };
        }

    }
}
