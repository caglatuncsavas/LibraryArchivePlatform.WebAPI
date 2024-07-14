
using LibraryArchieve.WebAPI.Data;
using LibraryArchieve.WebAPI.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace LibraryArchieve.WebAPI.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private INoteRepository _notes;
    private IBookRepository _books;
    private IBookShelvesRepository _bookShelves;
    private IAuthRepository _auth;

    public UnitOfWork(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public INoteRepository Notes => _notes ??= new NoteRepository(_context);
    public IBookRepository Books => _books ??= new BookRepository(_context);
    public IBookShelvesRepository BookShelves => _bookShelves ??= new BookShelvesRepository(_context);
    public IAuthRepository Auth => _auth ??= new AuthRepository(_userManager, _context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
