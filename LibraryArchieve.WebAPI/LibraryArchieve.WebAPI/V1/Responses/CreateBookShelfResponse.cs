namespace LibraryArchieve.WebAPI.V1.Responses;

public class CreateBookShelfResponse
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;
    public string Shelf { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
