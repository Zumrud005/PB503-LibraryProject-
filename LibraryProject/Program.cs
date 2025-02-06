using LibraryProject.DTOs.AuthorDto;
using LibraryProject.DTOs.BookDto;
using LibraryProject.DTOs.BorrowerDto;
using LibraryProject.Services.Implementation;
using LibraryProject.Services.Interface;
using System.Reflection.Metadata.Ecma335;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IAuthorService authorServices = new AuthorService();
            IBookService bookServices = new BookService();
            IBorrowerService borrowerService = new BorrowerService();
            
            string action;
            string input;
            do
            {
            Return:
                Console.WriteLine("");
                Console.WriteLine("1-Author actions");
                Console.WriteLine("2-Book actions");
                Console.WriteLine("3-Borrower actions");
                Console.WriteLine("4-BorrowBook");
                Console.WriteLine("5-ReturnBook");
                Console.WriteLine("6-The most borrowed book");
                Console.WriteLine("7-A list of overdue borrowers");
                Console.WriteLine("8-Which borrower has borrowed which books so far");
                Console.WriteLine("9-FilterBooksByTitle");
                Console.WriteLine("10-FilterBooksByAuthor");
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
                                        if (string.IsNullOrEmpty(name) || !IsValidString(name)) throw new ArgumentException("Author name cannot be empty ");


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
                                        if (string.IsNullOrEmpty(name) || !IsValidString(name)) throw new ArgumentException("Author name can not be empty or null");

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

                                        Console.WriteLine("Author removed aucessfuly.");
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
                            Console.WriteLine("2 - Create an books");
                            Console.WriteLine("3 - Edit an book");
                            Console.WriteLine("4 - Delete an book");
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
                                                Console.WriteLine($"ID: {book.Id}, Title: {book.Title}, Year: {book.PublishedYear}");
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
                            Console.WriteLine("2 - Create an borrowers");
                            Console.WriteLine("3 - Edit an borrowers");
                            Console.WriteLine("4 - Delete an borrower");
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
                            switch(cnt)
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
                                    catch(ArgumentException ex)
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
                                        if(string.IsNullOrEmpty(name) || !IsValidString(name) ) throw new ArgumentException("Borrower name can not be empty or null ");

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
                                    try { 
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
                                    try {
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
    




























































