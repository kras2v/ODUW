namespace BookCatalog.Models;

public class BookModel
{
    public int id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string author { get; set; }
    public int year { get; set; } 
    public string genre { get; set; }
    public decimal price { get; set; }
}
