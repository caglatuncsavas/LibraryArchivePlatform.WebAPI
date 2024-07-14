using LibraryArchieve.WebAPI.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace LibraryArchieve.WebAPI.Repositories;

public interface IAuthRepository
{
    Task<AppUser> FindByUsernameOrEmailAsync(string usernameOrEmail);
    Task<IdentityResult> CreateUserAsync(AppUser user, string password);
    Task<IList<string>> GetUserRolesAsync(AppUser user);
}
