using GraphQL_server.Models;
using Microsoft.AspNetCore.Mvc;

namespace GraphQL_server.Controllers
{
    public class Query
    {
        public List<Book> GetBooks()
        {
            var books = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Title = "The Great Gatsby",
                    Description = "A novel set in the Jazz Age exploring themes of wealth and identity.",
                    Author = "F. Scott Fitzgerald",
                    Price = 15.99m
                },
                new Book
                {
                    Id = 2,
                    Title = "To Kill a Mockingbird",
                    Description = "A classic novel about racial inequality and justice in the Deep South.",
                    Author = "Harper Lee",
                    Price = 12.49m
                },
                new Book
                {
                    Id = 3,
                    Title = "1984",
                    Description = "A dystopian novel about a totalitarian regime and its impacts on individual freedoms.",
                    Author = "George Orwell",
                    Price = 14.99m
                },
                new Book
                {
                    Id = 4,
                    Title = "Pride and Prejudice",
                    Description = "A timeless romance novel exploring societal expectations and personal pride.",
                    Author = "Jane Austen",
                    Price = 10.75m
                },
                new Book
                {
                    Id = 5,
                    Title = "The Hobbit",
                    Description = "A fantasy adventure about Bilbo Baggins and his quest to help reclaim a lost kingdom.",
                    Author = "J.R.R. Tolkien",
                    Price = 18.50m
                }
            };
            return books;
        }
    }
}
