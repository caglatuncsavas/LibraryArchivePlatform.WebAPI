namespace LibraryArchieve.WebAPI.V1.Requests;

public class CreateBookShelfRequest
{
    public int BookId { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;  
    public string Shelf { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
