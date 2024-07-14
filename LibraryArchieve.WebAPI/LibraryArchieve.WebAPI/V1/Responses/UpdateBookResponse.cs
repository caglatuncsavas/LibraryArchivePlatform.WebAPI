namespace LibraryArchieve.WebAPI.V1.Responses;

public class UpdateBookResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Summary { get; set; }
    public string CoverImageUrl { get; set; }
    public string ShelfLocation { get; set; }
    public int Quantity { get; set; }
    public string ISBN { get; set; }
    public List<string> CategoryNames { get; set; }
}
