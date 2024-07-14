namespace LibraryArchieve.WebAPI.V1.Requests;

public class CreateBookRequest
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public string ShelfLocation { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public List<string> CategoryNames { get; set; } = new();
}
