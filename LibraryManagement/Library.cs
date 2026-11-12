using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LibraryManagement
{
    public class Library
    {
        public const int LoanDays = 14;
        public const decimal LateFeePerDay = 2m;

        private readonly List<Book> _books = new List<Book>();
        private readonly Dictionary<int, Member> _members = new Dictionary<int, Member>();

        public DateTime Today { get; set; } = DateTime.Today; // read th value; change the value

        public IReadOnlyList<Book> Books => _books;
//      in python its quivalent to 
//       @property
//        def books(self) :
//            return self._books
        public IEnumerable<Member> Members => _members.Values; // acollection that can be looped through
    //  in python its similar to 
    //  for member in library.members:
    //      print(member.name)

        public Book AddBook(string title, string author)
        {
            var book = new Book(title, author);
            _books.Add(book);
            return book;
        }

        public Member AddMember(string name)
        {
            var member = new Member(name);
            _members[member.Id] = member;
            return member;
        }

        public Member? FindMember(int id)
        {
            return _members.TryGetValue(id, out Member? member) ? member : null;
        } 
        // python :  return self._members.get(id)

        public Book? FindBook(int id)
        {
            return _books.FirstOrDefault(b => b.Id == id); // LINQ stands for Language Integrated Query. 
            // go through the books and find the one that matches the condition 
            // if the requested id is same as the book then return b if not then null
//          for b in self._books:
//              if b.id == id:
//                  return b
//          return None
        }

        public List<Book> Search(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<Book>();
    //      if text is None or not text.strip():
    //          return []

            return _books
                .Where(b => b.Title.Contains(text, StringComparison.OrdinalIgnoreCase)
                            || b.Author.Contains(text, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
//          return [
//              b for b in self._books
//              if text.lower() in b.title.lower() or text.lower() in b.author.lower()
//          ]

        public void Borrow(int memberId, int bookId)
        {
            Member? member = FindMember(memberId);
            if (member == null) 
                throw new InvalidOperationException("Member not found.");

            Book? book = FindBook(bookId);
            if (book == null)
                throw new InvalidOperationException("Book not found.");

            if (!book.IsAvailable)
                throw new InvalidOperationException($"'{book.Title}' is already out.");

            if (!member.CanBorrow)
                throw new InvalidOperationException($"'{member.Name}' already has {Member.MaxBooks} books.");

            book.CheckOut(member, Today.AddDays(LoanDays));
//          book.check_out(
//              member,
//              today + timedelta(days = LOAN_DAYS)
//          )

            member.AddBook(book); // add book to the member
        }

        public decimal Return(int bookId)
        {
            Book? book = FindBook(bookId);
            if (book == null)
                throw new InvalidOperationException("Book not found.");

            if (book.IsAvailable)
                throw new InvalidOperationException($"'{book.Title}' is not borrowed.");

            decimal fee = CalculateLateFee(book);
            book.BorrowedBy!.RemoveBook(book);
            book.CheckIn();
            return fee;
        }

        public decimal CalculateLateFee(Book book)
        {
            if (!book.IsOverdue(Today))
                return 0m;

            int daysLate = (Today.Date - book.DueDate!.Value.Date).Days;
            // Duedate! : this might technically be nullable, but at this point I guarantee it is not null.
            return daysLate * LateFeePerDay;
        }

        public List<Book> GetOverdueBooks()
        {
            return _books.Where(b => b.IsOverdue(Today)).ToList();
        }
        // Go through all books, keep the ones that are overdue, and return them as a list.
    }
}