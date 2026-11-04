using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement
{
    internal class Book
    {
        private static int _nextId = 1;

        public int Id { get; }
        public string Title { get; }
        public string Author { get; }
        public Member? BorrowedBy { get; private set; }
        public DateTime? DueDate { get; private set; }
        public bool IsAvailable => BorrowedBy == null; // initially book is available py -> self.borrowedby = None

        public Book(string title, string author)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required");
            if (string.IsNullOrWhiteSpace(author))
                throw new ArguementException("Author name needed");

            Id = _nextId++;
            Title = title.Trim();
            Author = author.Trim();
        }

        public bool IsOverdue(DateTime today)
        {
            return DueDate.HasValue && today.Date > DueDate.Value.Date;  // self.duedate is not None and today.date() > self.duedate.date()
        }

        internal void CheckOut(Member member, DateTime dueDAte)
        {
            BorrowedBy = member;
            DueDate = dueDate;
        }

        internal void CheckIn()
        {
            BorrowedBy = null;
            DueDate = null;
        }

        public override string ToString() 
        {
            string status = IsAvailable
                ? "Available" 
                : $"Out ({BorrowedBy?.Name}, due {DueDate: dd-MMM-yyyy})"; // name of the borrower and the due date 
            return $"[{Id}] {Title} by {Author} - {status}"; 
        }
    }
}
