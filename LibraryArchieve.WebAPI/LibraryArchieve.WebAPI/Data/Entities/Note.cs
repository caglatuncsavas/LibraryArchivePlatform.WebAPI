using LibraryArchieve.WebAPI.Data.Enums;

namespace LibraryArchieve.WebAPI.Data.Entities;

public sealed class Note
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public bool IsShared { get; set; }
    public PrivacySetting Privacy { get; set; }
    public Book Book { get; set; }
    public string Content { get; set; } = string.Empty;
}
