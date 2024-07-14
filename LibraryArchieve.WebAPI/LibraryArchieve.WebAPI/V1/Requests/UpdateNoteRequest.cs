namespace LibraryArchieve.WebAPI.V1.Requests;

public class UpdateNoteRequest
{
    public string Content { get; set; }
    public bool IsShared { get; set; }
}
