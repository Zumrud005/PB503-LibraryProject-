using LibraryProject.DTOs;
using LibraryProject.DTOs.AuthorDto;
using LibraryProject.DTOs.BookDto;
using LibraryProject.DTOs.BorrowerDto;
using LibraryProject.DTOs.LoanDto;
using LibraryProject.Models;
using LibraryProject.Repositories.Implementation;
using LibraryProject.Repositories.Interface;
using LibraryProject.Services.Implementation;
using LibraryProject.Services.Interface;

namespace LibraryProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IAuthorService authorServices = new AuthorService();
            IBookService bookServices = new BookService();
            IBorrowerService borrowerService = new BorrowerService();
            ILoanService loanService = new LoanService();
            ILoanItemService loanItemService = new LoanItemService();

            string action;
            string input;
            do
            {
            Return:
                Console.WriteLine("");
                Console.WriteLine("1-Author actions");
                Console.WriteLine("2-Book actions");
                Console.WriteLine("3-Borrower actions");
                Console.WriteLine("4-Loan actions");
                Console.WriteLine("5-BorrowBook");
                Console.WriteLine("6-ReturnBook");
                Console.WriteLine("7-The most borrowed book");
                Console.WriteLine("8-A list of overdue borrowers");
                Console.WriteLine("9-Which borrower has borrowed which books so far");
                Console.WriteLine("10-FilterBooksByTitle");
                Console.WriteLine("11-FilterBooksByAuthor");
                Console.WriteLine("0-Exit");
                try
                {
                    Console.Write("Enter a number for input: ");
                    input = Console.ReadLine();
                    if (!int.TryParse(input, out int ipt) || ipt < 0)
                    {
                        throw new ArgumentException("Input must be a positive number.");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Eror:{ex.Message}");
                    goto Return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Eror:{ex.Message}");
                    goto Return;
                }

                switch (input)
                {
                    case "1":


                        do
                        {
                        ReturnHere:

                            Console.WriteLine("\nSelect an action for authors:");
                            Console.WriteLine("1 - List all authors");
                            Console.WriteLine("2 - Create an author");
                            Console.WriteLine("3 - Edit an author");
                            Console.WriteLine("4 - Delete an author");
                            Console.WriteLine("0 - Back to main menu");
                            try
                            {
                                Console.Write("Enter a number for action: ");
                                action = Console.ReadLine();
                                if (!int.TryParse(action, out int act) || act < 0)
                                {
                                    throw new ArgumentException("Action must be a positive number.");
                                }
                            }

                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Eror:{ex.Message}");
                                goto ReturnHere;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Eror:{ex.Message}");
                                goto ReturnHere;
                            }

                            switch (action)
                            {
                                case "1":
                                    Console.WriteLine("Authors list");
                                    try
                                    {
                                        var authors = authorServices.GetAllAuthors();
                                        if (authors.Count == 0) throw new KeyNotFoundException("Authors not found");

                                        else
                                        {
                                            foreach (var author in authors)
                                            {
                                                Console.WriteLine($"ID: {author.Id}, Name: {author.Name}");
                                                foreach (var book in author.Books)
                                                {
                                                    Console.WriteLine($"    Book: {book.Title} (ID: {book.Id})");
                                                }
                                            }
                                        }
                                    }
                                    catch (KeyNotFoundException ex)
                                    {
                                        Console.WriteLine($"Eror: {ex.Message}");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Eror: {ex.Message}");
                                    }
                                    break;
                                case "2":
                                ReturnAdd:
                                    try
                                    {
                                        Console.Write("Please enter author name: ");
                                        var name = Console.ReadLine();
                                        if (string.IsNullOrEmpty(name) || !IsValidString(name)) throw new ArgumentException("Author name cannot be empty or number");


                                        var authorDto = new AuthorCreateDto { Name = name };



                                        authorServices.Add(authorDto);

                                        Console.WriteLine("Author added succesfuly");
                                    }

                                    catch (ArgumentException ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                        goto ReturnAdd;
                                    }
                                    catch (KeyNotFoundException ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                        goto ReturnAdd;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                        Console.WriteLine($"Inner Error: {ex.InnerException?.Message}");
                                        goto ReturnAdd;
                                    }

                                    break;
                                case "3":
                                ReturnUpdate:
                                    try
                                    {
                                        Console.Write("Enter author id witch you want update: ");
                                        int id = Convert.ToInt32(Console.ReadLine());
                                        if (id < 1) throw new ArgumentException("Author ID must be a positive number.");


                                        Console.Write("Enter new author name: ");
                                        var name = Console.ReadLine();
                                        if (string.IsNullOrEmpty(name) || !IsValidString(name)) throw new ArgumentException("Author name can not be empty , null or number");

                                        foreach (var item in bookServices.GetAllBooks())
                                        {
                                            Console.WriteLine($"{item.Id} - {item.Title}");
                                        }
                                        Console.Write("Enter new author's books: ");
                                        var bookIds = Console.ReadLine().Split(',').Select(int.Parse).ToList();
                                        if (bookIds.Count == 0) throw new ArgumentException("List is empty");

                                        var authorDto = new AuthorUpdateDto { Name = name, BookIds = bookIds };
                                        if (authorDto is null) throw new KeyNotFoundException("Author not found!");
                                        authorServices.Update(id, authorDto);

                                        Console.WriteLine("Author updated succesfuly.");
                                    }

                                    catch (ArgumentException ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                        goto ReturnUpdate;
                                    }
                                    catch (KeyNotFoundException ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                        goto ReturnUpdate;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                        goto ReturnUpdate;
                                    }

                                    break;
                                case "4":
                                ReturnRemove:
                                    try
                                    {
                                        Console.Write("Enter author id whitch author you want remove: ");
                                        var id = int.Parse(Console.ReadLine());
                                        if (id < 1) throw new ArgumentException("Author ID must be a positive number.");


                                        authorServices.Delete(id);

                                        Console.WriteLine("Author removed sucessfuly.");
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        Console.WriteLine($"Eror: {ex.Message}");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Eror: {ex.Message}");
                                    }

                                    break;
                                case "0":
                                    Console.WriteLine("Back to main menu...");
                                    goto Return;

                            }

                        }
                        while (action != "0");
                        break;
                    case "2":
                        string count;
                        do
                        {
                        ReturnAs:

                            Console.WriteLine("\nSelect an action for books:");
                            Console.WriteLine("1 - List all books");
                            Console.WriteLine("2 - Create  books");
                            Console.WriteLine("3 - Edit a book");
                            Console.WriteLine("4 - Delete a book");
                            Console.WriteLine("0 - Back to main menu");
                            try
                            {
                                Console.Write("Enter a number for action: ");
                                count = Console.ReadLine();
                                if (!int.TryParse(count, out int ct) || ct < 0)
                                {
                                    throw new ArgumentException("Count must be a positive number.");
                                }
                            }

                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Eror:{ex.Message}");
                                goto ReturnAs;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Eror:{ex.Message}");
                                goto ReturnAs;
                            }

                            switch (count)
                            {
                                case "1":
                                    try
                                    {
                                        var books = bookServices.GetAllBooks();
                                        if (books.Count == 0) throw new ArgumentException("Books not found.");

                                        else
                                        {
                                            foreach (var book in books)
                                            {
                                                Console.WriteLine($"ID: {book.Id}, Title: {book.Title},Descreption: {book.Desc}, Year: {book.PublishedYear}");
                                                foreach (var author in book.Authors)
                                                {
                                                    Console.WriteLine($"    Author: {author.Name} (ID: {author.Id})");
                                                }
                                            }
                                        }
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        Console.WriteLine($"Eror: {ex.Message}");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Eror: {ex.Message}");
                                    }
                                    break;
                                case "2":
                                ReturnBookAdd:
                                    try
                                    {
                                        Console.Write("Enter new book title: ");
                                        var title = Console.ReadLine();
                                        if (string.IsNullOrEmpty(title) || !IsValidString(title)) throw new ArgumentException("Book title can not be empty or null");

                                        Console.Write("Enter new book descreption: ");
                                        var description = Console.ReadLine();
                                        if (string.IsNullOrEmpty(description) || !IsValidString(description)) throw new ArgumentException("Book descreption can not be empty or null");

                                        Console.Write("Enter new book published year: ");
                                        var publishedYear = int.Parse(Console.ReadLine());
                                        if (publishedYear < 500 && publishedYear > 2025) throw new ArgumentException("Published year must be greater than 1000 and less than 2025 ");
                                        foreach (var item in authorServices.GetAllAuthors())
                                        {
                                            Console.WriteLine($"{item.Id} - {item.Name}");
                                        }
                                        Console.Write("Enter book's authors id: ");
                                        var authorIds = Console.ReadLine().Split(',').Select(int.Parse).ToList();
                                        if (authorIds.Count == 0) throw new ArgumentException("List is empty");

                                        var bookDto = new BookCreateDto { Title = title, Desc = description, PublishedYear = publishedYear, AuthorIds = authorIds };
                                        if (bookDto is null) throw new KeyNotFoundException("Book not found");
                                        bookServices.Add(bookDto);

                                        Console.WriteLine("Book added succesfuly.");
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        Console.WriteLine($"Eror: {ex.Message}");
                                        goto ReturnBookAdd;
                                    }
                                    catch (KeyNotFoundException ex)
                                    {
                                        Console.WriteLine($"Eror: {ex.Message}");
                                        goto ReturnBookAdd;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Eror: {ex.Message}");
                                        goto ReturnBookAdd;
                                    }

                                    break;
                                case "3":
                                ReturnBookUpdate:
                                    try
                                    {
                                        Console.Write("Enter the ID of the book you want to change: ");
                                        var id = int.Parse(Console.ReadLine());
                                        if (id < 0) throw new ArgumentException("Id must be a positive number");

                                        Console.Write("Enter new book title: ");
                                        var title = Console.ReadLine();
                                        if (string.IsNullOrEmpty(title) || !IsValidString(title)) throw new ArgumentException("Book title can not be empty or null");

                                        Console.Write("Enter new book descreption: ");
                                        var description = Console.ReadLine();
                                        if (string.IsNullOrEmpty(description) || !IsValidString(description)) throw new ArgumentException("Book descreption can not be empty or null");

                                        Console.Write("Enter new book published year: ");
                                        var publishedYear = int.Parse(Console.ReadLine());
                                        if (publishedYear < 1000) throw new ArgumentException("Published year must be greater than 1000 ");

                                        foreach (var item in authorServices.GetAllAuthors())
                                        {
                                            Console.WriteLine($"{item.Id} - {item.Name}");
                                        }
                                        Console.Write("Enter new Author IDs: ");
                                        var authorIds = Console.ReadLine().Split(',').Select(int.Parse).ToList();
                                        if (authorIds.Count == 0) throw new ArgumentException("List is empty");

                                        var bookDto = new BookUpdateDto { Title = title, Desc = description, PublishedYear = publishedYear, AuthorIds = authorIds };
                                        if (bookDto is null) throw new KeyNotFoundException("Book not found");
                                        bookServices.Update(id, bookDto);

                                        Console.WriteLine("Book updated succesfuly.");
                                    }


                                    catch (ArgumentException ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                        goto ReturnBookUpdate;
                                    }
                                    catch (KeyNotFoundException ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                        goto ReturnBookUpdate;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"An error occurred: {ex.Message}");
                                        goto ReturnBookUpdate;
                                    }
                                    break;



                                case "4":
                                ReturnBookRemove:
                                    try
                                    {
                                        Console.Write("Enter the ID of the book you want to delete: ");
                                        var id = int.Parse(Console.ReadLine());
                                        if (id < 0) throw new ArgumentException("Id must be a positive number");

                                        bookServices.Delete(id);

                                        Console.WriteLine("Book removed succesfuly");
                                    }

                                    catch (ArgumentException ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                        goto ReturnBookRemove;

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                        goto ReturnBookRemove;
                                    }

                                    break;
                                case "0":
                                    Console.WriteLine("Back to main menu...");
                                    goto Return;
                                    break;
                            }

                        }
                        while (count != "0");
                        break;


                    case "3":
                        string cnt;
                        do
                        {
                        ReturnAs:

                            Console.WriteLine("\nSelect an action for borrower:");
                            Console.WriteLine("1 - List all borrowers");
                            Console.WriteLine("2 - Create  borrowers");
                            Console.WriteLine("3 - Edit borrowers");
                            Console.WriteLine("4 - Delete a borrower");
                            Console.WriteLine("0 - Back to main menu");
                            try
                            {
                                Console.Write("Enter a number for action: ");
                                cnt = Console.ReadLine();
                                if (!int.TryParse(cnt, out int ct) || ct < 0)
                                {
                                    throw new ArgumentException("Count must be a positive number.");
                                }
                            }

                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Eror:{ex.Message}");
                                goto ReturnAs;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Eror:{ex.Message}");
                                goto ReturnAs;
                            }
                            switch (cnt)
                            {
                                case "1":
                                    try
                                    {
                                        var borrowers = borrowerService.GetAll();
                                        if (!borrowers.Any()) throw new ArgumentException("No borrower was found in the system.");


                                        Console.WriteLine("\n--- Borrowers list ---");
                                        foreach (var borrower in borrowers)
                                        {
                                            Console.WriteLine($"ID: {borrower.Id}, Name: {borrower.Name}, Email: {borrower.Email}");
                                        }
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                    }

                                    break;
                                case "2":
                                ReturnAddBorrower:
                                    try
                                    {
                                        Console.Write("Borrower name: ");
                                        string name = Console.ReadLine();
                                        if (string.IsNullOrEmpty(name) || !IsValidString(name)) throw new ArgumentException("Borrower name can not be empty or null ");

                                        Console.Write("Borrower email: ");
                                        string email = Console.ReadLine();
                                        if (string.IsNullOrEmpty(email) || !IsValidString(email)) throw new ArgumentException("Borrower email can not be empty or null ");



                                        var dto = new BorrowerCreateDto { Name = name, Email = email };
                                        borrowerService.Create(dto);
                                        Console.WriteLine("Borrower added succesfuly.");
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                        goto ReturnAddBorrower;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                        goto ReturnAddBorrower;
                                    }

                                    break;
                                case "3":
                                ReturnUpdateBorrower:
                                    try
                                    {
                                        Console.Write("Enter Borrower id whitch borrower you want update: ");
                                        if (!int.TryParse(Console.ReadLine(), out int id)) throw new ArgumentException("ID must be a positive number.");



                                        var borrower = borrowerService.GetById(id);
                                        if (borrower is null) throw new KeyNotFoundException("Borrower not found");


                                        Console.Write("New Borrower name:");
                                        string name = Console.ReadLine();
                                        if (string.IsNullOrEmpty(name) || !IsValidString(name)) throw new ArgumentException("Borrower name can not be empty or null ");

                                        Console.Write("New Borrower email: ");
                                        string email = Console.ReadLine();
                                        if (string.IsNullOrEmpty(email) || !IsValidString(email)) throw new ArgumentException("Borrower email can not be empty or null ");



                                        var dto = new BorrowerUpdateDto { Name = name, Email = email };
                                        borrowerService.Update(id, dto);
                                        Console.WriteLine($"Borrower ID {id} updated succesfuly.");
                                    }
                                    catch (KeyNotFoundException ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                        goto ReturnUpdateBorrower;

                                    }
                                    catch (ArgumentException ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                        goto ReturnUpdateBorrower;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Eror: {ex.Message}");
                                        goto ReturnUpdateBorrower;
                                    }

                                    break;
                                case "4":
                                ReturnRemoveBorrower:
                                    try
                                    {
                                        Console.Write("Enter id which borrower you want delete: ");
                                        if (!int.TryParse(Console.ReadLine(), out int id)) throw new ArgumentException("ID must be a positive number");



                                        borrowerService.Delete(id);
                                        Console.WriteLine($"Borrower ID {id} deleted succesfuly.");
                                    }
                                    catch (InvalidOperationException ex)
                                    {
                                        Console.WriteLine($"Eror: {ex.Message}");
                                        goto ReturnRemoveBorrower;
                                    }
                                    catch (KeyNotFoundException ex)
                                    {
                                        Console.WriteLine($"Eror: {ex.Message}");
                                        goto ReturnRemoveBorrower;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Eror: {ex.Message}");
                                        goto ReturnRemoveBorrower;
                                    }
                                    break;
                                case "0":
                                    Console.WriteLine("Back to main menu...");
                                    goto Return;
                            }
                        }
                        while (cnt != "0");
                        break;

                    case "4":

                        string enter;
                        do
                        {
                        ReturnAs:

                            Console.WriteLine("\nSelect an action for loan:");
                            Console.WriteLine("1 - List all Loans");
                            Console.WriteLine("2 - Create a Loan");
                            Console.WriteLine("3 - Edit a Loan");

                            Console.WriteLine("0 - Back to main menu");
                            try
                            {
                                Console.Write("Enter a number for action: ");
                                enter = Console.ReadLine();
                                if (!int.TryParse(enter, out int ent) || ent < 0)
                                {
                                    throw new ArgumentException("Count must be a positive number.");
                                }
                            }

                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Eror:{ex.Message}");
                                goto ReturnAs;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Eror:{ex.Message}");
                                goto ReturnAs;
                            }
                            switch (enter)
                            {
                                case "1":
                                ReturnCreateLoan:
                                    try
                                    {
                                        var loans = loanService.GetAll();
                                        if (loans.Count == 0) throw new KeyNotFoundException("No loans found.");



                                        foreach (var loan in loans)
                                        {
                                            Console.WriteLine($"Loan ID: {loan.Id}, Borrower ID: {loan.BorrowerId}, Loan Date: {loan.LoanDate}, Must Return Date: {loan.MustReturnDate}, Return Date: {loan.ReturnDate?.ToString() ?? "Not Returned"}");
                                        }
                                    }
                                    catch (KeyNotFoundException ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");

                                    }
                                    break;
                                case "2":
                                ReturnLoanAdd:
                                    try
                                    {
                                        Console.WriteLine("Borrower List");
                                        foreach (var item in borrowerService.GetAll())
                                        {
                                            Console.WriteLine($"{item.Id} - {item.Name}");
                                        }

                                        Console.Write("Enter Borrower Id: ");
                                        int borrowerId = int.Parse(Console.ReadLine());
                                        if (borrowerId < 1) throw new ArgumentException("must be a positive number and less than 0");
                                        Console.WriteLine("Book List");
                                        foreach (var item in bookServices.GetAllBooks())
                                        {
                                            Console.WriteLine($"{item.Id} - {item.Title}");
                                        }
                                        Console.Write("Enter Book Ids: ");
                                        List<int> bookIds = Console.ReadLine().Split(',').Select(int.Parse).ToList();


                                        var loan = loanService.Create(new LoanCreateDto { BorrowerId = borrowerId, BookIds = bookIds });
                                        Console.WriteLine($"Loan created successfully! Loan Id: {loan.Id}");
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        Console.WriteLine($"Error: {ex.Message}");
                                        goto ReturnLoanAdd;
                                    }


                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Eror:{ex.Message}");
                                        goto ReturnLoanAdd;

                                    }

                                    break;
                                case "3":
                                ReturnUpdateLoan:
                                    try
                                    {
                                        Console.Write("Enter Loan ID to update: ");
                                        int loanId = int.Parse(Console.ReadLine());
                                        if (loanId < 1) throw new ArgumentException("must be a positive number and less than 0");

                                        Console.Write("Enter Return Date (YYYY-MM-DD): ");

                                        DateTime? returnDate = null;

                                        while (returnDate is null)
                                        {
                                            Console.Write("Enter Return Date (YYYY-MM-DD): ");
                                            string returnDateInput = Console.ReadLine();

                                            if (string.IsNullOrEmpty(returnDateInput))
                                            {
                                                Console.WriteLine("Return Date cannot be empty. Please enter a valid date.");
                                                continue;
                                            }

                                            if (DateTime.TryParse(returnDateInput, out DateTime parsedDate))
                                            {
                                                returnDate = parsedDate;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid date format. Please enter a valid date (YYYY-MM-DD).");
                                            }

                                            loanService.Update(loanId, new LoanUpdateDto { ReturnDate = returnDate });
                                            Console.WriteLine("Loan updated successfully!");
                                        }
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        Console.WriteLine($"Error: {ex.Message}");
                                        goto ReturnUpdateLoan;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error: {ex.Message}");
                                        goto ReturnUpdateLoan;
                                    }
                                    break;

                                case "0":
                                    Console.WriteLine("Back to main menu...");
                                    goto Return;

                            }
                        }
                        while (enter != "0");
                        break;
                    case "5":
                    ReturnBorrowBook:


                        //try
                        //{

                        //    List<int> selectedBookIds = new List<int>();
                        //    Console.WriteLine("Borrower list");
                        //    var borrowers = borrowerService.GetAll();
                        //    if (borrowers is null) throw new ArgumentException("List is empty");
                        //    foreach (var borrower in borrowers)
                        //    {
                        //        Console.WriteLine($"{borrower.Id} - {borrower.Name}");
                        //    }
                        //    Console.WriteLine("Please enter borrower id:");
                        //    int borrowerId;
                        //    while (!int.TryParse(Console.ReadLine(), out borrowerId) || !borrowers.Any(b => b.Id == borrowerId))
                        //    {
                        //        Console.WriteLine("Wrong borrower id. Please try again...");
                        //    }

                        //    while (true)
                        //    {
                        //        Console.WriteLine("Books list:");
                        //        var books = bookServices.GetAllBooks();
                        //        var borrowedBookIds = loanItemService.GetAll().Select(x => x.BookId).ToHashSet();
                        //        foreach (var book in books)
                        //        {
                        //            string isAviable = loanService.IsBookAvailable(book.Id) ? " Aviable" : "Not Aviable";
                        //            Console.WriteLine($"Id: {book.Id} - Book title: {book.Title}  -  {isAviable}");
                        //        }

                        //        Console.WriteLine("Please choose book id(0-exit):");
                        //        int bookId;
                        //        while (!int.TryParse(Console.ReadLine(), out bookId) || bookId != 0 && !books.Any(b => b.Id == bookId))
                        //        {
                        //            Console.WriteLine("Wrong book id. Please try again...");
                        //        }

                        //        if (bookId == 0)
                        //        {
                        //            break;
                        //        }
                        //        if (borrowedBookIds.Contains(bookId) && loanItemService.GetByIdLoanItem(bookId).ReturnDate is null)
                        //        {
                        //            Console.WriteLine("This book borrowed. Please choose other book");
                        //            continue;
                        //        }
                        //        if (!loanService.IsBookAvailable(bookId))
                        //        {
                        //            Console.WriteLine("This book is currently not available.");
                        //            continue; 
                        //        }
                        //        selectedBookIds.Add(bookId);
                        //        Console.WriteLine($"{books.First(x => x.Id == bookId).Title} choosed.");

                        //        Console.WriteLine("Next:");
                        //        Console.WriteLine("1-Borrow new book");
                        //        Console.WriteLine("2-Start borrowing process");

                        //        Console.WriteLine("Choose:");
                        //        string choose = Console.ReadLine();
                        //        if (choose == "2") break;


                        //    }

                        //    if (!selectedBookIds.Any())
                        //    {
                        //        throw new ArgumentException("No book was selected");
                        //    }

                        //    Loan newLoan = new Loan
                        //    {
                        //        BorrowerId = borrowerId,
                        //        LoanDate = DateTime.UtcNow.AddHours(4),
                        //        MustReturnDate = DateTime.UtcNow.AddHours(4).AddDays(15),
                        //        CreatedAt = DateTime.UtcNow.AddHours(4),
                        //        UpdateAt = DateTime.UtcNow.AddHours(4),
                        //        LoanItems = selectedBookIds.Select(bookId => new LoanItem { BookId = bookId }).ToList()
                        //    };

                        //    loanService.CreateLoan(newLoan);

                        //    Console.WriteLine("Book borrowed succesfuly");
                        //}



                        //catch (ArgumentException ex)
                        //{
                        //    Console.WriteLine($"Error: {ex.Message}");
                        //    goto ReturnBorrowBook;
                        //}
                        //catch (Exception ex)
                        //{
                        //    Console.WriteLine($"Error: {ex.Message}");
                        //    goto ReturnBorrowBook;
                        //}
                        break;
                    case "6":
                        ReturnBookBorrow:
                        try
                        {
                            Console.WriteLine("Borrowers list:");
                            var borrowers = borrowerService.GetAll();
                            foreach (var borrower in borrowers)
                            {
                                Console.WriteLine($"ID: {borrower.Id} - {borrower.Name}");
                            }

                            Console.Write("Choose borrower id : ");
                            int borrowerId;
                            while (!int.TryParse(Console.ReadLine(), out borrowerId) || !borrowers.Any(b => b.Id == borrowerId))
                            {
                                Console.WriteLine("Wrong borrower id. Please try again...");
                            }


                            loanService.ReturnBook(borrowerId);
                            Console.WriteLine("Return time updated");


                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine($"Eror:{ex.Message}");
                            goto ReturnBookBorrow;
                        }
                        break;
                    case "7":
                        Console.WriteLine("Most Borrowed book");
                        var wantedBook = bookServices.GetMostBorrowedBook();
                        Console.WriteLine($"{wantedBook.Id} - {wantedBook.Title}");
                        break;
                    case "8":
                       
                        try
                        {
                           
                            List<Borrower> delayedBorrowers = loanService.GetDelayedBorrowers();

                            if (!delayedBorrowers.Any())
                            {
                                Console.WriteLine("Currently, there are no delayed borrowers.");
                            }
                            else
                            {
                                Console.WriteLine("List of delayed borrowers:");
                                foreach (var borrower in delayedBorrowers)
                                {
                                    Console.WriteLine($"ID: {borrower.Id} - Name: {borrower.Name}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                        break;
                    case "9":
                        Console.WriteLine("Full borrowers and borrowed books list");
                        var list = borrowerService.GetAllBorrowedBooks();
                        foreach (var item in list)
                        {
                            Console.WriteLine("---Borrower---");
                            Console.WriteLine($"{item.BorrowerId} - {item.BorrowerName}");
                            Console.WriteLine("Books:");
                            foreach (var book in item.BorrowedBooks)
                            {
                                Console.WriteLine($"{book.Title}");
                            }
                        }
                        break;
                    case "10":
                        ReturnFilterBook:
                        try
                        {
                           
                            Console.Write("Search by title: ");
                            var inputs = Console.ReadLine() ?? throw new ArgumentException();
                            IBookService bookService = new BookService();
                            var datas = bookService.GetAllBooks()
                                                   .Where(b => b.Title
                                                   .Trim()
                                                   .ToLower()
                                                   .Contains(inputs
                                                   .Trim()
                                                   .ToLower()))
                                                   .ToList();
                            if (datas.Count == 0) throw new KeyNotFoundException("Book not found"); 
                          
                            foreach (var data in datas)
                            {
                                data.Desc = string.Empty;
                                Console.WriteLine($"{data.Id} - {data.Title}");
                            }
                           
                        }
                        catch(ArgumentException ex)
                        {
                            Console.WriteLine($"Eror:{ex.Message}");
                            goto ReturnFilterBook;
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine($"Eror:{ex.Message}");
                            goto ReturnFilterBook;

                        }
                        break;
                    case "11":
                        ReturnFilterAuthor:
                        try
                        {
                            Console.Clear();
                            Console.Write("Search by author: ");
                            var inputs = Console.ReadLine() ?? throw new ArgumentException("Inputs can not be empty or null");
                            
                            var authors = authorServices.GetAllAuthors()
                                                        .Where(a => a.Name
                                                        .Trim()
                                                        .ToLower()
                                                        .Contains(inputs
                                                        .Trim()
                                                        .ToLower()))
                                                        .ToList();
                            var books = new List<BookDtos>();
                            authors.ForEach(a => books.AddRange(a.Books));
                            books = books.Distinct().ToList();
                            if (books.Count == 0) throw new KeyNotFoundException("Book not found");
                            books.ForEach(b => Console.WriteLine(b));

                          
                        }
                        catch(KeyNotFoundException ex)
                        {
                            Console.WriteLine($"Eror:{ex.Message}");
                            goto ReturnFilterAuthor;
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine($"Eror:{ex.Message}");
                            goto ReturnFilterAuthor;
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine($"Eror:{ex.Message}");
                            goto ReturnFilterAuthor;

                        }
                        break;
                    case "0":
                        Console.WriteLine("Stoped programing...");
                        break;
                      

                }
            }
            while (input != "0");

        }
        static bool IsValidString(string value)
        {
            foreach (char c in value)
            {
                if (!char.IsLetter(c))
                {
                    return false;
                }

            }
            return true;
        }
       


    }
}





























































