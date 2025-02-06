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
        public void Add(BookCreateDto bookDto)
        {
            if (bookDto == null
           || string.IsNullOrWhiteSpace(bookDto.Title)
           || string.IsNullOrWhiteSpace(bookDto.Desc)) throw new ArgumentException("Book titles or book decs can not be null or empty");

            var book = new Book()
            {
                Title = bookDto.Title,
                Description = bookDto.Desc,
                PublishedYear = bookDto.PublishedYear,
                Authors = new List<Author>()
            };

            BookRepository bookRepository = new BookRepository();
            var authors = bookRepository._appDbContext.Authors
                .Where(a => bookDto.AuthorIds.Contains(a.Id))
                .ToList();
            if (authors is null || authors.Count <  bookDto.AuthorIds.Count) throw new ArgumentException("Author(s) not found");

            foreach (var author in authors)
            {
                book.Authors.Add(author);
            }
            bookRepository.Add(book);

            bookRepository.Commit();

            //if (bookDto is null) throw new ArgumentNullException(nameof(bookDto));

            //if (string.IsNullOrWhiteSpace(bookDto.Title) || string.IsNullOrWhiteSpace(bookDto.Desc)) throw new ArgumentException("Book title or book descreption cannot be empty.");

            //var authors = _authorRepository.GetByIds(bookDto.AuthorIds).ToList();
            //var authors = _authorRepository.GetAll().Where(a => bookDto.AuthorIds.Contains(a.Id)).ToList();

            //if (authors is null || authors.Count != bookDto.AuthorIds.Count)
            //{
            //    throw new KeyNotFoundException("One or more authors not found.");
            //}
            //if (!authors.Any())
            //{
            //    throw new InvalidOperationException("No featured authors available.");
            //}


            //var book = new Book
            //{
            //    Title = bookDto.Title,
            //    Description = bookDto.Desc,
            //    PublishedYear = bookDto.PublishedYear,
            //    CreatedAt = DateTime.UtcNow.AddHours(4),
            //    UpdateAt = DateTime.UtcNow.AddHours(4),
            //    Authors = authors
            //};


            //_bookRepository.Add(book);
            //_bookRepository.Commit();
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
                Authors = book.Authors.Select(x => new AuthorDtos { Id = x.Id, Name = x.Name }).ToList()
            };
        }

        public void Update(int id, BookUpdateDto bookDto)
        {
            if (bookDto is null) throw new ArgumentNullException(nameof(bookDto));

            var book = _bookRepository.GetById(id);
            if (book is null) throw new KeyNotFoundException("Book not found.");

            if (string.IsNullOrWhiteSpace(bookDto.Title) || string.IsNullOrWhiteSpace(bookDto.Desc)) throw new ArgumentException("Book title or book descreption cannot be empty.");

            var authors = _authorRepository.GetByIds(bookDto.AuthorIds);
            if (authors is null || !authors.Any())  throw new InvalidOperationException("At least one valid author must be selected.");

            book.Title = bookDto.Title;
            book.Description = bookDto.Desc;
            book.PublishedYear = bookDto.PublishedYear;
            book.Authors = authors;
            book.UpdateAt = DateTime.UtcNow.AddHours(4);

            _bookRepository.Commit();
        }
    }
}
