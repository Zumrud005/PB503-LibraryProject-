using LibraryProject.Data;
using LibraryProject.DTOs;
using LibraryProject.DTOs.AuthorDto;
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
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        public AuthorService()
        {
            _bookRepository = new BookRepository(); 
            _authorRepository = new AuthorRepository();
        }
        public void Add(AuthorCreateDto authorCreateDto)
        {
            if (authorCreateDto is null) throw new ArgumentNullException(nameof(authorCreateDto));

            if (string.IsNullOrWhiteSpace(authorCreateDto.Name)) throw new ArgumentException("Author name cannot be empty.");

            

            var author = new Author
            {
                Name = authorCreateDto.Name,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                UpdateAt = DateTime.UtcNow.AddHours(4)
            };

            _authorRepository.Add(author);
            _authorRepository.Commit();
        }

        public void Delete(int id)
        {
            AuthorRepository authorRepository = new AuthorRepository();
            var author = authorRepository.GetById(id);
            if (author is null) throw new KeyNotFoundException("Author not found.");
            if(author.Books == null  || author.Books.Count == 0)
            {
                authorRepository.Remove(author);
                authorRepository.Commit();
            }

            else
            {
                 authorRepository.RemoveBookAuthorRelations(author);
                 authorRepository.Remove(author);
                 authorRepository.Commit();
 


            }
        
        }

        public List<AuthorGetDto> GetAllAuthors()
        {
            var authors = _authorRepository.GetAllWithBooks();
            if (authors is null || !authors.Any()) throw new InvalidOperationException("No authors found.");

            return authors.Select(x => new AuthorGetDto
            {
                Id = x.Id,
                Name = x.Name,
                Books = x.Books.Select(x => new BookDtos { Id = x.Id, Title = x.Title }).ToList()
            }).ToList();
        }

        public AuthorGetDto GetAuthorById(int id)
        {
            var author = _authorRepository.GetByIdWithBooks(id);
            if (author is null) throw new KeyNotFoundException("Author not found.");

            return new AuthorGetDto
            {
                Id = author.Id,
                Name = author.Name,
                Books = author.Books.Select(x => new BookDtos { Id = x.Id, Title = x.Title }).ToList()
            };
        }

        public void Update(int id, AuthorUpdateDto authorUpdateDto)
        {
            AuthorRepository authorRepository = new AuthorRepository();
            if (authorUpdateDto is null) throw new ArgumentNullException(nameof(authorUpdateDto));

            var author = authorRepository.GetByIdWithBooks(id);
            if (author is null) throw new KeyNotFoundException("Author not found.");

            if (string.IsNullOrWhiteSpace( authorUpdateDto.Name)) throw new ArgumentException("Author name cannot be empty.");


            
            var books = authorRepository._appDbContext.Books
                                                      .Where(a => authorUpdateDto.BookIds
                                                      .Contains(a.Id))
                                                      .ToList();

            authorRepository.RemoveBookAuthorRelations(author);


            author.Name = authorUpdateDto.Name;
            author.UpdateAt = DateTime.UtcNow.AddHours(4);
            if (books is null || books.Count < authorUpdateDto.BookIds.Count) throw new KeyNotFoundException("Books not found");

            foreach (var book in books)
            {
                author.Books.Add(book);

            }





            authorRepository.Commit();
        }
    }
}
