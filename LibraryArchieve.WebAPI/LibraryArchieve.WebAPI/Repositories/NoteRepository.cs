using LibraryArchieve.WebAPI.Data;
using LibraryArchieve.WebAPI.Data.Entities;
using LibraryArchieve.WebAPI.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryArchieve.WebAPI.Repositories;

public class NoteRepository(ApplicationDbContext context) : INoteRepository
{
    public async Task<Note> CreateNoteAsync(Note note)
    {
        await context.Notes.AddAsync(note);
        await context.SaveChangesAsync();
        return note;
    }

    public async Task<bool> DeleteNoteAsync(int noteId)
    {
        var note = await context.Notes.FindAsync(noteId);
        if (note == null)
        {
            return false;
        }
        context.Notes.Remove(note);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Note> GetNoteByIdAsync(int noteId)
    {
        return await context.Notes.FindAsync(noteId);
    }

    public async Task<IEnumerable<Note>> GetNotesByBookIdAsync(int bookId, PrivacySetting privacySetting, Guid userId)
    {
        var query = context.Notes.AsQueryable();

        query = query.Where(note => note.BookId == bookId);

        if (privacySetting == PrivacySetting.Private)
        {
            query = query.Where(note => note.Privacy == PrivacySetting.Private);
        }

        if (privacySetting == PrivacySetting.FriendsOnly)
        {
            query = query.Where(note => note.Privacy == PrivacySetting.FriendsOnly);
        }

        if (privacySetting == PrivacySetting.Public)
        {
            query = query.Where(note => note.Privacy == PrivacySetting.Public);
        }

        return await query.ToListAsync();
    }

    public async Task<Note> UpdateNoteAsync(Note note)
    {
        context.Notes.Update(note);
        await context.SaveChangesAsync();
        return note;
    }
}
