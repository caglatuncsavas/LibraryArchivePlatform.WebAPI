using LibraryArchieve.WebAPI.Data.Entities;
using LibraryArchieve.WebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryArchieve.WebAPI.Repositories;

public class BookRepository(ApplicationDbContext context) : IBookRepository
{
    public async Task<IEnumerable<Book>> QueryBooksAsync(string title, string author, string isbn, bool? isActive, int? categoryId, string shelfLocation)
    {
        var query = context.Books.AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(x => x.Title.Contains(title));
        }

        if (!string.IsNullOrEmpty(author))
        {
            query = query.Where(x => x.Author.Contains(author));
        }

        if (!string.IsNullOrEmpty(isbn))
        {
            query = query.Where(x => x.ISBN.Contains(isbn));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(b => b.BookCategories!.Any(bc => bc.CategoryId == categoryId.Value));
        }

        if (!string.IsNullOrEmpty(shelfLocation))
        {
            query = query.Where(x => x.ShelfLocation.Contains(shelfLocation));
        }

        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Book> CreateBookAsync(Book book)
    {
        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();
        return book;
    }
    public async Task<Book> UpdateBookAsync(Book book)
    {
        context.Books.Update(book);
        await context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> DeleteBookAsync(Book book)
    {
        book.IsActive = false;
        await context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> GetBookByIdAsync(int id)
    {
        return await context.Books
                 .Include(b => b.BookCategories)
                 .FirstOrDefaultAsync(b => b.Id == id);
    }
}