using LibraryArchieve.WebAPI.Data;
using LibraryArchieve.WebAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryArchieve.WebAPI.Repositories;

public class BookShelvesRepository(ApplicationDbContext context) : IBookShelvesRepository
{
    public async Task<BookShelf> CreateBookShelfAsync(BookShelf bookShelf)
    {
        await context.BookShelves.AddAsync(bookShelf);
        await context.SaveChangesAsync();
        return bookShelf;
    }
}
