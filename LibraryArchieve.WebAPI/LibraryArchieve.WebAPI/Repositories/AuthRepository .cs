using LibraryArchieve.WebAPI.Data;
using LibraryArchieve.WebAPI.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryArchieve.WebAPI.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _context;

    public AuthRepository(UserManager<AppUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<AppUser> FindByUsernameOrEmailAsync(string usernameOrEmail)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == usernameOrEmail || u.Email == usernameOrEmail);
        return user;
    }

    public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IList<string>> GetUserRolesAsync(AppUser user)
    {
        var roles = await _context.AppUserRoles
            .Where(ur => ur.UserId == user.Id)
            .Include(ur => ur.Role)
            .Select(ur => ur.Role.Name)
            .ToListAsync();

        return roles.Cast<string>().ToList();
    }
}
