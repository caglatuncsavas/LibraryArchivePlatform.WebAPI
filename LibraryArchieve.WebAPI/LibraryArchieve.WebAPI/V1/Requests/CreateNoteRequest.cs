using LibraryArchieve.WebAPI.Data.Enums;

namespace LibraryArchieve.WebAPI.V1.Requests;

public class CreateNoteRequest
{
    public string Content { get; set; } = string.Empty;
    public bool IsShared { get; set; }
    public PrivacySetting Privacy { get; set; }
    public int BookId { get; set; }
}
