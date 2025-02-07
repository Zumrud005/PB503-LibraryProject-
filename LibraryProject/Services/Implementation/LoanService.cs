using LibraryProject.DTOs.LoanDto;
using LibraryProject.DTOs.LoanItemDto;
using LibraryProject.Models;
using LibraryProject.Repositories.Implementation;
using LibraryProject.Repositories.Interface;
using LibraryProject.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Services.Implementation
{
    public class LoanService : ILoanService
    {

        public LoanGetDto Create(LoanCreateDto loanCreateDto)
        {
            BorrowerRepository borrowerRepository = new BorrowerRepository();
            LoanRepository loanRepository = new LoanRepository();
            IBookRepository bookRepository = new BookRepository();


            var borrower = borrowerRepository.GetById(loanCreateDto.BorrowerId);
            if (borrower is null)
                throw new KeyNotFoundException("Borrower not found");


            if (loanCreateDto.BookIds is null || !loanCreateDto.BookIds.Any())
                throw new ArgumentException("At least one book must be selected!");

            var books = bookRepository.GetAll().Where(b => loanCreateDto.BookIds.Contains(b.Id)).ToList();
            if (books.Count != loanCreateDto.BookIds.Count)
                throw new ArgumentException("Some books do not exist!");
            if (loanCreateDto.BookIds.Any(bookId => !bookRepository.IsAvailable(bookId)))
            {
                throw new InvalidOperationException("One or more books are currently borrowed and not returned yet.");
            }


            var loanItems = books.Select(book => new LoanItem { BookId = book.Id }).ToList();


            var loan = new Loan
            {
                BorrowerId = loanCreateDto.BorrowerId,
                LoanDate = DateTime.UtcNow.AddHours(4),
                MustReturnDate = DateTime.UtcNow.AddHours(4).AddDays(14),
                CreatedAt = DateTime.UtcNow.AddHours(4),
                UpdateAt = DateTime.UtcNow.AddHours(4),
                LoanItems = loanItems
            };


            loanRepository.Add(loan);
            loanRepository.Commit();


            return new LoanGetDto
            {
                Id = loan.Id,
                BorrowerId = loan.BorrowerId,
                LoanDate = loan.LoanDate,
                MustReturnDate = loan.MustReturnDate,
                LoanItems = loan.LoanItems.Select(li => new LoanItemGetDto
                {
                    Id = li.Id,
                    BookId = li.BookId
                }).ToList()
            };
        }


        public List<LoanGetDto> GetAll()
        {
            LoanRepository loanRepository = new LoanRepository();
            var loans = loanRepository.GetAll();
            return loans.Select(loan => new LoanGetDto
            {
                Id = loan.Id,
                BorrowerId = loan.BorrowerId,
                LoanDate = loan.LoanDate,
                MustReturnDate = loan.MustReturnDate,
                ReturnDate = loan.ReturnDate,
                LoanItems = loan.LoanItems.Select(li => new LoanItemGetDto { Id = li.Id, BookId = li.BookId }).ToList()
            }).ToList();
        }

        public LoanGetDto GetById(int id)
        {

            LoanRepository loanRepository = new LoanRepository();
            var loan = loanRepository.GetById(id);
            if (loan is null) throw new KeyNotFoundException("Loan not found");

            return new LoanGetDto
            {
                Id = loan.Id,
                BorrowerId = loan.BorrowerId,
                LoanDate = loan.LoanDate,
                MustReturnDate = loan.MustReturnDate,
                ReturnDate = loan.ReturnDate,
                LoanItems = loan.LoanItems.Select(li => new LoanItemGetDto { Id = li.Id, BookId = li.BookId }).ToList()
            };
        }

        public void Update(int id, LoanUpdateDto loanUpdateDto)
        {
            LoanRepository loanRepository = new LoanRepository();
            var loan = loanRepository.GetById(id);
            if (loan is null) throw new KeyNotFoundException("Loan not found");

            loan.ReturnDate = loanUpdateDto.ReturnDate;
            loanRepository.Commit();
        }
       

        public void CreateLoan(Loan loan)
        {
            LoanRepository loanRepository = new LoanRepository();
            if (loan is null) { throw new KeyNotFoundException(nameof(loan)); }
            loanRepository.Add(loan);
            loanRepository.Commit();
        }

        public void ReturnBook(int borrowerId)
        {

            LoanRepository loanRepository = new LoanRepository();
            LoanItemRepository loanItemRepository = new LoanItemRepository();
            BookRepository bookRepository = new BookRepository();


            var loans = loanRepository.GetAll()
                .Where(l => l.BorrowerId == borrowerId && l.ReturnDate is null)
                .ToList();

            if (!loans.Any())
                throw new KeyNotFoundException("Active loans not found for borrower");


            var loanItems = loanItemRepository.GetAll()
                .Where(li => loans.Select(l => l.Id).Contains(li.LoanId))
                .ToList();

            var books = bookRepository.GetAll()
                .Where(b => loanItems.Select(li => li.BookId).Contains(b.Id))
                .ToList();


            Console.WriteLine("Borrowed Books:");
            foreach (var book in books)
            {
                Console.WriteLine($"{book.Id} - {book.Title}");
            }


            Console.Write("Enter the book ID to return: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
            {
                throw new ArgumentException("Invalid book ID");
            }


            var loanItemToReturn = loanItems.FirstOrDefault(li => li.BookId == bookId);
            if (loanItemToReturn is null)
            {
                throw new KeyNotFoundException("Book not found in borrower's loans");
            }


            var loan = loans.First(l => l.Id == loanItemToReturn.LoanId);
            loan.ReturnDate = DateTime.UtcNow.AddHours(4);


            loanRepository.Commit();
            Console.WriteLine($"Book '{books.First(b => b.Id == bookId).Title}' returned successfully.");
        }



        public List<Borrower> GetDelayedBorrowers()
        {
            LoanRepository loanRepository = new LoanRepository();
            BorrowerRepository borrowerRepository = new BorrowerRepository();


            var delayedLoans = loanRepository.GetAll()
                                             .Where(l =>  l.MustReturnDate < l.ReturnDate )
                                             .ToList();


            var delayedBorrowerIds = delayedLoans.Select(l => l.BorrowerId).Distinct().ToList();
            var delayedBorrowers = borrowerRepository.GetAll()
                                                      .Where(b => delayedBorrowerIds.Contains(b.Id))
                                                      .ToList();
            return delayedBorrowers;
        }




    }


}

