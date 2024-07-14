namespace LibraryArchieve.WebAPI.V1.Responses;

public class QueryBooksResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public string Category { get; set; }
    public string ShelfLocation { get; set; }
    public bool IsActive { get; set; }
}
