using LibraryProject.Data;
using LibraryProject.DTOs;
using LibraryProject.DTOs.AuthorDto;
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
        public void Add(AuthorCreateDto authorDto)
        {
            if (authorDto is null) throw new ArgumentNullException(nameof(authorDto));

            if (string.IsNullOrWhiteSpace(authorDto.Name)) throw new ArgumentException("Author name cannot be empty.");

            

            var author = new Author
            {
                Name = authorDto.Name,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                UpdateAt = DateTime.UtcNow.AddHours(4)
            };

            _authorRepository.Add(author);
            _authorRepository.Commit();
        }

        public void Delete(int id)
        {
            var author = _authorRepository.GetById(id);
            if (author is null) throw new KeyNotFoundException("Author not found.");

            
            _authorRepository.RemoveBookAuthorRelations(author);

           
            _authorRepository.Remove(author);
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

        public void Update(int id, AuthorUpdateDto authorDto)
        {
            if (authorDto is null) throw new ArgumentNullException(nameof(authorDto));

            var author = _authorRepository.GetById(id);
            if (author is null) throw new KeyNotFoundException("Author not found.");

            if (string.IsNullOrWhiteSpace(authorDto.Name)) throw new ArgumentException("Author name cannot be empty.");

            
            var books = _bookRepository.GetByIds(authorDto.BookIds);
            if (books is null || !books.Any()) throw new InvalidOperationException("At least one valid book must be selected.");

           
            author.Name = authorDto.Name;
            author.UpdateAt = DateTime.UtcNow.AddHours(4);



            _authorRepository.RemoveBookAuthorRelations(author);

            
            author.Books = books;

            _authorRepository.Commit();
        }
    }
}
