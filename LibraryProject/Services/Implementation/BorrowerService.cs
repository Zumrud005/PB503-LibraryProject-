using LibraryProject.DTOs;
using LibraryProject.DTOs.BookDto;
using LibraryProject.DTOs.BorrowerDto;
using LibraryProject.Models;
using LibraryProject.Repositories.Implementation;
using LibraryProject.Repositories.Interface;
using LibraryProject.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Services.Implementation
{
    public class BorrowerService : IBorrowerService
    {
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILoanItemRepository _loanItemRepository;

        public BorrowerService()
        {
            _borrowerRepository = new BorrowerRepository();
            _loanRepository = new LoanRepository();
            _bookRepository = new BookRepository();
            _loanItemRepository = new LoanItemRepository();
        }
        public void Create(BorrowerCreateDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email)) throw new ArgumentException("Author name cannot be empty.");



            var borrower = new Borrower
            {
                Name = dto.Name,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                UpdateAt = DateTime.UtcNow.AddHours(4)
            };

            _borrowerRepository.Add(borrower);
            _borrowerRepository.Commit();
        }

        public void Delete(int id)
        {
            BorrowerRepository repository = new BorrowerRepository();
            var borrower = repository.GetById(id);
            if (borrower is null)
                throw new KeyNotFoundException($"Borrower ID {id} not found!");
            LoanRepository loanRepository = new LoanRepository();
            var loans = loanRepository.GetAll().Where(l => l.BorrowerId == id).ToList();

         
            bool hasUnreturnedBooks = loans.Any(l => l.ReturnDate == null);
            if (hasUnreturnedBooks)
            {
                throw new InvalidOperationException("There are books that Borrower has not returned! Update is not possible.");
            }

            LoanItemRepository loanItemRepository = new LoanItemRepository();
            foreach (var loan in loans)
            {
                
                var loanItems = loanItemRepository.GetAll().Where(li => li.LoanId == loan.Id).ToList();
                foreach (var loanItem in loanItems)
                {
                    loanItemRepository.Remove(loanItem);
                }
                loanRepository.Remove(loan);
            }

      
            repository.Remove(borrower);
            repository.Commit();

            
        }

        public List<BorrowerGetDto> GetAll()
        {
            var borrowers = _borrowerRepository.GetAll();
            return borrowers.Select(b => new BorrowerGetDto
            {
                Id = b.Id,
                Name = b.Name,
                Email = b.Email
            }).ToList();
        }

        public BorrowerGetDto GetById(int id)
        {
            var borrower = _borrowerRepository.GetById(id);
            if (borrower is null)
            {
                throw new KeyNotFoundException($"Borrower with id {id} not found.");
            }

            var loans = _loanRepository.GetAll()
                                       .Where(l => l.BorrowerId == id)
                                       .ToList();
            var loanItems = _loanItemRepository.GetAll()
                                               .Where(li => loans
                                               .Select(l => l.Id)
                                               .Contains(li.LoanId))
                                               .ToList();
            var bookIds = loanItems.Select(li => li.BookId)
                                   .Distinct()
                                   .ToList();
            var books = _bookRepository.GetAll()
                                        .Where(b => bookIds
                                        .Contains(b.Id))
                                        .ToList();

            return new BorrowerGetDto
            {
                Id = borrower.Id,
                Name = borrower.Name,
                Email = borrower.Email,
                BorrowedBooks = books.Select(b => new BookGetDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Desc = b.Description,
                    PublishedYear = b.PublishedYear
                }).ToList()
            };
        }

        public void Update(int id, BorrowerUpdateDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email)) throw new ArgumentException("Borrower name or email cannot be empty.");

            BorrowerRepository repository = new BorrowerRepository();
            var borrower = repository.GetById(id);
            if (borrower is null)
            {
                throw new KeyNotFoundException($"Borrower with id {id} not found.");
            }


            LoanRepository loanRepository = new LoanRepository();
            var loans = loanRepository.GetAll()
                                     
                                      .Where(l => l.BorrowerId == id)
                                      .ToList();

            LoanItemRepository loanItemRepository = new LoanItemRepository();
            var loanItems = loanItemRepository.GetAll().Where(li => loans.Select(l => l.Id).Contains(li.LoanId)).ToList();

          
            bool hasUnreturnedBooks = loans.Any(l => l.ReturnDate == null);
            if (hasUnreturnedBooks)
            {
                throw new InvalidOperationException("There are books that Borrower has not returned! Update is not possible.");
            }

            borrower.Name = dto.Name;
            borrower.Email = dto.Email;
            borrower.UpdateAt = DateTime.UtcNow.AddHours(4);

          
            

           
            repository.Commit();

           
        }


        public List<BorrowerBooksDto> GetAllBorrowedBooks()
        {
            LoanRepository loanRepository = new LoanRepository();
            LoanItemRepository loanItemRepository = new LoanItemRepository();
            BookRepository bookRepository = new BookRepository();
            BorrowerRepository borrowerRepository = new BorrowerRepository();

          
            var allLoans = loanRepository.GetAll();

         
            if (!allLoans.Any()) return new List<BorrowerBooksDto>();

          
            var loanItems = loanItemRepository.GetAll()
                .Where(li => allLoans.Select(l => l.Id).Contains(li.LoanId))
                .ToList();

            
            var books = bookRepository.GetAll()
                .Where(b => loanItems.Select(li => li.BookId).Contains(b.Id))
                .ToList();

       
            var borrowers = borrowerRepository.GetAll()
                .Where(b => allLoans.Select(l => l.BorrowerId).Contains(b.Id))
                .ToList();

           
            var result = borrowers.Select(borrower => new BorrowerBooksDto
            {
                BorrowerId = borrower.Id,
                BorrowerName = borrower.Name,
                BorrowedBooks = loanItems
                    .Where(li => allLoans.Any(l => l.BorrowerId == borrower.Id && l.Id == li.LoanId))
                    .Select(li => new BookGetDto
                    {
                        Id = li.BookId,
                        Title = books.FirstOrDefault(b => b.Id == li.BookId)?.Title
                        
                    })
                    .ToList()
            }).ToList();

            return result;
        }


       

    }
}
