using LibraryArchive.Application.Services;
using LibraryArchive.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LibraryArchive.Application.Features.Auth;
internal sealed class LoginCommandHandler(
    UserManager<AppUser> userManager,
    IJwtprovider jwtProvider) : IRequestHandler<LoginCommand>
{
    public Task Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
