using LibraryManagement;
using Microsoft.VisualBasic;

namespace LibraryManagement
{
    internal class Program
    {
        private static readonly Library library = new Library(); 
        // create a variable called library called Library to store library object.
        // py: library = Library()
        static void Main(string[] args)
        {
            SeedData(); // initial data added when app starts

            bool running = true;
            while (running)
            {
                ShowMenu();
                string? choice = Console.ReadLine();
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1": AddBook(); break;
                        case "2": AddMember(); break;
                        case "3": ListBooks(); break;
                        case "4": SearchBooks(); break;
                        case "5": BorrowBook(); break;
                        case "6": ReturnBook(); break;
                        case "7": ShowOverdue(); break;
                        case "8": SkipDays(); break;
                        case "9": ListMember(); break;
                        case "0": running = false; break;
                        default: Console.WriteLine("Invalid choice."); break;
                    }
                }
                catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            Console.WriteLine("Goodbye!");
        }

        static void SeedData()
        {
            library.AddBook("Clean Code", "Robert C. Martin");
            library.AddBook("The Pragmatic Programmer", "Andrew Hunt");
            library.AddBook("C# in Depth", "Jon Skeet");
            library.AddBook("Refactoring", "Martin Fowler");
            library.AddBook("Domain-Driven Design", "Eric Evans");
            library.AddMember("Asha");
            library.AddMember("Ravi");
        }

        static void AddBook()
        {
            Console.Write("Title: ");
            string title = Console.ReadLine() ?? ""; // if the value on left is null then replace it with 
            // "" empty string if not then value on the left. (null-coleacing it is) 
            // py: title = input() or ""
            Console.Write("Author:");
            string author = Console.ReadLine() ?? "";
            Book book = library.AddBook(title, author);
            Console.WriteLine($"Added: {book}");
        }

        static void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine($"=== LIBRARY (today: {library.Today:dd-MMM-yyyy}) ===");
            Console.WriteLine("1. Add book        6. Return book");
            Console.WriteLine("2. Add member      7. Overdue books");
            Console.WriteLine("3. List books      8. Skip days (test overdue)");
            Console.WriteLine("4. Search books    9. List members");
            Console.WriteLine("5. Borrow book     0. Exit");
            Console.Write("Choose: ");
        }

        static void AddMember()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? "";
            Member member = library.AddMember(name);
            Console.WriteLine($"Added: {member}");
        }

        static void ListBooks()
        {
            foreach (Book book in library.Books)
                Console.WriteLine(book);
        }

        static void ListMember()
        {
            foreach (Member member in library.Members)
            {
                Console.WriteLine(member);
                foreach (Book book in member.Borrowed)
                    Console.WriteLine($"  - {book.Title}");
            }
        }

        static void SearchBooks()
        {
            Console.Write("Search title or author: ");
            string text = Console.ReadLine() ?? "";
            List<Book> results = library.Search(text);

            if (results.Count == 0)
                Console.WriteLine("No books found.");
            else
                results.ForEach(Console.WriteLine);
        }

        static void BorrowBook()
        {
            int memberId = ReadInt("Member ID: ");
            int bookId = ReadInt("Book ID: ");
            library.Borrow(memberId, bookId);
            Console.WriteLine("Borrowed. Due in " + Library.LoanDays + " days.");
        }

        static void ReturnBook()
        {
            int bookId = ReadInt("Book ID: ");
            decimal fee = library.Return(bookId);
            Console.WriteLine(fee > 0 ? $"Returned. Late fee: Rs {fee:F2}" : "returned on time.");
        }

        static void ShowOverdue()
        {
            List<Book> overdue = library.GetOverdueBooks();
            if (overdue.Count == 0)
            {
                Console.WriteLine("No overdue books.");
                return;
            }
            foreach (Book book in overdue)
                Console.WriteLine($"{book} | fee so far: Rs {library.CalculateLateFee(book):F2}");
        }

        static void SkipDays()
        {
            int days = ReadInt("Days to skip: ");
            library.Today = library.Today.AddDays(days);
            Console.WriteLine($"Today is now {library.Today:dd-MMM-yyyy}");
        }

        static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value))
                    return value;
                Console.WriteLine("Please enter a number.");
            }
        }
    }
}
