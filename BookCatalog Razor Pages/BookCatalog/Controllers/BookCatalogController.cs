using BookCatalog.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Controllers;
public class BookCatalogController : Controller
{
    private readonly ILogger<BookCatalogController> _logger;
    private readonly static List<Book> books = new List<Book>()
    {
        new Book()
        {
            Id = 1,
            Title = "To Kill a Mockingbird",
            Author = "Harper Lee",
            PublicationYear = 1960,
            Genre = "Fiction",
            Price = 7.99m,
            Description = "A novel about the serious issues of race and class in the Deep South, seen through the eyes of a young girl."
        },
        new Book()
        {
            Id = 2,
            Title = "1984",
            Author = "George Orwell",
            PublicationYear = 1949,
            Genre = "Dystopian",
            Price = 8.99m,
            Description = "A chilling tale about totalitarianism and surveillance in a dystopian future."
        },
        new Book()
        {
            Id = 3,
            Title = "The Great Gatsby",
            Author = "F. Scott Fitzgerald",
            PublicationYear = 1925,
            Genre = "Classic",
            Price = 10.50m,
            Description = "A story about the American dream and the excesses of the Jazz Age, narrated by Nick Carraway."
        },
        new Book()
        {
            Id = 4,
            Title = "Pride and Prejudice",
            Author = "Jane Austen",
            PublicationYear = 1813,
            Genre = "Romance",
            Price = 6.99m,
            Description = "A romantic novel that deals with issues of class, marriage, and morality in early 19th-century England."
        },
        new Book()
        {
            Id = 5,
            Title = "The Catcher in the Rye",
            Author = "J.D. Salinger",
            PublicationYear = 1951,
            Genre = "Fiction",
            Price = 9.99m,
            Description = "A coming-of-age story that explores themes of identity, belonging, and alienation."
        }
    };

    public BookCatalogController(ILogger<BookCatalogController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAllBooks()
    {
        return View("List", books);
    }

    [HttpGet]
    public IActionResult GetBook(int id)
    {
        var res = books.FirstOrDefault(book => book.Id == id);
        if (res == null)
            return View("Error", $"Cannot find any record with id {id} :c");

        return View("Book", res);
    }

    public IActionResult UpdateBook(int id)
    {
        var res = books.FirstOrDefault(book => book.Id == id);
        if (res == null)
            return View("Error", $"Cannot find any record with id {id} :c");

        return View("Edit", res);
    }

    [HttpPost]
    [HttpPut]
    public IActionResult UpdateBook(Book bookToUpdate)
    {
        var book = books.FirstOrDefault(book => book.Id == bookToUpdate.Id);
        if (book == null)
            return View("Error", $"Cannot find any record with id {bookToUpdate.Id} :c");
        book.Title = bookToUpdate.Title;
        book.Author = bookToUpdate.Author;
        book.Genre = bookToUpdate.Genre;
        if (bookToUpdate.PublicationYear < 0 || bookToUpdate.PublicationYear > DateTime.Today.Year)
            return View("Error", $"Wrong record :c");
        book.PublicationYear = bookToUpdate.PublicationYear;
        if (bookToUpdate.Price < 0)
            return View("Error", $"Wrong record :c");
        book.Price = bookToUpdate.Price;

        return RedirectToAction("GetAllBooks");
    }

    public IActionResult CreateBook()
    {
        return View("Create");
    }

    [HttpPost]
    public IActionResult CreateBook(Book book)
    {
        book.Id = books.Last().Id + 1;
        books.Add(book);

        return RedirectToAction("GetAllBooks");
    }

    public IActionResult DeleteBook(int id)
    {
        var book = books.Find(x => x.Id == id);
        if (book == null)
            return View("Error", $"Cannot find any record with id {id} :c");

        books.Remove(book);
        return RedirectToAction("GetAllBooks");
    }
}

