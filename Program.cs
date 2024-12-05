using System;
using System.Collections.Generic;

class Book
{
    public int BookID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public string Genre { get; set; }
    public int Copies { get; set; }

    public Book(int bookID, string title, string author, string isbn, string genre, int copies)
    {
        BookID = bookID;
        Title = title;
        Author = author;
        ISBN = isbn;
        Genre = genre;
        Copies = copies;
    }
}

class Member
{
    public int MemberID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public Member(int memberID, string name, string email, string phone)
    {
        MemberID = memberID;
        Name = name;
        Email = email;
        Phone = phone;
    }
}

class Transaction
{
    public int TransactionID { get; set; }
    public Book Book { get; set; }
    public Member Member { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public double Fine { get; set; }

    public Transaction(int transactionID, Book book, Member member, DateTime issueDate, DateTime dueDate)
    {
        TransactionID = transactionID;
        Book = book;
        Member = member;
        IssueDate = issueDate;
        DueDate = dueDate;
        Fine = 0;
    }

    public void ReturnBook(DateTime returnDate)
    {
        ReturnDate = returnDate;
        if (returnDate > DueDate)
        {
            Fine = (returnDate - DueDate).Days * 10; // Fine: 10 units per day overdue
        }
    }
}

class Library
{
    private List<Book> books = new List<Book>();
    private List<Member> members = new List<Member>();
    private List<Transaction> transactions = new List<Transaction>();
    private int transactionCounter = 1;

    public void AddBook(Book book)
    {
        books.Add(book);
        Console.WriteLine("Book added successfully!");
    }

    public void AddMember(Member member)
    {
        members.Add(member);
        Console.WriteLine("Member added successfully!");
    }

    public void IssueBook(int bookID, int memberID)
    {
        var book = books.Find(b => b.BookID == bookID);
        var member = members.Find(m => m.MemberID == memberID);

        if (book == null || member == null)
        {
            Console.WriteLine("Book or Member not found!");
            return;
        }

        if (book.Copies > 0)
        {
            book.Copies--;
            var transaction = new Transaction(transactionCounter++, book, member, DateTime.Now, DateTime.Now.AddDays(14));
            transactions.Add(transaction);
            Console.WriteLine("Book issued successfully!");
        }
        else
        {
            Console.WriteLine("Book is not available.");
        }
    }

    public void ReturnBook(int transactionID, DateTime returnDate)
    {
        var transaction = transactions.Find(t => t.TransactionID == transactionID);
        if (transaction == null || transaction.ReturnDate != null)
        {
            Console.WriteLine("Invalid transaction!");
            return;
        }

        transaction.ReturnBook(returnDate);
        transaction.Book.Copies++;
        Console.WriteLine($"Book returned successfully! Fine: {transaction.Fine}");
    }

    public void ViewBooks()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("No books available in the library.");
            return;
        }

        Console.WriteLine("\nList of Books:");
        foreach (var book in books)
        {
            Console.WriteLine($"ID: {book.BookID}, Title: {book.Title}, Author: {book.Author}, Genre: {book.Genre}, ISBN: {book.ISBN}, Copies: {book.Copies}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Library library = new Library();
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\nLibrary Management System");
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. Add Member");
            Console.WriteLine("3. Issue Book");
            Console.WriteLine("4. Return Book");
            Console.WriteLine("5. View Books");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Write("Enter Book ID: ");
                    int bookID = int.Parse(Console.ReadLine());
                    Console.Write("Enter Title: ");
                    string title = Console.ReadLine();
                    Console.Write("Enter Author: ");
                    string author = Console.ReadLine();
                    Console.Write("Enter ISBN: ");
                    string isbn = Console.ReadLine();
                    Console.Write("Enter Genre: ");
                    string genre = Console.ReadLine();
                    Console.Write("Enter Number of Copies: ");
                    int copies = int.Parse(Console.ReadLine());

                    library.AddBook(new Book(bookID, title, author, isbn, genre, copies));
                    break;

                case 2:
                    Console.Write("Enter Member ID: ");
                    int memberID = int.Parse(Console.ReadLine());
                    Console.Write("Enter Name: ");
                    string name = Console.ReadLine();
                    Console.Write("Enter Email: ");
                    string email = Console.ReadLine();
                    Console.Write("Enter Phone: ");
                    string phone = Console.ReadLine();

                    library.AddMember(new Member(memberID, name, email, phone));
                    break;

                case 3:
                    Console.Write("Enter Book ID to Issue: ");
                    int issueBookID = int.Parse(Console.ReadLine());
                    Console.Write("Enter Member ID to Issue To: ");
                    int issueMemberID = int.Parse(Console.ReadLine());

                    library.IssueBook(issueBookID, issueMemberID);
                    break;

                case 4:
                    Console.Write("Enter Transaction ID to Return: ");
                    int transactionID = int.Parse(Console.ReadLine());
                    Console.Write("Enter Return Date (yyyy-mm-dd): ");
                    DateTime returnDate = DateTime.Parse(Console.ReadLine());

                    library.ReturnBook(transactionID, returnDate);
                    break;

                case 5:
                    library.ViewBooks();
                    break;

                case 6:
                    exit = true;
                    Console.WriteLine("Exiting the system. Goodbye!");
                    break;

                default:
                    Console.WriteLine("Invalid choice! Please try again.");
                    break;
            }
        }
    }
}

