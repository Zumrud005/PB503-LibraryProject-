﻿using LibraryProject.Data;
using LibraryProject.Models;
using LibraryProject.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Repositories.Implementation
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        
        public List<Author> GetAllWithBooks()
        => _appDbContext.Authors.Include(a => a.Books).ToList();


        public List<Author> GetByIds(List<int> ids)
            => _appDbContext.Authors.Include(n => n.Books).Where(x => ids.Contains(x.Id)).ToList();


        public Author GetByIdWithBooks(int id)
        => _appDbContext.Authors.Include(x => x.Books).FirstOrDefault(x => x.Id == id);


        public List<Book>? BooksSet(List<int> bookId)
        => _appDbContext.Books.Where(a => bookId
                              .Contains(a.Id))
                              .ToList();
        public void RemoveBookAuthorRelations(Author author)
        {
            
            author.Books.Clear();  

            _appDbContext.SaveChanges();
        }

    }
}
