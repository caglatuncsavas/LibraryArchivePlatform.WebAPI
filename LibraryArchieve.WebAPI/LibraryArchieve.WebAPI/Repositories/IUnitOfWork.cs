namespace LibraryArchieve.WebAPI.Repositories;

public interface IUnitOfWork
{
    IBookRepository Books { get; }
    IBookShelvesRepository BookShelves { get; }
    IAuthRepository Auth { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    INoteRepository Notes { get; }
}
