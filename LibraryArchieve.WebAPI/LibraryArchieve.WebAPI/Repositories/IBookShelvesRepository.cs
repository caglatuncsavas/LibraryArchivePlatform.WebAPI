using LibraryArchieve.WebAPI.Data.Entities;

namespace LibraryArchieve.WebAPI.Repositories;

public interface IBookShelvesRepository
{
    Task<BookShelf> CreateBookShelfAsync(BookShelf bookShelf);
}
