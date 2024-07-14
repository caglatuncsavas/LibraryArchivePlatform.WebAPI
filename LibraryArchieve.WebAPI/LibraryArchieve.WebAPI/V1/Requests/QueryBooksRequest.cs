namespace LibraryArchieve.WebAPI.V1.Requests;

public class QueryBooksRequest
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? ISBN { get; set; }
    public int? CategoryId { get; set; }
    public string? ShelfLocation { get; set; }
    public bool? IsActive { get; set; }
}
