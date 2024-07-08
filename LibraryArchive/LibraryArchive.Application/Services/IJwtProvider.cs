using LibraryArchive.Application.Features.Auth;
using LibraryArchive.Domain.Entities;

namespace LibraryArchive.Application.Services;
public interface IJwtprovider
{
    Task<LoginCommandResponse> CreateToken(AppUser user);
}
