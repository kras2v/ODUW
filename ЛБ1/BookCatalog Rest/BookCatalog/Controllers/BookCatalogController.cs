using BookCatalog.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            Year = 1960,
            Genre = "Fiction",
            Price = 7.99m,
            Description = "A novel about the serious issues of race and class in the Deep South, seen through the eyes of a young girl."
        },
        new Book()
        {
            Id = 2,
            Title = "1984",
            Author = "George Orwell",
            Year = 1949,
            Genre = "Dystopian",
            Price = 8.99m,
            Description = "A chilling tale about totalitarianism and surveillance in a dystopian future."
        },
        new Book()
        {
            Id = 3,
            Title = "The Great Gatsby",
            Author = "F. Scott Fitzgerald",
            Year = 1925,
            Genre = "Classic",
            Price = 10.50m,
            Description = "A story about the American dream and the excesses of the Jazz Age, narrated by Nick Carraway."
        },
        new Book()
        {
            Id = 4,
            Title = "Pride and Prejudice",
            Author = "Jane Austen",
            Year = 1813,
            Genre = "Romance",
            Price = 6.99m,
            Description = "A romantic novel that deals with issues of class, marriage, and morality in early 19th-century England."
        },
        new Book()
        {
            Id = 5,
            Title = "The Catcher in the Rye",
            Author = "J.D. Salinger",
            Year = 1951,
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
        return Ok(books);
    }
    
    [HttpGet("{id}")]
    public IActionResult GetBook(int id)
    {
        var res = books.FirstOrDefault(book => book.Id == id);
        if (res == null)
            return BadRequest();

        return Ok(res);
    }

    [HttpPut]
    public IActionResult UpdateBook([FromBody]BookModel bookToUpdate)
    {

        var book = books.FirstOrDefault(book => book.Id == bookToUpdate.id);
        if (book == null)
            return BadRequest($"Cannot find book with id {bookToUpdate.id}");

        book.Title = bookToUpdate.title;
        book.Author = bookToUpdate.author;
        book.Genre = bookToUpdate.genre;
        book.Description = bookToUpdate.description;
        if (bookToUpdate.year < 0 || bookToUpdate.year > DateTime.Today.Year)
            return BadRequest($"Wrong values");
        book.Year = bookToUpdate.year;
        if (bookToUpdate.price < 0)
            return BadRequest($"Wrong values");
        book.Price = bookToUpdate.price;

        return Ok(bookToUpdate);
    }

    [HttpPost]
    public IActionResult CreateBook([FromBody]BookModel bookToCreate)
    {
        try
        {
            if (bookToCreate == null)
                return BadRequest();

            bookToCreate.id = books.Last().Id + 1;
            var book = new Book();
            book.Id = bookToCreate.id;
            book.Title = bookToCreate.title;
            book.Description = bookToCreate.description;
            book.Author = bookToCreate.author;
            book.Genre = bookToCreate.genre;
            book.Year = bookToCreate.year;
            book.Price = bookToCreate.price;
            books.Add(book);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        var book = books.Find(x => x.Id == id);
        if (book == null)
            return BadRequest(404);

        books.Remove(book);
        return Ok();
    }
}

