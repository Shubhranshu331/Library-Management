using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement
{
    public class Library
    {
        public const int LoanDays = 14;
        public const decimal LateFeePerDay = 2m;

        private readonly List<Book> _book = new List<Book>();
        private readonly Dictionary<int, Member> _members = new Dictionary<int, Member>();

        public DateTime Today { get; set; } = DateTime.Today;

        public IReadOnlyList<Book> Books => _books;
        public IEnumerable<Member> Members => _members.Values;

        public Book AddBook(string title, string author)
        {
            var book = new Book(title, author);
            _books.Add(book);
            return book;
        }

    }
}
