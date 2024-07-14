using LibraryArchieve.WebAPI.Data.Entities;

namespace LibraryArchieve.WebAPI.Repositories;

public interface IBookRepository
{
    Task<Book> CreateBookAsync(Book book);
    Task<Book> UpdateBookAsync(Book book);
    Task<Book> DeleteBookAsync(Book book);
    Task<Book> GetBookByIdAsync(int id); 
    Task<IEnumerable<Book>> QueryBooksAsync(string title, string author, string isbn, bool? isActive, int? categoryId, string shelfLocation);

}
