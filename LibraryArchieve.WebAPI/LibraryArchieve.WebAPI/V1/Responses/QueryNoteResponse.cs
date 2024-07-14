using LibraryArchieve.WebAPI.Data.Enums;

namespace LibraryArchieve.WebAPI.V1.Responses;

public class QueryNoteResponse
{
    public int Id { get; set; }
    public string Content { get; set; }
    public bool IsShared { get; set; }
    public PrivacySetting Privacy { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; }
    public string BookAuthor { get; set; }
}
