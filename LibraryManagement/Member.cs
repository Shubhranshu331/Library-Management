using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement
{
    public class Member
    {
        private static int _nextId = 1001;
        private readonly List<Book> _borrowed = new List<Book>(); // list of borrowed books

        public const int MaxBooks = 3; // max books can be borrowed by one individual 

        public int Id { get; }
        public string Name { get; };
        public IReadOnlyList<Book> Borrowed => _borrowed; // showing borrowed books safely 

        public bool CanBorrow => _borrowed.Count < MaxBooks; // checking the borrowing limit 

        public Member(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArguementException("Member name is required."); // checking hwether the name is valid.

            Id = _nextId++;
            Name = name.Trim();
        }
        internal void AddBook(Book book) => _borrowed.Add(book); // adding a borrowed book 
        internal void RemoveBook(Book book) => _borrowed.Remove(book); // removing a borroweed book 

        public override string ToString()
        {
            return $"[{Id}] {Name} ({_borrowed.Count}/{MaxBooks} books)"; // displaying members
        }
    }
}
