using LibraryArchieve.WebAPI.Data.Entities;
using LibraryArchieve.WebAPI.Data.Enums;

namespace LibraryArchieve.WebAPI.Repositories;

public interface INoteRepository
{
    Task<Note> CreateNoteAsync(Note note);
    Task<Note> UpdateNoteAsync(Note note);
    Task<bool> DeleteNoteAsync(int noteId);
    Task<Note> GetNoteByIdAsync(int noteId);
    Task<IEnumerable<Note>> GetNotesByBookIdAsync(int bookId, PrivacySetting privacySetting, Guid userId);
}
