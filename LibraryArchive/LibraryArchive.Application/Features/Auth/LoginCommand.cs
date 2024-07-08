using MediatR;

namespace LibraryArchive.Application.Features.Auth;
public sealed record LoginCommand(
    string EmailOrUsername,
    string Password): IRequest<LoginCommandResponse>;